using FluentValidation;
using PaymentGatewayService.Payments.Commands;

namespace PaymentGatewayService.Payments.Validators
{
    public class GetPaymentCommandValidator : AbstractValidator<GetPaymentCommand>
    {
        public GetPaymentCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("PAYMENT_ID_IS_MANDATORY");
        }
    }
}