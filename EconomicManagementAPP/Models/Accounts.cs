using EconomicManagementAPP.Validations;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EconomicManagementAPP.Models
{
    public class Accounts
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        [FirstCapitalLetter]
        public string Name { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        public int AccountTypeId { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        [MinZero]
        public decimal Balance { get; set; }
        [MinZero]
        public string Money { get; set; }
        [StringLength(50)]
        public string Description { get; set; }
        public int NumberTransaction { get; set; }
        public Boolean DbStatus { set; get; }

    }
}
