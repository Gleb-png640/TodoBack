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


        public async Task<Friendship?> FindFriendshipAsync(Guid senderId, Guid recieverId, bool tracked = true)
        {
            IQueryable<Friendship> query = _db.Friendship;

            if (!tracked) { query = query.AsNoTracking(); }

            return await query
                .Where(f => f.SenderId == senderId && f.RecieverId == recieverId)
                .FirstOrDefaultAsync();
        }


        public async Task<Friendship> SendRequestAsync(FriendshipRequest dto) 
        {

            var friendship = dto.RequestToEntity();

            await _db.Friendship.AddAsync(friendship);
            await _db.SaveChangesAsync();

            return friendship;
        }


        public async Task<ICollection<Friendship>> GetFriendshipsAsync(Guid userId, int friendshipRequestType, bool tracked = true)
        {
            var type = (FriendshipsListType)friendshipRequestType;
            IQueryable<Friendship> query = _db.Friendship;

            if (!tracked) { query = query.AsNoTracking(); }

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

        public async Task<Friendship> RequestRespond(Friendship friendship, bool accepted) 
        {
            friendship.FriendshipStatus = accepted ? FriendshipStatus.accepted: FriendshipStatus.denied;
            await _db.SaveChangesAsync();

            return friendship;
        }

        public enum FriendshipsListType 
        {
            Sent = 0,
            Received = 1
        }
    }
}
