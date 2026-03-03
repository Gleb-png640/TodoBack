using System.ComponentModel.DataAnnotations;

namespace TodoBack.Dtos.Tasks {
    public class CreateTaskCommonDto {

        [Required] public string Name { get; set; } = string.Empty;
        [Required] public string Description { get; set; } = string.Empty;

        //public DateTime DateCreated { get; set; }
        //public DateTime DateDueTo { get; set; }

    }
}
