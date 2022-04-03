using System.ComponentModel.DataAnnotations;

namespace EconomicManagementAPP.Validations
{
    public class MinZero:ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
           
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }

            double num;
            if (!Double.TryParse(value.ToString(), out num))
            {
                return new ValidationResult("The balance must be a decimal");
            }
    
            if (num < 0)
            {
                return new ValidationResult("The balance must be greater than zero");
            }

            return ValidationResult.Success;
        }
    }
}
