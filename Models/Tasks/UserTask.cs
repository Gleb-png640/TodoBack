using TodoBack.Models.Users;

namespace TodoBack.Models.Tasks {

    public enum TaskType 
    {
        Basic = 0,
        Timed = 1
    }

    public class UserTask {

        public int TaskId { get; set; }
        public Guid UserId { get; set; }

        public string Name { get; set; } = "default name";
        public string Description { get; set; } = "default description";

        public bool IsDone { get; set; } = false;
        public TaskType TaskType { get; set; } 

        public DateTime? DueTo { get; set; }
        public DateTime? From { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public User User { get; set; }
    }
}
