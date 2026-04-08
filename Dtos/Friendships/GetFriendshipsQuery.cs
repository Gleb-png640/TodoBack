using System.ComponentModel.DataAnnotations;

namespace TodoBack.Dtos.Friendships
{
    public class GetFriendshipsQuery
    {
        [Required] public int FriendshipListType { get; set; }
    }
}
