using System.ComponentModel.DataAnnotations;

namespace TradingCompany.MVC.App.Validation
{
    public class DateRangeAttribute : ValidationAttribute
    {
        private readonly DateTime _minDate;
        private readonly DateTime _maxDate;

        public DateRangeAttribute(string minDate, string maxDate)
        {
            _minDate = DateTime.Parse(minDate);
            _maxDate = DateTime.Parse(maxDate);
            ErrorMessage = $"Date must be between {_minDate:yyyy-MM-dd} and {_maxDate:yyyy-MM-dd}";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime date)
            {
                if (date >= _minDate && date <= _maxDate)
                    return ValidationResult.Success;
            }
            return new ValidationResult(ErrorMessage);
        }
    }
}