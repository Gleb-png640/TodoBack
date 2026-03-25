using Microsoft.EntityFrameworkCore;
using TodoBack.Data;
using TodoBack.Dtos.Tasks;
using TodoBack.Models.Tasks;
using TodoBack.QueryParameters;
using TodoBack.Repositories.Interfaces;

namespace TodoBack.Repositories {
    public class PostgresTaskRepository : ITaskRepository {

        private readonly TodoDbContext _db;

        public PostgresTaskRepository(TodoDbContext db)
        {
            _db = db;
        }

        public async Task<UserTask> AddAsync(UserTask task) {

            await _db.Tasks.AddAsync(task);
            await _db.SaveChangesAsync();

            return task;
        }

        public async Task<bool> DeleteAsync(UserTask task) {

            _db.Tasks.Remove(task);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<UserTask>> GetPagedAsync(GetPageQuery query, Guid UserId) {

            IQueryable<UserTask> queryable = _db.Tasks
                .Where(t => t.UserId == UserId)
                .AsNoTracking();

            if (query.isDone is not null) {
                queryable = queryable.Where(t => t.IsDone == query.isDone);
            }

            const int pageOffset = 1;
            int _page = query.page;
            int _pageSize = query.pageSize;

            IEnumerable<UserTask> result = await queryable
                .Skip( (_page - pageOffset) * _pageSize)
                .Take(_pageSize)
                .ToListAsync();

            return result;
        }

        public async Task<UserTask?> GetByIdAsync(int id, Guid UserId) {
            var task = await _db.Tasks
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.UserId == UserId && t.TaskId == id);

            return task;
        }

        public async Task<UserTask?> GetByIdTrackedAsync(int id, Guid UserId)
        {
            var task = await _db.Tasks
                .FirstOrDefaultAsync(t => t.UserId == UserId && t.TaskId == id);

            return task;
        }

        public async Task<UserTask> ChangeExistingTaskAsync(UserTask task, UpdateUserTaskDto taskDto) {

            task.Name = taskDto.Name;
            task.Description = taskDto.Description;

            task.IsDone = taskDto.IsDone;

            task.From = taskDto.From;
            task.DueTo = taskDto.DueTo;

            await _db.SaveChangesAsync();
            return task;
        }

    }
}
