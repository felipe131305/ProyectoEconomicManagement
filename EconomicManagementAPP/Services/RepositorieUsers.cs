using Microsoft.Data.SqlClient;
using Dapper;
using EconomicManagementAPP.Models;
using EconomicManagementAPP.Interface;

namespace EconomicManagementAPP.Services
{
    
    public class RepositorieUsers : IRepositorieUsers
    {
        private readonly string connectionString;

        public RepositorieUsers(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<bool> ExistingUser(string Email)
        {
            using var connection = new SqlConnection(connectionString);
            var exists = await connection.QueryFirstOrDefaultAsync<int>(
                @"SELECT 1 
                FROM Users
                WHERE Email = @Email",
                new { Email }
                );
            return exists == 1;
        }
        public async Task Create(Users users)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>($@"INSERT INTO Users (Email, StandarEmail, Password, DbStatus)
                                                         VALUES (@Email, @StandarEmail, @Password, @DbStatus); SELECT SCOPE_IDENTITY();", users);
            users.Id = id;
        }

        public async Task<Users> Login(string email, string password)
        {
            using var connection = new  SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Users>("SELECT * FROM Users WHERE Email = @email AND Password = @password", 
                                                                    new { email, password });

        }

        public async Task<IEnumerable<Users>> GetUsers()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Users>(@"SELECT Id, Email, StandarEmail, Password
                                                        FROM Users 
                                                        WHERE DbStatus=1
                                                        ORDER BY 2");
                                                            
        }

        public async Task Modify(Users users)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE Users
                                            SET StandarEmail = @StandarEmail,
                                             Password = @Password
                                            WHERE DbStatus=1 AND Id = @Id", users);
        }

        //Para actualizar se necesita obtener el tipo de cuenta por el id
        public async Task<Users> GetUserById(int id)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Users>(@"
                                                                SELECT Id, Email, StandarEmail, Password
                                                                FROM Users
                                                                WHERE Id = @Id AND DbStatus=1",
                                                                new { id });
        }
        public async Task Delete(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("UPDATE SET DbStatus=0 FROM Users WHERE Id = @Id", new { id });
        }

        public bool Auth(ISession session)
        {
            throw new NotImplementedException();
        }
    }
}
