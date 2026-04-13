namespace TodoBack.Dtos.Friendships
{
    public class FriendshipResponseDto
    {
        public Guid SenderId { get; set; }
        public bool Accepted { get; set; }
    }
}
