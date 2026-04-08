using System.Security.Claims;
using TodoBack.Dtos.Friendships;
using TodoBack.Mapping;
using TodoBack.Repositories.Interfaces;
using static TodoBack.Repositories.PostgresFriendshipRepository;

namespace TodoBack.Endpoints
{
    public static class FriendshipEndpoints
    {
        public static void MapFriendshipEndpoints(this WebApplication app) 
        {
            var group = app.MapGroup("/friendship").RequireAuthorization();


            group.MapGet("", async (GetFriendshipsQuery query, IFriendshipRepository repo, ClaimsPrincipal user) => 
            {
                var userId = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var res = await repo.GetFriendshipsAsync(userId, (FriendshipsListType)query.FriendshipListType);
                return Results.Ok(res);
            });

            group.MapPost("/add", async (SendFriendshipRequestDto dto, IFriendshipRepository repo, ClaimsPrincipal user) =>
            {
                var request = dto.DtoToRequest(Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value));

                var friendship = await repo.FindFriendshipTrackedAsync(request.SenderId, request.RecieverId);

                if (friendship is not null) { return Results.Conflict("Request already exists"); }

                friendship = await repo.SendRequestAsync(request);

                return Results.Ok(friendship);
            });
        }

    }
}
