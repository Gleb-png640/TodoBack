namespace TodoBack.Dtos.Users {
    public class UserDto {

        public string UserName { get; set; } = string.Empty;

        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;

        public int TasksCount { get; set; }
    }
}
