using System.ComponentModel.DataAnnotations;

namespace EconomicManagementAPP.Models
{
    public class LoginViewModel
    {

        [Required(ErrorMessage = "{0} is required")]
        [EmailAddress(ErrorMessage ="Invalid format Email")]
        public string Email { set; get; }
        [Required(ErrorMessage = "{0} is required")]
        [DataType(DataType.Password)]
        public string Password { set; get; }
        public bool sesion { get; set; }
    }
}
