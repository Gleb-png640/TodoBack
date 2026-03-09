using System.ComponentModel.DataAnnotations;
using TodoBack.Models.Tasks;

namespace TodoBack.Dtos.Tasks
{
    public class CreateUserTaskDto
    {
        [Required] public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public TaskType TaskType { get; set; }

        public DateTime? DueTo { get; set; }
        public DateTime? From { get; set; }

    }
}
