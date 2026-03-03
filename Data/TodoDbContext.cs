using Microsoft.EntityFrameworkCore;
using TodoBack.Models.Tasks;
using TodoBack.Models.Users;


namespace TodoBack.Data {

    public class TodoDbContext(DbContextOptions<TodoDbContext> options) 
        : DbContext(options)
    {

        public DbSet<TaskCommon> Tasks => Set<TaskCommon>();
        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TaskCommon>()
                .HasKey(k => k.TaskId);

            modelBuilder.Entity<TaskCommon>()
                .Property(k => k.TaskId)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<TaskCommon>()
                .HasOne(t => t.User)
                .WithMany(u => u.Tasks)
                .HasForeignKey(t => t.UserId);
        }
    }
}
