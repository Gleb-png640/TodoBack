using TodoBack.Dtos.Users;
using TodoBack.Repositories.Interfaces;
using TodoBack.Services.Security;

namespace TodoBack.Endpoints {

    public static class AuthEndpoints {

        public static void MapAuthEndpoints(this WebApplication app) {

            var group = app.MapGroup("auth");


            group.MapPost("/refresh", async ([AsParameters] RefreshTokenRequestDto dto, IUserRepository repo, JwtTokenServices jwt) =>
            {
                var result = await repo.RefreshTokensAsync(dto, jwt);

                if (result is null) { return Results.Unauthorized(); }

                return Results.Ok(result);
            });
        }
    }
}
