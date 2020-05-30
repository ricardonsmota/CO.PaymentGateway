using System;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using PaymentGatewayService.AcquiringBank;
using PaymentGatewayService.AcquiringBank.Commands;
using PaymentGatewayService.Common.ServiceResponse;
using PaymentGatewayService.Common.Validation;
using PaymentGatewayService.Payments.Commands;

namespace PaymentGatewayService.Payments
{
    public class PaymentService : IPaymentService
    {
        private readonly ILogger _logger;
        private readonly IValidatorFactory _validator;
        private readonly IPaymentRepository _repository;
        private readonly IAcquiringBankService _acquiringBankService;

        public PaymentService(
            ILogger<PaymentService> logger,
            IValidatorFactory validator,
            IPaymentRepository repository,
            IAcquiringBankService acquiringBankService)
        {
            _logger = logger;
            _validator = validator;
            _repository = repository;
            _acquiringBankService = acquiringBankService;
        }

        public async Task<ServiceResult<Payment>> Create(CreatePaymentCommand command)
        {
            var validator = _validator.GetValidator<CreatePaymentCommand>();
            var validationResult = await validator.ValidateAsync(command);

            if (validationResult.IsValid == false)
            {
                _logger.LogWarning($"Validation Error: {validationResult.ErrorsToString()}");
                return new ServiceResult<Payment>(ServiceErrorCode.ValidationError);
            }

            var payment = new Payment()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Amount = command.Amount,
                CardNumber = command.CardNumber,
                Created = DateTime.UtcNow,
                Currency = command.Currency,
                Cvv = command.Cvv,
                ExpirationMonth = command.ExpirationMonth,
                ExpirationYear = command.ExpirationYear,
                Status = new PaymentStatus()
                {
                    Modified = DateTime.UtcNow,
                    StatusCode = PaymentStatusCode.Processing
                }
            };

            await _repository.Create(payment);

            /* In an ideal scenario I would publish an event to a message broker stating that a payment
            was created. This would be consumed by the AcquiringBankWorker and the logic dealt there */

#pragma warning disable 4014
            /* We dont want to wait, not a good practice but it is just for mock purposes. */
            Task.Run(async () =>
#pragma warning restore 4014
            {
                var transactionResponse = await _acquiringBankService.StartTransaction(new StartTransactionCommand()
                {
                    Amount = command.Amount,
                    PaymentId = payment.Id
                });

                if (transactionResponse.IsSuccess)
                {
#pragma warning disable 4014
                    /* We dont want to wait, not a good practice but it is just for mock purposes. */
                    SetStatusAccepted(new SetPaymentStatusAcceptedCommand()
#pragma warning restore 4014
                    {
                        Id = payment.Id,
                        Modified = DateTime.UtcNow
                    });
                }
                else
                {
#pragma warning disable 4014
                    /* We dont want to wait, not a good practice but it is just for mock purposes. */
                    SetStatusRejected(new SetPaymentStatusRejectedCommand()
#pragma warning restore 4014
                    {
                        Id = payment.Id,
                        Modified = DateTime.UtcNow,
                        ErrorMessage = transactionResponse.ErrorMessage
                    });
                }
            });

            return new ServiceResult<Payment>(payment);
        }

        public async Task<ServiceResult<Payment>> Get(GetPaymentCommand command)
        {
            var validator = _validator.GetValidator<GetPaymentCommand>();
            var validationResult = await validator.ValidateAsync(command);

            if (validationResult.IsValid == false)
            {
                _logger.LogWarning($"Validation Error: {validationResult.ErrorsToString()}");
                return new ServiceResult<Payment>(ServiceErrorCode.ValidationError);
            }

            var payment = await _repository.Get(command.Id);

            if (payment == null)
            {
                _logger.LogWarning($"Payment with id {command.Id.ToString()} not found.");
                return new ServiceResult<Payment>(ServiceErrorCode.NotFound);
            }

            return new ServiceResult<Payment>(payment);
        }

        public async Task<ServiceResult<Payment>> SetStatusAccepted(SetPaymentStatusAcceptedCommand command)
        {
            var validator = _validator.GetValidator<SetPaymentStatusAcceptedCommand>();
            var validationResult = await validator.ValidateAsync(command);

            if (validationResult.IsValid == false)
            {
                _logger.LogError(
                    $"A validation error occurred while trying to set payment {command.Id} as accepted.");
                return new ServiceResult<Payment>(ServiceErrorCode.ValidationError);
            }

            var payment = await _repository.Get(command.Id);

            if (payment == null)
            {
                _logger.LogError($"Payment with id {command.Id} not found.");
                return new ServiceResult<Payment>(ServiceErrorCode.NotFound);
            }

            payment.Status = new PaymentStatus()
            {
                StatusCode = PaymentStatusCode.Accepted,
                Modified = command.Modified
            };

            await _repository.Save(payment);

            return new ServiceResult<Payment>(payment);
        }

        public async Task<ServiceResult<Payment>> SetStatusRejected(SetPaymentStatusRejectedCommand command)
        {
            var validator = _validator.GetValidator<SetPaymentStatusRejectedCommand>();
            var validationResult = await validator.ValidateAsync(command);

            if (validationResult.IsValid == false)
            {
                _logger.LogError(
                    $"A validation error occurred while trying to set payment {command.Id} as rejected.");

                return new ServiceResult<Payment>(ServiceErrorCode.ValidationError);
            }

            var payment = await _repository.Get(command.Id);

            if (payment == null)
            {
                _logger.LogError($"Payment with id {command.Id} not found.");
                return new ServiceResult<Payment>(ServiceErrorCode.NotFound);
            }

            payment.Status = new PaymentStatus()
            {
                StatusCode = PaymentStatusCode.Rejected,
                ErrorMessage = command.ErrorMessage,
                Modified = command.Modified
            };

            await _repository.Save(payment);

            return new ServiceResult<Payment>(payment);
        }
    }
}