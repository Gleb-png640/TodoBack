using Microsoft.EntityFrameworkCore;
using TodoBack.Data;
using TodoBack.Dtos.Tasks;
using TodoBack.Models.Tasks;
using TodoBack.QueryParameters;

namespace TodoBack.Repositories {
    public class PostgresRepository : ITaskRepository {

        private readonly TodoDbContext _db;

        public PostgresRepository(TodoDbContext db)
        {
            _db = db;
        }

        public TaskCommon Add(TaskCommon task) {

            _db.Add(task);
            _db.SaveChanges();

            return task;
        }

        public bool Delete(TaskCommon task) {

            _db.Remove(task);
            _db.SaveChanges();

            return true;
        }

        public IEnumerable<TaskCommon> GetPaged(GetPageQuery query, Guid UserId) {

            IQueryable<TaskCommon> queryable = _db.Tasks
                .Where(t => t.UserId == UserId)
                .AsNoTracking();

            if (query.isDone is not null) {
                queryable = queryable.Where(t => t.IsDone == query.isDone);
            }

            const int pageOffset = 1;
            int _page = query.page;
            int _pageSize = query.pageSize;

            IEnumerable<TaskCommon> result = queryable
                .Skip( (_page - pageOffset) * _pageSize)
                .Take(_pageSize)
                .ToList();

            return result;
        }



        public TaskCommon? GetById(int id, Guid UserId) {
            var task = _db.Tasks
                .AsNoTracking()
                .FirstOrDefault(t => t.UserId == UserId && t.TaskId == id);

            return task;
        }

        public TaskCommon ChangeExistingTask(TaskCommon task, UpdateTaskCommonDto taskDto) {

            task.Name = taskDto.Name;
            task.Description = taskDto.Description;
            task.IsDone = taskDto.IsDone;
            //task.DateCreated = taskDto.DateCreated;
            //task.DateDueTo = taskDto.DateDueTo;

            Update(task);
            return task;
        }

        private bool Update(TaskCommon task)
        {

            _db.Update(task);
            _db.SaveChanges();

            return true;
        }
    }
}
