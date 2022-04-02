using EconomicManagementAPP.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using EconomicManagementAPP.Interface;

namespace EconomicManagementAPP.Services
{
   
    public class RepositorieAccountTypes : IRepositorieAccountTypes
    {
        private readonly string connectionString;

        public RepositorieAccountTypes(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        // El async va acompañado de Task
        public async Task<int> Create(AccountTypes accountTypes)
        {
            using var connection = new SqlConnection(connectionString);
            // Requiere el await - tambien requiere el Async al final de la query
            var id = await connection.QuerySingleAsync<int>($@"INSERT INTO AccountTypes 
                                                (Name, UserId, OrderAccount, DbStatus) 
                                                VALUES (@Name, @UserId, @OrderAccount, @DbStatus); SELECT SCOPE_IDENTITY();", accountTypes);
            accountTypes.Id = id;
            return accountTypes.Id;
        }

        //Cuando retorna un tipo de dato se debe poner en el Task Task<bool>
        public async Task<bool> Exist(string Name, int UserId)
        {
            using var connection = new SqlConnection(connectionString);
            // El select 1 es traer lo primero que encuentre y el default es 0
            var exist = await connection.QueryFirstOrDefaultAsync<int>(
                                    @"SELECT 1
                                    FROM AccountTypes
                                    WHERE Name = @Name AND UserId = @UserId;",
                                    new { Name, UserId });
            return exist == 1;
        }

        // Obtenemos las cuentas del usuario
        public async Task<IEnumerable<AccountTypes>> GetAccountsTypes(int UserId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<AccountTypes>(@"SELECT  [at].Id ,[at].[Name] AS 'Name' , UserId, OrderAccount
                                                             FROM AccountTypes AS [at]
                                                             JOIN Users AS u
                                                             ON u.Id=[at].UserId
                                                             WHERE [at].UserId=@UserId AND [at].DbStatus=1 AND u.DbStatus=1", new { UserId });
        }
        // Actualizar
        public async Task Modify(AccountTypes accountTypes)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE AccountTypes
                                            SET Name = @Name
                                            WHERE Id = @Id AND DbStatus=1", accountTypes);
        }

        //Para actualizar se necesita obtener el tipo de cuenta por el id
        public async Task<AccountTypes> GetAccountById(int id, int userId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<AccountTypes>(@"
                                                                SELECT [at].Id, Name, UserId, OrderAccount
                                                                FROM AccountTypes AS [at]
                                                                JOIN Users AS u
                                                                ON u.Id=[at].UserId
                                                                WHERE [at].Id = @Id AND UserID = @UserID AND [at].DbStatus=1 AND u.DbStatus=1",
                                                                new { id, userId });
        }

        //Eliminar
        public async Task DeleteModify(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("UPDATE AccountTypes SET DbStatus=0  WHERE Id = @Id", new { id });
        }
        public async Task<bool> ExistingAccountType(int AccountTypeId)
        {
            using var connection = new SqlConnection(connectionString);
            var exist = await connection.QueryFirstOrDefaultAsync<int>(
                                @"SELECT 1
                                    FROM Accounts
                                    WHERE AccountTypeId = @AccountTypeId GROUP BY  AccountTypeId;",
                                new { AccountTypeId });
            return exist == 1;

        }
        public async Task Delete(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("DELETE  FROM AccountTypes WHERE Id = @Id", new { id });
        }

        public async Task<int> GetNumberAccount(int id)
        {
            Console.WriteLine(id);
            using var connection = new SqlConnection(connectionString);
            int numberAccount = await connection.QuerySingleAsync<int>("SELECT COUNT (AccountTypeId) AS CantidadCuentas  FROM Accounts WHERE AccountTypeId = @Id;", new { id });
            Console.WriteLine(numberAccount);

            return numberAccount;
        }


    }
}
