using TodoBack.Dtos.Tasks;
using TodoBack.Models.Tasks;
using TodoBack.QueryParameters;

namespace TodoBack.Repositories {
    public interface ITaskRepository {

        IEnumerable<TaskCommon> GetPaged(GetPageQuery query);

        TaskCommon? GetById(int id);

        TaskCommon Add(TaskCommon task);

        bool Update(TaskCommon task);

        bool Delete(TaskCommon task);

        public TaskCommon ChangeExistingTask(TaskCommon task, UpdateTaskCommonDto taskDto);
    }
}
