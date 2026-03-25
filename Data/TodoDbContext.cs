using Microsoft.EntityFrameworkCore;
using TodoBack.Models.Tasks;
using TodoBack.Models.Users;


namespace TodoBack.Data {

    public class TodoDbContext(DbContextOptions<TodoDbContext> options) 
        : DbContext(options)
    {

        public DbSet<UserTask> Tasks => Set<UserTask>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Friendship> Friendship => Set<Friendship>();

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserTask>()
                .HasKey(k => k.TaskId);

            modelBuilder.Entity<UserTask>()
                .Property(k => k.TaskId)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<UserTask>()
                .HasOne(t => t.User)
                .WithMany(u => u.Tasks)
                .HasForeignKey(t => t.UserId);


            modelBuilder.Entity<Friendship>()
                .HasKey(f => f.FriendshipId);

            modelBuilder.Entity<Friendship>()
                .Property(f => f.FriendshipId)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Friendship>()
                .HasOne(f => f.Sender)
                .WithMany(u => u.SentFriendships)
                .HasForeignKey(f => f.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Friendship>()
                .HasOne(f => f.Reciever)
                .WithMany(u => u.ReceivedFriendships)
                .HasForeignKey(f => f.RecieverId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
