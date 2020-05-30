using System.Linq;
using FluentValidation.Results;

namespace PaymentGatewayService.Common.Validation
{
    public static class ValidationResultExtensions
    {
        public static string ErrorsToString(this ValidationResult result)
        {
            if (result != null && !result.IsValid)
            {
                return string.Join(", ", result.Errors.Select(e => e.ToString()).ToArray());
            }

            return string.Empty;
        }
    }
}