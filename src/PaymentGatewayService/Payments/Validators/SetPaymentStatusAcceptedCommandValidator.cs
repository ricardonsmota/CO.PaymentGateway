using FluentValidation;
using PaymentGatewayService.Payments.Commands;

namespace PaymentGatewayService.Payments.Validators
{
    public class SetPaymentStatusAcceptedCommandValidator : AbstractValidator<SetPaymentStatusAcceptedCommand>
    {
        public SetPaymentStatusAcceptedCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("PAYMENT_ID_IS_MANDATORY");
        }
    }
}