using EconomicManagementAPP.Validations;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EconomicManagementAPP.Models
{
    public class Users
    {
        public int Id { set; get; }
        [Required(ErrorMessage="{0} is required")]
        [Remote(action: "VerifyUser", controller:"Users")]
        public string Email { set; get; }
        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Standar email")]
        public string StandarEmail { set; get; }
        [Required(ErrorMessage = "{0} is required")]
        [ValidationsPassword]
        public string Password { set; get; }
        public Boolean DbStatus { set; get; }
    }
}
