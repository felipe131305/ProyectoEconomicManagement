namespace EconomicManagementAPP.Models
{
    public class AccountAndAccountTypes
    {
        public string UserName { get; set; }
        public IEnumerable<Accounts> Accounts { get; set; }
        public IEnumerable<AccountTypes> AccountTypes { get; set;}
    }
}
