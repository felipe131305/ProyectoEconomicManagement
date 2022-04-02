namespace EconomicManagementAPP.Models
{
    public class TransactionsView
    {
        public List<OperationTypes> OperationTypeDescription { get; set; }
        public List<OperationTypes> OperationTypeId { get; set; }
        public List<Categories> CategoryName { get; set; }
        public List<Categories> CategoryId { get; set; }
        public Transactions Transactions { get; set; }
    }
}
