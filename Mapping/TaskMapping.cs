using TodoBack.Dtos.Tasks;
using TodoBack.Models.Tasks;

namespace TodoBack.Mapping {
    public static class TaskMapping {

        public static GetUserTaskDto EntityToDto(this UserTask task) {
            return new GetUserTaskDto
            {
                Name = task.Name,
                Description = task.Description,

                IsDone = task.IsDone,
                TaskId = task.TaskId,

                TaskType = task.TaskType,

                DueTo = task.DueTo,
                From = task.From,

                CreatedAt = task.CreatedAt
            };
        }

        public static TaskCommon DtoToEntity(this CreateTaskCommonDto task, Guid id) {

            return new UserTask
            { 
                Name = task.Name, 
                Description = task.Description,
                IsDone = false,
                UserId = id
            };
        }
    }
}
