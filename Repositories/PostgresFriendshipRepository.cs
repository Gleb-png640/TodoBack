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

        public async Task<Friendship?> FindFriendshipTrackedAsync(Guid senderId, Guid recieverId)
        {
            return await _db.Friendship
                .Where(f => f.SenderId == senderId && f.RecieverId == recieverId)
                .FirstOrDefaultAsync();
        }

        public async Task<Friendship?> FindFriendshipAsync(Guid senderId, Guid recieverId)
        {
            return await _db.Friendship
                .Where(f => f.SenderId == senderId && f.RecieverId == recieverId)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<Friendship> SendRequestAsync(SendFriendshipRequest dto) 
        {

            var friendship = dto.RequestToEntity();

            await _db.Friendship.AddAsync(friendship);
            await _db.SaveChangesAsync();

            return friendship;
        }


        public async Task<ICollection<Friendship>> GetFriendshipsAsync(Guid userId, FriendshipsListType type)
        {
            IQueryable<Friendship> query = _db.Friendship.AsNoTracking();

            switch (type)
            {
                case FriendshipsListType.Sent:
                    query = query.Where(f => f.SenderId == userId);
                    break;

                case FriendshipsListType.Received:
                    query = query.Where(f => f.RecieverId == userId);
                    break;

                default:
                    throw new Exception("incorrect FriendshipsListType");
            }

            var list = await query.ToListAsync();

            return list;
        }

        public async Task<ICollection<Friendship>> GetFriendshipsTrackedAsync(Guid userId, FriendshipsListType type)
        {
            IQueryable<Friendship> query = _db.Friendship;

            switch (type) 
            {
                case FriendshipsListType.Sent:
                    query = query.Where(f => f.SenderId == userId);
                    break;

                case FriendshipsListType.Received:
                    query = query.Where(f => f.RecieverId == userId);
                    break;

                default:
                    throw new Exception("incorrect FriendshipsListType");
            }

            var list = await query.ToListAsync();

            return list;
        }

        public enum FriendshipsListType 
        {
            Sent = 0,
            Received = 1
        }
    }
}
