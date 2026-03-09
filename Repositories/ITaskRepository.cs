using TodoBack.Dtos.Tasks;
using TodoBack.Models.Tasks;
using TodoBack.QueryParameters;

namespace TodoBack.Repositories
{
    public interface ITaskRepository
    {
        public UserTask Add(UserTask task);

        public IEnumerable<UserTask> GetPaged(GetPageQuery query, Guid UserId);

        public UserTask? GetById(int id, Guid UserId);
        public UserTask? GetByIdTracked(int id, Guid UserId);


		public bool Delete(UserTask task);

        public UserTask ChangeExistingTask(UserTask task, UpdateUserTaskDto taskDto);
    }
}