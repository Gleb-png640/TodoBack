
namespace TodoBack.Dtos.Tasks {
    public class UpdateUserTaskDto
    {

        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public bool IsDone { get; set; } = false;

        public DateTime? From { get; set; }
        public DateTime? DueTo { get; set; }
    }
}
