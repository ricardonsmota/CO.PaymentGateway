using FluentValidation;
using PaymentGatewayService.Payments.Commands;

namespace PaymentGatewayService.Payments.Validators
{
    public class SetPaymentStatusRejectedCommandValidator : AbstractValidator<SetPaymentStatusRejectedCommand>
    {
        public SetPaymentStatusRejectedCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("PAYMENT_ID_IS_MANDATORY");
        }
    }
}