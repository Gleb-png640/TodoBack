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


            group.MapPost("/add", async (SendFriendshipRequestDto dto, IFriendshipRepository repo, ClaimsPrincipal user) =>
            {
                var request = dto.DtoToRequest(Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value));

                var friendship = await repo.FindFriendshipAsync(request.SenderId, request.RecieverId);

                if (friendship is null) { return Results.Conflict("Request already exists"); }

                friendship = await repo.SendRequestAsync(request);

                return Results.Ok(friendship);
            });
        }

    }
}
