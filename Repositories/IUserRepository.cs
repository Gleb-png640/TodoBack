using Microsoft.AspNetCore.Identity;
using TodoBack.Dtos.Users;
using TodoBack.Models.Users;
using TodoBack.Services.Security;

namespace TodoBack.Repositories {

    public interface IUserRepository 
    {
        public Task<User> AddUserAsync(User user);

        public Task<User?> GetByEmailAsync(string email);
        public Task<User?> GetByUserNameAsync(string userName);

        public Task<TokenResponeDto?> LoginAsync(LoginUserDto dto, IPasswordHasher<User> passwordHasher, JwtTokenServices jwt);

        public Task<TokenResponeDto?> RefreshTokensAsync(RefreshTokenRequestDto dto, JwtTokenServices jwt);
    }
}