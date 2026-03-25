using FluentValidation;
using Microsoft.AspNetCore.Identity;
using TodoBack.Dtos.Users;
using TodoBack.Mapping;
using TodoBack.Models.Users;
using TodoBack.Repositories;
using TodoBack.Services.Security;

namespace TodoBack.Endpoints {

    public static class UsersEndpoints {

        public static void MapUsersEndpoints(this WebApplication app) {

            var group = app.MapGroup("users");


            // POST /users/register
            group.MapPost("/register", async (CreateUserDto dto, IUserRepository repo, IPasswordHasher<User> passwordHasher, IValidator<CreateUserDto> validator, JwtTokenServices jwt) =>
            {

                // Validation
                var result = await validator.ValidateAsync(dto);
                if (!result.IsValid) { return Results.ValidationProblem(result.ToDictionary()); }

                // Searching by email in DB
                if (await repo.GetByEmailAsync(dto.Email) is not null) { return Results.Conflict("User already exists"); }

                // Checking if UserName is already taken
                if (await repo.GetByUserNameAsync(dto.UserName) is not null) { return Results.Conflict("User name is already taken"); }

                var user = dto.CreateDtoToEntity(passwordHasher);

                await repo.AddUserAsync(user);

                return Results.Created($"/users/{user.Id}", user.EntityToDto());
            });


            // POST /users/login 
            group.MapPost("/login", async (LoginUserDto dto, IValidator<LoginUserDto> validator, IUserRepository repo, IPasswordHasher<User> passwordHasher, JwtTokenServices jwt) => 
            {
                var result = await validator.ValidateAsync(dto);
                if (!result.IsValid) { return Results.ValidationProblem(result.ToDictionary()); }

                var response = await repo.LoginAsync(dto, passwordHasher, jwt);

                if (response is null) { return Results.Unauthorized(); } 

                return Results.Ok(response);
            });

        }
    }
}
