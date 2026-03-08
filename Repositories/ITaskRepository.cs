using TodoBack.Dtos.Tasks;
using TodoBack.Models.Tasks;
using TodoBack.QueryParameters;

namespace TodoBack.Repositories {
    public interface ITaskRepository {

        public IEnumerable<TaskCommon> GetPaged(GetPageQuery query, Guid UserId);

        public TaskCommon? GetById(int id, Guid UserId);

        public TaskCommon Add(TaskCommon task);

        public bool Delete(TaskCommon task);

        public TaskCommon ChangeExistingTask(TaskCommon task, UpdateTaskCommonDto taskDto);
    }
}
