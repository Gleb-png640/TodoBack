namespace TodoBack.Dtos.Friendships
{
    public class SendFriendshipRequestDto
    {
        public Guid SenderId { get; set; }
        public Guid RecieverId { get; set; }
    }
}
