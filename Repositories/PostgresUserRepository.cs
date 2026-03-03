using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
                .AsNoTracking()
                .FirstOrDefault(u => u.Email == email);
        }

        public string? Login(LoginUserDto dto, IPasswordHasher<User> passwordHasher, JwtTokenServices jwt) {

            // Searching by email in DB
            var user = GetByEmail(dto.Email);
            if (user is null) { return null; }

            // Verifying password
            var passwordVerificationResult = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (passwordVerificationResult == PasswordVerificationResult.Failed) { return null; }

            string token = jwt.CreateJWT(user);

            return token;
        }
    }
}
