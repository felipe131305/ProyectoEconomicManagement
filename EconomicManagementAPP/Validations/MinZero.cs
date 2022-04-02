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
            //double num = Double.Parse(value.ToString());
            if (!Double.TryParse(value.ToString(), out num))
            {
                return new ValidationResult("The balance must be a decimal");
            }
           // value = num;
          //  Console.WriteLine(value);
            if (num < 0)
            {
                return new ValidationResult("The balance must be greater than zero");
            }

            return ValidationResult.Success;
        }
    }
}
