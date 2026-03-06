using Microsoft.AspNetCore.Identity;
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

        public User AddUser(User user) {
            _db.Users.Add(user);
            _db.SaveChanges();
            return user;
        }

        public User? GetByEmail(string email) {
            return _db.Users
                .FirstOrDefault(u => u.Email == email);
        }


        private User? FindById(Guid userId)
        {
            return _db.Users
                .Where(u => u.Id == userId)
                .FirstOrDefault();
        }

        public TokenResponeDto? Login(LoginUserDto dto, IPasswordHasher<User> passwordHasher, JwtTokenServices jwt) {

            // Searching by email in DB
            var user = GetByEmail(dto.Email);
            if (user is null) { return null; }

            // Verifying password
            var passwordVerificationResult = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (passwordVerificationResult == PasswordVerificationResult.Failed) { return null; }

            TokenResponeDto response = CreateTokenResponse(jwt, user);

            _db.SaveChanges();

            return response;
        }


        public TokenResponeDto? RefreshTokens(RefreshTokenRequestDto dto, JwtTokenServices jwt)
        {
            var user = ValidateRefreshToken(dto.UserId, dto.RefreshToken);

            if (user is null) { return null; }

            TokenResponeDto response = CreateTokenResponse(jwt, user);

            return response;
        }

        private User? ValidateRefreshToken(Guid userId, string token) 
        {
            var user = FindById(userId);

            if (user is null || user.RefreshToken != token || user.RefreshTokenExpiryTime <= DateTime.UtcNow) { return null; }

            return (user);
        }


        private TokenResponeDto CreateTokenResponse(JwtTokenServices jwt, User user)
        {
            TokenResponeDto response = jwt.CreateJWT(user);
            user.RefreshToken = response.RefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(10);

            _db.SaveChanges();
            return response;
        }
    }
}
