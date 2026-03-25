using TodoBack.Dtos.Tasks;
using TodoBack.Models.Tasks;
using TodoBack.QueryParameters;

namespace TodoBack.Repositories.Interfaces
{
    public interface ITaskRepository
    {
        public Task<UserTask> AddAsync(UserTask task);

        public Task<IEnumerable<UserTask>> GetPagedAsync(GetPageQuery query, Guid UserId);

        public Task<UserTask?> GetByIdAsync(int id, Guid UserId);
        public Task<UserTask?> GetByIdTrackedAsync(int id, Guid UserId);


		public Task<bool> DeleteAsync(UserTask task);

        public Task<UserTask> ChangeExistingTaskAsync(UserTask task, UpdateUserTaskDto taskDto);
    }
}