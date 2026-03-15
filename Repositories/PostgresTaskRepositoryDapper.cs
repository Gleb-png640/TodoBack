using Dapper;
using Npgsql;
using System.Data;
using TodoBack.Dtos.Tasks;
using TodoBack.Models.Tasks;
using TodoBack.QueryParameters;

namespace TodoBack.Repositories
{
    public class PostgresTaskRepositoryDapper : ITaskRepository
    {

        private readonly IConfiguration _configuration;

        public PostgresTaskRepositoryDapper(IConfiguration configuration) 
        {
            _configuration = configuration;
        }


        public Task<UserTask> AddAsync(UserTask task)
        {
            throw new NotImplementedException();
        }

        public Task<UserTask> ChangeExistingTaskAsync(UserTask task, UpdateUserTaskDto taskDto)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAsync(UserTask task)
        {

            using (var connection = GetConnection()) 
            {
                const string sql =
                    """
                    DELETE 
                    FROM "Tasks"
                    WHERE "TaskId" = @taskId
                    """;

                var res = await connection.ExecuteAsync(sql, new { taskId = task.TaskId });

                return res == 1 ? true : false; 
            }
        }

        public async Task<UserTask?> GetByIdAsync(int id, Guid UserId)
        {
            using (var connection = GetConnection()) 
            {
                const string sql =
                    """
                    SELECT * 
                    FROM "Tasks"
                    WHERE "UserId" = @UserId AND "TaskId" = @id
                    """;

                return await connection.QueryFirstOrDefaultAsync<UserTask?>(sql, new 
                    {
                        UserId = UserId,
                        id = id
                    });
            }
        }

        public async Task<UserTask?> GetByIdTrackedAsync(int id, Guid UserId)
        {
            using (var connection = GetConnection())
            {
                const string sql =
                    """
                    SELECT * 
                    FROM "Tasks"
                    WHERE "UserId" = @UserId AND "TaskId" = @id
                    """;

                return await connection.QueryFirstOrDefaultAsync<UserTask?>(sql, new
                {
                    UserId = UserId,
                    id = id
                });
            }
        }

        public async Task<IEnumerable<UserTask>> GetPagedAsync(GetPageQuery query, Guid UserId)
        {
            using (var connection = GetConnection())
            {
                const string sql =
                """
                    SELECT  *
                    FROM "Tasks"
                    WHERE "UserId" = @UserId
                    ORDER BY "TaskId"
                    LIMIT @PageSize OFFSET @Offset
                """;

                return await connection.QueryAsync<UserTask>(sql, new
                {
                    UserId = UserId,
                    PageSize = query.pageSize,
                    Offset = query.pageSize * (query.page - 1)
                });
            }
        }


        private IDbConnection GetConnection()
        {
            var connString = _configuration.GetConnectionString("DefaultConnection");

            return new NpgsqlConnection(connString);
        }
    }
}
