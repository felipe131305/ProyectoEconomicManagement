using EconomicManagementAPP.Models;

namespace EconomicManagementAPP.Interface
{
    public interface IRepositorieOperationTypes
    {
        Task Create(OperationTypes operationTypes); // Se agrega task por el asincronismo
        Task<bool> Exist(string Description);
        Task<IEnumerable<OperationTypes>> GetOperation();
        Task Modify(OperationTypes operationTypes);
        Task<OperationTypes> GetOperationById(int id); // para el modify
        Task Delete(int id);
        Task<string> GetOperationTypeByCategoryId(int categoryId, int userId);
    }
}
