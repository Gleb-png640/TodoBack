namespace TodoBack.Models.Tasks {
    public interface ITask {
        string Name { get; set; }
        string Description { get; set; }
        bool IsDone { get; set; }

        int TaskId { get; }

        //public DateTime DateCreated { get; set; }
        //public DateTime DateDueTo { get; set; }
    }
}
