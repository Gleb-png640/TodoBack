using Microsoft.AspNetCore.Identity;
using TodoBack.Dtos.Users;
using TodoBack.Models.Users;

namespace TodoBack.Mapping {

    public static class UserMapping {

        public static UserDto EntityToDto(this User user) {

            return new UserDto {
                UserName = user.UserName,
                Email = user.Email,
                Id = user.Id,
                TasksCount = user.Tasks.Count
            };
        }

        public static User CreateDtoToEntity(this CreateUserDto dto, IPasswordHasher<User> passwordHasher)
        {
            var user = new User {
                UserName = dto.UserName,
                Email = dto.Email
            };

            user.PasswordHash = passwordHasher.HashPassword(user, dto.Password);

            return user;
        }
    }
}
