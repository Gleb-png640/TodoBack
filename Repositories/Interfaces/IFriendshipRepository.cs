using TodoBack.Dtos.Friendships;
using TodoBack.Models.Friendships;

namespace TodoBack.Repositories.Interfaces
{
    public interface IFriendshipRepository
    {
        public Task<Friendship> SendRequestAsync(SendFriendshipRequestDto dto);

        public Task<Friendship?> FindFriendshipAsync(Guid senderId, Guid recieverId);
    }
}
