using System;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PaymentGatewayService.Common.ServiceResponse;
using PaymentGatewayService.Payments.Commands;

namespace PaymentGatewayService.Payments
{
    public class PaymentService : IPaymentService
    {
        private readonly ILogger _logger;
        private readonly IValidator _validator;
        private readonly IPaymentRepository _repository;

        public PaymentService(
            ILogger logger,
            IValidator validator,
            IPaymentRepository repository)
        {
            _logger = logger;
            _validator = validator;
            _repository = repository;
        }

        public async Task<ServiceResult<Payment>> Create(CreatePaymentCommand command)
        {
            var validationResult = await _validator.ValidateAsync(command);

            if (validationResult.IsValid == false)
            {
                return new ServiceResult<Payment>(ServiceErrorCode.ValidationError);
            }

            var payment = new Payment()
            {
                Id = Guid.NewGuid(),
            };

            // Repository CREATE

            return new ServiceResult<Payment>(payment);
        }

        public async Task<ServiceResult<Payment>> Get(GetPaymentCommand command)
        {
            var validationResult = await _validator.ValidateAsync(command);

            if (validationResult.IsValid == false)
            {
                return new ServiceResult<Payment>(ServiceErrorCode.ValidationError);
            }

            var payment = await _repository.Get();

            if (payment == null)
            {
                _logger.LogWarning($"Payment with id {command.Id.ToString()} not found.");
                return new ServiceResult<Payment>(ServiceErrorCode.NotFound);
            }

            // Repository GET

            return new ServiceResult<Payment>(payment);
        }

        public async Task<ServiceResult> SetStatusAccepted(SetPaymentStatusAcceptedCommand command)
        {
            var validationResult = await _validator.ValidateAsync(command);

            if (validationResult.IsValid == false)
            {
                _logger.LogError(
                    $"A validation error occurred while trying to set payment {command.Id.ToString()} as accepted.");

                return new ServiceResult<Payment>(ServiceErrorCode.ValidationError);
            }

            var payment = await _repository.Get();

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

            // Repository Save.

            return new ServiceResult<Payment>(payment);
        }

        public async Task<ServiceResult> SetStatusRejected(SetPaymentStatusRejectedCommand command)
        {
            var validationResult = await _validator.ValidateAsync(command);

            if (validationResult.IsValid == false)
            {
                _logger.LogError(
                    $"A validation error occurred while trying to set payment {command.Id.ToString()} as rejected.");

                return new ServiceResult<Payment>(ServiceErrorCode.ValidationError);
            }

            var payment = await _repository.Get();

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

            // Repository Save.

            return new ServiceResult<Payment>(payment);
        }
    }
}