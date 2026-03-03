using TodoBack.Models.Users;

namespace TodoBack.Models.Tasks {
    public class TaskCommon {
        public string Name { get; set; } = "default name";
        public string Description { get; set; } = "default description";
        public bool IsDone { get; set; } = false;
        public int TaskId { get; set; }

        public User User { get; set; }
        public Guid UserId { get; set; }

        //public DateTime DateCreated { get; set; }
        //public DateTime DateDueTo { get; set; }
    }
}
