using TodoBack.Dtos.Friendships;
using TodoBack.Models.Friendships;

namespace TodoBack.Mapping
{
    public static class FriendshipMapping
    {
        public static Friendship DtoToEntity(this SendFriendshipRequestDto dto) 
        {
            return new Friendship
            {
                SenderId = dto.SenderId,
                RecieverId = dto.RecieverId
            };
        }
    }
}
