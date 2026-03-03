using System.ComponentModel.DataAnnotations;

namespace TodoBack.Dtos.Tasks {
    public class UpdateTaskCommonDto {

        [Required] public string Name { get; set; } = string.Empty;
        [Required] public string Description { get; set; } = string.Empty;
        public bool IsDone { get; set; }
        //public DateTime DateCreated { get; set; }
        //public DateTime DateDueTo { get; set; }
    }
}
