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

        public async Task<ServiceResponse<Payment>> Create(CreatePaymentCommand command)
        {
            var validationResult = await _validator.ValidateAsync(command);

            if (validationResult.IsValid == false)
            {
                return new ServiceResponse<Payment>(ServiceErrorCode.ValidationError);
            }

            var payment = new Payment();

            throw new System.NotImplementedException();
        }

        public async Task<ServiceResponse<Payment>> Get(GetPaymentCommand command)
        {
            var validationResult = await _validator.ValidateAsync(command);

            if (validationResult.IsValid == false)
            {
                return new ServiceResponse<Payment>(ServiceErrorCode.ValidationError);
            }

            var payment = await _repository.Get();

            if (payment == null)
            {
                _logger.LogWarning($"Payment with id {command} not found.");
                return new ServiceResponse<Payment>(ServiceErrorCode.NotFound);
            }

            return new ServiceResponse<Payment>(payment);
        }
    }
}