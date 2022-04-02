using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
namespace EconomicManagementAPP.Validations
{
    public class ValidationsPassword : ValidationAttribute
    {

        protected override ValidationResult IsValid(Object value, ValidationContext validationContext)
        {
            if(value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return new ValidationResult("The password must exits");
            }
            string password = value.ToString();
            int length = password.Length;
            if (length < 8)
            {
                return new ValidationResult("The password should have min eight characters");
            }

            Regex rx = new Regex("[0-9]");


            if (length < 8)
            {
                return new ValidationResult("The password should have min eight characters");
            }
            Boolean flag = false;
            for (int i = 0; i < length; i++)
            {

                if (rx.IsMatch(password.ToString()[i].ToString()))
                {
                    flag = true;
                    break;
                }
            }

            if (!flag)
            {
                return new ValidationResult("The password should have min one number ");
            }
            flag = false;
            Regex rxCapital = new Regex("[A-Z]");

            for (int i = 0; i < length; i++)
            {

                if (rxCapital.IsMatch(password.ToString()[i].ToString()))
                {
                    flag = true;
                    break;
                }
            }

            if (!flag)
            {
                return new ValidationResult("The password should have min one capital letter");
            }


            return ValidationResult.Success;
        }

    }
}
