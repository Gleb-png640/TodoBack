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


        public async Task<UserTask> AddAsync(UserTask task)
        {
            const string sql =
                """
                INSERT INTO "Tasks" ("TaskId", "Name", "Description", "IsDone", "UserId", "CreatedAt", "DueTo", "From", "TaskType")
                VALUES (@TaskId, @Name, @Description, @IsDone, @UserId, @CreatedAt, @DueTo, @From, @TaskType)
                """;

            using (var connection = GetConnection()) 
            {
                var res = await connection.ExecuteAsync(sql, task);

                if (res != 1) { throw new Exception("Number of tasks added to bd is not equal to 1"); }

                return task;
            }
        }

        public Task<UserTask> ChangeExistingTaskAsync(UserTask task, UpdateUserTaskDto taskDto)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAsync(UserTask task)
        {
            const string sql =
                    """
                    DELETE 
                    FROM "Tasks"
                    WHERE "TaskId" = @taskId
                    """;

            using (var connection = GetConnection()) 
            {

                var res = await connection.ExecuteAsync(sql, new { taskId = task.TaskId });

                return res == 1 ? true : false; 
            }
        }

        public async Task<UserTask?> GetByIdAsync(int id, Guid UserId)
        {
            const string sql =
                    """
                    SELECT * 
                    FROM "Tasks"
                    WHERE "UserId" = @UserId AND "TaskId" = @id
                    """;

            using (var connection = GetConnection()) 
            {

                return await connection.QueryFirstOrDefaultAsync<UserTask?>(sql, new 
                    {
                        UserId = UserId,
                        id = id
                    });
            }
        }

        public async Task<UserTask?> GetByIdTrackedAsync(int id, Guid UserId)
        {
            const string sql =
                    """
                    SELECT * 
                    FROM "Tasks"
                    WHERE "UserId" = @UserId AND "TaskId" = @id
                    """;

            using (var connection = GetConnection())
            {

                return await connection.QueryFirstOrDefaultAsync<UserTask?>(sql, new
                {
                    UserId = UserId,
                    id = id
                });
            }
        }

        public async Task<IEnumerable<UserTask>> GetPagedAsync(GetPageQuery query, Guid UserId)
        {

            const string sql =
                """
                    SELECT  *
                    FROM "Tasks"
                    WHERE "UserId" = @UserId
                    ORDER BY "TaskId"
                    LIMIT @PageSize OFFSET @Offset
                """;

            using (var connection = GetConnection())
            {

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
