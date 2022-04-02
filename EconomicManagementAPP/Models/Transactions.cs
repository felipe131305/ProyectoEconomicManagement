using EconomicManagementAPP.Validations;
using System.ComponentModel.DataAnnotations;

namespace EconomicManagementAPP.Models
{
    public class Transactions
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        public int UserId { get; set; }
        public DateTime TransactionDate { get; set; }
        [MinZero]
        public decimal Total { get; set; }
        public string Description { get; set; }
        public int AccountId { get; set; }
        [Display(Name = "Category")]
        [Required(ErrorMessage = "{0} is required")]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public IEnumerable<Categories> CategoryList { get; set; }
        public string OperationTypeDescription { get; set; }
        public string AccountName { get; set; }

    }
}
