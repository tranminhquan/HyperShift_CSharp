using System.Globalization;
using System.Windows.Controls;

namespace HyberShift_CSharp.Domain
{
    public class NotEmptyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var str = value as string;

            if (!string.IsNullOrEmpty(str))
                return ValidationResult.ValidResult;
            return new ValidationResult(false, "Field is required");
        }
    }
}