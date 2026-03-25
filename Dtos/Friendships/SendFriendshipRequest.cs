namespace TodoBack.Dtos.Friendships
{
    public class SendFriendshipRequest
    {
        public Guid SenderId { get; set; }
        public Guid RecieverId { get; set; }
    }
}
