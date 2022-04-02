using EconomicManagementAPP.Validations;
using System.ComponentModel.DataAnnotations;

namespace EconomicManagementAPP.Models
{
    public class OperationTypes
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        [FirstCapitalLetter]
        public string Description { get; set; }
    }
}
