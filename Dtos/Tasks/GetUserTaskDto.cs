using System.ComponentModel.DataAnnotations;
using TodoBack.Models.Tasks;

namespace TodoBack.Dtos.Tasks
{

    public record class GetUserTaskDto
    {
        [Required] public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public bool IsDone { get; set; } 
        public int TaskId { get; set; }

        public TaskType TaskType { get; set; }

        public DateTime? DueTo { get; set; }
        public DateTime? From { get; set; }

        public DateTime CreatedAt { get; set; }

        // sent to user
    }
}
