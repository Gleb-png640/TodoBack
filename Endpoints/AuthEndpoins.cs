using TodoBack.Dtos.Users;
using TodoBack.Repositories;
using TodoBack.Services.Security;

namespace TodoBack.Endpoints {

    public static class AuthEndpoints {

        public static void MapAuthEndpoints(this WebApplication app) {

            var group = app.MapGroup("auth");


            group.MapPost("/refresh", (RefreshTokenRequestDto dto, IUserRepository repo, JwtTokenServices jwt) =>
            {
                var result = repo.RefreshTokens(dto, jwt);

                if (result is null) { return Results.Unauthorized(); }

                return Results.Ok(result);
            });
        }
    }
}
