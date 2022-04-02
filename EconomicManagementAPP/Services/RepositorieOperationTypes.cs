using EconomicManagementAPP.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using EconomicManagementAPP.Interface;

namespace EconomicManagementAPP.Services
{
    public class RepositorieOperationTypes : IRepositorieOperationTypes
    {
        private readonly string connectionString;

        public RepositorieOperationTypes(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task Create(OperationTypes operationTypes)
        {
            using var connection = new SqlConnection(connectionString);
            // Requiere el await - tambien requiere el Async al final de la query
            var id = await connection.QuerySingleAsync<int>($@"INSERT INTO OperationTypes
                                                            ( Description)
                                                            VALUES (@Description);
                                                            SELECT SCOPE_IDENTITY();", operationTypes);
            operationTypes.Id = id;
        }

        public async Task Delete(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("DELETE OperationTypes WHERE Id = @Id", new { id });
        }

        public async Task<bool> Exist(string Description)
        {
            using var connection = new SqlConnection(connectionString);
            // El select 1 es traer lo primero que encuentre y el default es 0
            var exist = await connection.QueryFirstOrDefaultAsync<int>(
                                    @"SELECT 1
                                    FROM OperationTypes WHERE Description = @Description;",
                                    new { Description });
            return exist == 1;
        }

        public async Task<IEnumerable<OperationTypes>> GetOperation()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<OperationTypes>(@"SELECT Id,Description
                                                            FROM OperationTypes
                                                            ORDER BY Id");
        }

        public async Task<OperationTypes> GetOperationById(int id)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<OperationTypes>(@"
                                                                SELECT Id, Description
                                                                FROM OperationTypes AS ot
                                                                WHERE [ot].Id = @Id ",
                                                                new { id });
        }

        /*public async Task<OperationTypes> GetOperationById(int id)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<OperationTypes>(@"
                                                                SELECT [ot].Id, [ot].Description
                                                                FROM OperationTypes AS [ot]
                                                                JOIN Categories AS c
                                                                ON ot.Id=c.OperationTypeId
                                                                WHERE c.OperationTypeId = @id ",
                                                                new { id });
        }*/

        public async Task Modify(OperationTypes operationTypes)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE OperationTypes
                                            SET Description = @Description
                                            WHERE Id = @Id", operationTypes);
        }

        public async Task<string> GetOperationTypeByCategoryId(int categoryId, int userId)
        {
            using var connection = new SqlConnection(connectionString);
            var description = await connection.QuerySingleAsync<string>(@"SELECT o.Description
                                                                FROM Categories as c
                                                                JOIN OperationTypes as o
                                                                ON c.OperationTypeId=o.Id
                                                                WHERE c.Id=@categoryId  AND c.UserId = @UserId", new { categoryId, userId });
            return description;
        }
    }
}
