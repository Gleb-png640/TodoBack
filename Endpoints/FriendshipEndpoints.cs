using System.Security.Claims;
using TodoBack.Dtos.Friendships;
using TodoBack.Mapping;
using TodoBack.Repositories.Interfaces;

namespace TodoBack.Endpoints
{
    public static class FriendshipEndpoints
    {
        public static void MapFriendshipEndpoints(this WebApplication app) 
        {
            var group = app.MapGroup("/friendship").RequireAuthorization();


            group.MapGet("", async ([AsParameters] GetFriendshipsQuery query, IFriendshipRepository repo, ClaimsPrincipal user) => 
            {
                var userId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var res = await repo.GetFriendshipsAsync(userId, query.FriendshipListType);
                return Results.Ok(res);
            });

            group.MapPost("/add", async ([AsParameters] SendFriendshipRequestDto dto, IFriendshipRepository repo, ClaimsPrincipal user) =>
            {
                var userId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);

                var request = dto.DtoToRequest(userId);

                var friendship = await repo.FindFriendshipAsync(request.SenderId, request.RecieverId, false);

                if (friendship is not null) { return Results.Conflict("Request already exists"); }

                friendship = await repo.SendRequestAsync(request);

                return Results.Ok(friendship);
            });
        }

    }
}
