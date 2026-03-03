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

        public IEnumerable<TaskCommon> GetPaged(GetPageQuery query) {

            IQueryable<TaskCommon> queryable = _db.Tasks.AsNoTracking();

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



        public TaskCommon? GetById(int id) {
            var task = _db.Tasks.Find(id);

            return task;
        }

        public bool Update(TaskCommon task) {
            /*
             
             Переписать с ExecuteUpdate
             
             */


            _db.Update(task);
            _db.SaveChanges();

            return true;
        }

        public TaskCommon ChangeExistingTask(TaskCommon task, UpdateTaskCommonDto taskDto) {


            /*
             
             Переписать с ExecuteUpdate
             
             */

            task.Name = taskDto.Name;
            task.Description = taskDto.Description;
            task.IsDone = taskDto.IsDone;
            //task.DateCreated = taskDto.DateCreated;
            //task.DateDueTo = taskDto.DateDueTo;

            return task;
        }
    }
}
