using EconomicManagementAPP.Validations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.DataAnnotations;

namespace EconomicManagemenAPP.test
{
    [TestClass]
    public class ValidationsPasswordTest
    {
        [TestMethod]
        public void insufficientCharacters_ReturnError()
        {
            var validationsPassword = new ValidationsPassword();
            var data = "contra";

            var context = new ValidationContext(new { Name = data });

            var testResult = validationsPassword.GetValidationResult(data, context);

            Assert.AreEqual("The password should have min eight characters", testResult?.ErrorMessage);
        }

        [TestMethod]

        public void NotNuber_ReturnError()
        {
            var validationsPassword = new ValidationsPassword();
            var data = "contraaaa";

            var context = new ValidationContext(new { Name = data });

            var testResult = validationsPassword.GetValidationResult(data, context);

            Assert.AreEqual("The password should have min one number", testResult?.ErrorMessage);
        }

        [TestMethod]

        public void NotCapitalLetter_ReturnError()
        {
            var validationsPassword = new ValidationsPassword();
            var data = "7contraaa";

            var context = new ValidationContext(new { Name = data });

            var testResult = validationsPassword.GetValidationResult(data, context);

            Assert.AreEqual("The password should have min one capital letter", testResult?.ErrorMessage);
        }
    }
}