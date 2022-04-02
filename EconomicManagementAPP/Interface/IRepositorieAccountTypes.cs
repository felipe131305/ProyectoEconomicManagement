using EconomicManagementAPP.Models;

namespace EconomicManagementAPP.Interface
{
    public interface IRepositorieAccountTypes
    {
        Task<int> Create(AccountTypes accountTypes); // Se agrega task por el asincronismo
        Task<bool> Exist(string Name, int UserId);
        Task<IEnumerable<AccountTypes>> GetAccountsTypes(int UserId = 1);
        Task Modify(AccountTypes accountTypes);
        Task<AccountTypes> GetAccountById(int id, int userId = 1); // para el modify
        Task<int> GetNumberAccount(int id); //para saber cuantas cuentas tiene el tipo de cuenta
        Task Delete(int id);
        Task DeleteModify(int id);
        Task<bool> ExistingAccountType(int AccountTypeId);
    }
}
