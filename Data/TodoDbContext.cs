using Microsoft.EntityFrameworkCore;
using TodoBack.Models.Tasks;
using TodoBack.Models.Users;


namespace TodoBack.Data {

    public class TodoDbContext(DbContextOptions<TodoDbContext> options) 
        : DbContext(options)
    {

        public DbSet<UserTask> Tasks => Set<UserTask>();
        public DbSet<User> Users => Set<User>();

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
        }
    }
}
