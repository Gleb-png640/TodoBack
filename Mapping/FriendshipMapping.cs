using TodoBack.Dtos.Friendships;
using TodoBack.Models.Friendships;

namespace TodoBack.Mapping
{
    public static class FriendshipMapping
    {
        public static Friendship RequestToEntity(this FriendshipRequest dto) 
        {
            return new Friendship
            {
                SenderId = dto.SenderId,
                RecieverId = dto.RecieverId
            };
        }

        public static FriendshipRequest DtoToRequest(this SendFriendshipRequestDto dto, Guid senderId)
        {
            return new FriendshipRequest
            {
                SenderId = senderId,
                RecieverId = dto.RecieverId
            };
        }
    }
}
