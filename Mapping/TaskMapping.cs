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

        public static UserTask DtoToEntity(this CreateUserTaskDto task, Guid userId) {

            return new UserTask
            {
                UserId = userId,

                Name = task.Name,
                Description = task.Description ?? string.Empty,

                IsDone = task.IsDone ?? false,

                TaskType = task.TaskType,

                DueTo = task.DueTo,
                From = task.From,
            };
        }
    }
}
