using Microsoft.AspNetCore.Identity;
using TodoBack.Dtos.Users;
using TodoBack.Models.Users;
using TodoBack.Services.Security;

namespace TodoBack.Repositories {
    public interface IUserRepository {
        User AddUser(User user);

        User? GetByEmail(string email);

        public string? Login(LoginUserDto dto, IPasswordHasher<User> passwordHasher, JwtTokenServices jwt);
    }
}