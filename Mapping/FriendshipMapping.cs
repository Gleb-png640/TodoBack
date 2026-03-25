using TodoBack.Dtos.Friendships;
using TodoBack.Models.Friendships;

namespace TodoBack.Mapping
{
    public static class FriendshipMapping
    {
        public static Friendship RequestToEntity(this SendFriendshipRequest dto) 
        {
            return new Friendship
            {
                SenderId = dto.SenderId,
                RecieverId = dto.RecieverId
            };
        }

        public static SendFriendshipRequest DtoToRequest(this SendFriendshipRequestDto dto, Guid senderId)
        {
            return new SendFriendshipRequest
            {
                SenderId = senderId,
                RecieverId = dto.RecieverId
            };
        }
    }
}
