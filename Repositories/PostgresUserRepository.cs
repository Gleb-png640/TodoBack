using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using TodoBack.Data;
using TodoBack.Dtos.Users;
using TodoBack.Models.Users;
using TodoBack.Services.Security;

namespace TodoBack.Repositories {

    public class PostgresUserRepository : IUserRepository {

        private readonly TodoDbContext _db;

        public PostgresUserRepository(TodoDbContext db) {
            _db = db;
        }

        public async Task<User> AddUserAsync(User user) {
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
            return user;
        }

        public async Task<User?> GetByEmailAsync(string email) {
            return await _db.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<TokenResponeDto?> LoginAsync(LoginUserDto dto, IPasswordHasher<User> passwordHasher, JwtTokenServices jwt) {

            // Searching by email in DB
            var user = await GetByEmailAsync(dto.Email);
            if (user is null) { return null; }

            // Verifying password
            var passwordVerificationResult = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (passwordVerificationResult == PasswordVerificationResult.Failed) { return null; }

            TokenResponeDto response = await CreateTokenResponseAsync(jwt, user);

            return response;
        }


        public async Task<TokenResponeDto?> RefreshTokensAsync(RefreshTokenRequestDto dto, JwtTokenServices jwt)
        {
            var user = await ValidateRefreshTokenAsync(dto.RefreshToken);

            if (user is null) { return null; }

            TokenResponeDto response = await CreateTokenResponseAsync(jwt, user);

            return response;
        }

        private async Task<User?> ValidateRefreshTokenAsync(string refreshToken) 
        {
            var token = HashRefreshToken(refreshToken);

            var user = await _db.Users.FirstOrDefaultAsync(u => u.RefreshToken == token);

            if (user is null || user.RefreshTokenExpiryTime <= DateTime.UtcNow) { return null; }

            return (user);
        }


        private async Task<TokenResponeDto> CreateTokenResponseAsync(JwtTokenServices jwt, User user)
        {
            TokenResponeDto response = jwt.CreateJWT(user);

            user.RefreshToken = HashRefreshToken(response.RefreshToken);
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(10);

            await _db.SaveChangesAsync();

            return response;
        }

        private string HashRefreshToken(string token) 
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(token));
            return Convert.ToBase64String(bytes);
        }
    }
}
