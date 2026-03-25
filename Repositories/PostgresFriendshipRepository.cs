using Microsoft.EntityFrameworkCore;
using TodoBack.Data;
using TodoBack.Dtos.Friendships;
using TodoBack.Mapping;
using TodoBack.Models.Friendships;
using TodoBack.Repositories.Interfaces;

namespace TodoBack.Repositories
{
    public class PostgresFriendshipRepository: IFriendshipRepository
    {
        private readonly TodoDbContext _db;

        public PostgresFriendshipRepository(TodoDbContext db) => _db = db;

        public async Task<Friendship?> FindFriendshipAsync(Guid senderId, Guid recieverId)
        {
            return await _db.Friendship
                .Where(f => f.SenderId == senderId && f.RecieverId == recieverId)
                .FirstOrDefaultAsync();
        }

        public async Task<Friendship> SendRequestAsync(SendFriendshipRequest dto) 
        {

            var friendship = dto.RequestToEntity();

            await _db.Friendship.AddAsync(friendship);

            return friendship;
        }

    }
}
