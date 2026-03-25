using System.ComponentModel.DataAnnotations;
using TodoBack.Models.Users;

namespace TodoBack.Models.Friendships
{
    public class Friendship
    {
        public Guid FriendshipId { get; set; } = Guid.NewGuid();

        [Required] public Guid SenderId { get; set; }
        [Required] public User Sender { get; set; }

        [Required] public Guid RecieverId { get; set; }
        [Required] public User Reciever { get; set; }

        public FriendshipStatus FriendshipStatus { get; set; } = 0;

    }

    public enum FriendshipStatus 
    {
        pending = 0,
        accepted = 1,
        denied = 2
    }
}
