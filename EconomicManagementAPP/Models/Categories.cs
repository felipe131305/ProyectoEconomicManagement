using EconomicManagementAPP.Validations;
using System.ComponentModel.DataAnnotations;

namespace EconomicManagementAPP.Models
{
    public class Categories
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        [FirstCapitalLetter]
        public string Name { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        public int OperationTypeId { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        public int UserId { get; set; }
        [Display(Name = "Operation type description")]
        public string OperationTypeDescription { get; set; }
        public IEnumerable<OperationTypes> OperationTypesList { get; set; }

    }
}
