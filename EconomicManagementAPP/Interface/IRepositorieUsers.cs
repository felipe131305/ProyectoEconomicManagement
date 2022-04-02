using EconomicManagementAPP.Models;

namespace EconomicManagementAPP.Interface
{
    public interface IRepositorieUsers
    {
        Task Create(Users users);
        Task<bool> ExistingUser(string Email);
        Task<IEnumerable<Users>> GetUsers();
        Task Modify(Users users);
        Task<Users> GetUserById(int id); // para el modify
        Task Delete(int id);
        Task<Users> Login(string email, string password);
        bool Auth(ISession session);
    }
}
