using TodoBack.Dtos.Tasks;
using TodoBack.Models.Tasks;

namespace TodoBack.Mapping {
    public static class TaskMapping {

        public static GetTaskCommonDto EntityToDto(this TaskCommon task) {
            return new GetTaskCommonDto {
                Name = task.Name,
                Description = task.Description,
                IsDone = task.IsDone,
                TaskId = task.TaskId,
                //DateCreated = task.DateCreated,
                //DateDueTo = task.DateDueTo
            };
        }

        public static TaskCommon DtoToEntity(this CreateTaskCommonDto task, Guid id) {

            return new TaskCommon
            { 
                Name = task.Name, 
                Description = task.Description,
                IsDone = false,
                UserId = id
            };
        }
    }
}
