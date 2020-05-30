using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using PaymentGatewayService.Payments.Commands;

namespace PaymentGatewayService.Payments.Validators
{
    public class CreatePaymentCommandValidator : AbstractValidator<CreatePaymentCommand>
    {
        private List<string> _validCurrencies = new List<string>()
        {
            "EUR", "USD", "NOK"
        };

        public CreatePaymentCommandValidator()
        {
            RuleFor(x => x.Amount).NotEmpty().WithMessage("AMOUNT_IS_MANDATORY");

            RuleFor(x => x.Currency).Must(BeValidCurrency).WithMessage("INVALID_CURRENCY");

            RuleFor(x => x.ExpirationYear).NotEmpty().WithMessage("EXPIRATION_YEAR_IS_MANDATORY");

            RuleFor(x => x.ExpirationMonth).NotEmpty().WithMessage("EXPIRATION_MONTH_IS_MANDATORY");

            RuleFor(x => x.Cvv).Must(BeValidCvv).WithMessage("INVALID_CVV");

            RuleFor(x => x.CardNumber).Must(BeValidCardNumber).WithMessage("INVALID_CARD_NUMBER");
        }

        private bool BeValidCurrency(string currency)
        {
            return !string.IsNullOrEmpty(currency) && _validCurrencies.Contains(currency);
        }

        private bool BeValidCvv(string cvv)
        {
            return !string.IsNullOrEmpty(cvv)
                   && cvv.All(char.IsDigit) && cvv.Length == 3;        }

        private bool BeValidCardNumber(string cardNumber)
        {
            return !string.IsNullOrEmpty(cardNumber)
                   && cardNumber.All(char.IsDigit) && cardNumber.Length == 16;
        }
    }
}