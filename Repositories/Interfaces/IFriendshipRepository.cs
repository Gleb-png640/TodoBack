using TodoBack.Dtos.Friendships;
using TodoBack.Models.Friendships;
using static TodoBack.Repositories.PostgresFriendshipRepository;

namespace TodoBack.Repositories.Interfaces
{
    public interface IFriendshipRepository
    {
        public Task<Friendship> SendRequestAsync(FriendshipRequest dto);

        public Task<Friendship?> FindFriendshipAsync(Guid senderId, Guid recieverId, bool tracked = true);

        public Task<ICollection<Friendship>> GetFriendshipsAsync(Guid userId, int friendshipRequestType, bool tracked = true);

        public Task<Friendship> RequestRespond(Friendship friendship, bool accepted);
    }
}
