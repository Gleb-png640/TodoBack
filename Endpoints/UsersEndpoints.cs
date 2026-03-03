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
            group.MapPost("/register", (CreateUserDto dto, IUserRepository repo, IPasswordHasher<User> passwordHasher, IValidator<CreateUserDto> validator, JwtTokenServices jwt) =>
            {

                // Validation
                var result = validator.Validate(dto);
                if (!result.IsValid) { return Results.ValidationProblem(result.ToDictionary()); }

                // Searching by email in DB
                if (repo.GetByEmail(dto.Email) is not null) { return Results.Conflict("User already exists"); }

                var user = dto.CreateDtoToEntity(passwordHasher);

                repo.AddUser(user);

                return Results.Created($"/users/{user.Id}", user.EntityToDto());
            });


            // POST /users/login 
            group.MapPost("/login", (LoginUserDto dto, IValidator<LoginUserDto> validator, IUserRepository repo, IPasswordHasher<User> passwordHasher, JwtTokenServices jwt) => 
            {
                var result = validator.Validate(dto);
                if (!result.IsValid) { return Results.ValidationProblem(result.ToDictionary()); }

                string? token = repo.Login(dto, passwordHasher, jwt);

                if (token is null) { return Results.BadRequest("Incorrect email or password"); } 

                return Results.Ok(token);
            });
            
            // ye
            //group.MapGet("auth", () =>
            //{ 
            //    return Results.Ok("you are authorized");
            //}).RequireAuthorization();
        }
    }
}
