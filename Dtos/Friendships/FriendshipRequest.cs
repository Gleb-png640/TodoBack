namespace TodoBack.Dtos.Friendships
{
    public class FriendshipRequest
    {
        public Guid SenderId { get; set; }
        public Guid RecieverId { get; set; }
    }
}
