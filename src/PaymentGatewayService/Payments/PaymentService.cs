using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using PaymentGatewayService.Common.ServiceResponse;
using PaymentGatewayService.Payments.Commands;
using PaymentGatewayService.Payments.Validators;

namespace PaymentGatewayService.Payments
{
    public class PaymentService : IPaymentService
    {
        private readonly ILogger _logger;
        private readonly IPaymentRepository _repository;

        public PaymentService(
            ILogger<PaymentService> logger,
            IPaymentRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<ServiceResult<Payment>> Create(CreatePaymentCommand command)
        {
            var validator = new CreatePaymentCommandValidator();
            var validationResult = await validator.ValidateAsync(command);

            if (validationResult.IsValid == false)
            {
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

            return new ServiceResult<Payment>(payment);
        }

        public async Task<ServiceResult<Payment>> Get(GetPaymentCommand command)
        {
            var validator = new GetPaymentCommandValidator();
            var validationResult = await validator.ValidateAsync(command);

            if (validationResult.IsValid == false)
            {
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

        public async Task<ServiceResult> SetStatusAccepted(SetPaymentStatusAcceptedCommand command)
        {
            var validator = new SetPaymentStatusAcceptedCommandValidator();
            var validationResult = await validator.ValidateAsync(command);

            if (validationResult.IsValid == false)
            {
                _logger.LogError(
                    $"A validation error occurred while trying to set payment {command.Id.ToString()} as accepted.");

                return new ServiceResult<Payment>(ServiceErrorCode.ValidationError);
            }

            var payment = await _repository.Get(command.Id);

            if (payment == null)
            {
                _logger.LogError($"Payment with id {command.Id.ToString()} not found.");
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

        public async Task<ServiceResult> SetStatusRejected(SetPaymentStatusRejectedCommand command)
        {
            var validator = new SetPaymentStatusRejectedCommandValidator();
            var validationResult = await validator.ValidateAsync(command);

            if (validationResult.IsValid == false)
            {
                _logger.LogError(
                    $"A validation error occurred while trying to set payment {command.Id.ToString()} as rejected.");

                return new ServiceResult<Payment>(ServiceErrorCode.ValidationError);
            }

            var payment = await _repository.Get(command.Id);

            if (payment == null)
            {
                _logger.LogError($"Payment with id {command.Id.ToString()} not found.");
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