using TodoBack.Dtos.Friendships;
using TodoBack.Models.Friendships;
using static TodoBack.Repositories.PostgresFriendshipRepository;

namespace TodoBack.Repositories.Interfaces
{
    public interface IFriendshipRepository
    {
        public Task<Friendship> SendRequestAsync(SendFriendshipRequest dto);

        public Task<Friendship?> FindFriendshipAsync(Guid senderId, Guid recieverId);
        public Task<Friendship?> FindFriendshipTrackedAsync(Guid senderId, Guid recieverId);

        public Task<ICollection<Friendship>> GetFriendshipsAsync(Guid userId, FriendshipsListType type);
        public Task<ICollection<Friendship>> GetFriendshipsTrackedAsync(Guid userId, FriendshipsListType type);
    }
}
