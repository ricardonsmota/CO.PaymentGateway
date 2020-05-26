using System.Text.RegularExpressions;
using AutoMapper;
using PaymentGatewayService.Api.ViewModels;
using PaymentGatewayService.Payments;

namespace PaymentGatewayService.Api
{
    public class AutoMapperApiProfile : Profile
    {
        public AutoMapperApiProfile()
        {
            CreateMap<Payment, PaymentViewModel>()
                .ForMember(x => x.CardNumber, d => d.MapFrom(v => MaskCardNumber(v.CardNumber)));

            CreateMap<PaymentStatus, PaymentStatusViewModel>()
                .ForMember(x => x.StatusCode, d => d.MapFrom(v => v.StatusCode.ToString()));
        }

        private string MaskCardNumber(string cardNumber)
        {
            var firstDigits = cardNumber.Substring(0, 6);
            var lastDigits = cardNumber.Substring(cardNumber.Length - 4, 4);

            var requiredMask = new string('X', cardNumber.Length - firstDigits.Length - lastDigits.Length);

            var maskedString = string.Concat(firstDigits, requiredMask, lastDigits);
            return Regex.Replace(maskedString, ".{4}", "$0 ");
        }
    }
}