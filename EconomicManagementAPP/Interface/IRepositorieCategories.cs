using EconomicManagementAPP.Models;
using System.Data;

namespace EconomicManagementAPP.Interface
{
    public interface IRepositorieCategories
    {
        Task Create(Categories categories); // Se agrega task por el asincronismo
        Task<bool> ExistingCategories(string name, int operationTypeId, int userId);
        Task<IEnumerable<Categories>> GetCategories(int userId);
        Task Modify(Categories categories);
        Task<Categories> GetCategoriesById(int id); // para el modify
        Task Delete(int id);
        Task<Categories> GetCategorieByIds( int categoryId, int userId);
    }
}
