using Microsoft.EntityFrameworkCore;
using TodoBack.Data;
using TodoBack.Dtos.Tasks;
using TodoBack.Models.Tasks;
using TodoBack.QueryParameters;

namespace TodoBack.Repositories {
    public class PostgresTaskRepository : ITaskRepository {

        private readonly TodoDbContext _db;

        public PostgresTaskRepository(TodoDbContext db)
        {
            _db = db;
        }

        public UserTask Add(UserTask task) {

            _db.Tasks.Add(task);
            _db.SaveChanges();

            return task;
        }

        public bool Delete(UserTask task) {

            _db.Remove(task);
            _db.SaveChanges();

            return true;
        }

        public IEnumerable<UserTask> GetPaged(GetPageQuery query, Guid UserId) {

            IQueryable<UserTask> queryable = _db.Tasks
                .Where(t => t.UserId == UserId)
                .AsNoTracking();

            if (query.isDone is not null) {
                queryable = queryable.Where(t => t.IsDone == query.isDone);
            }

            const int pageOffset = 1;
            int _page = query.page;
            int _pageSize = query.pageSize;

            IEnumerable<UserTask> result = queryable
                .Skip( (_page - pageOffset) * _pageSize)
                .Take(_pageSize)
                .ToList();

            return result;
        }



        public UserTask? GetById(int id, Guid UserId) {
            var task = _db.Tasks
                .AsNoTracking()
                .FirstOrDefault(t => t.UserId == UserId && t.TaskId == id);

            return task;
        }

        public UserTask? GetByIdTracked(int id, Guid UserId)
        {
            var task = _db.Tasks
                .FirstOrDefault(t => t.UserId == UserId && t.TaskId == id);

            return task;
        }

        public UserTask ChangeExistingTask(UserTask task, UpdateUserTaskDto taskDto) {

            task.Name = taskDto.Name;
            task.Description = taskDto.Description;

            task.IsDone = taskDto.IsDone;

            task.From = taskDto.From;
            task.DueTo = taskDto.DueTo;

            Update(task);
            return task;
        }

        private bool Update(UserTask task)
        {

            _db.Update(task);
            _db.SaveChanges();

            return true;
        }
    }
}
