using FluentValidation;
using PaymentGatewayService.Payments.Commands;

namespace PaymentGatewayService.Payments.Validators
{
    public class CreatePaymentCommandValidator : AbstractValidator<CreatePaymentCommand>
    {
    }
}