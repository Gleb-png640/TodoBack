using TodoBack.Models.Tasks;

namespace TodoBack.Models.Users {

    public class User {

        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public Guid Id { get; set; }

        public string PasswordHash { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<UserTask> Tasks { get; set; } = new List<UserTask>();

        public string?  RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
