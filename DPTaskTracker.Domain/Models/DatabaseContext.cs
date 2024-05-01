using Microsoft.EntityFrameworkCore;

namespace DPTaskTracker.Domain.Models;

public class DatabaseContext : DbContext
{
    public DbSet<Project> Projects { get; set; }

    public DbSet<TaskProject> TaskProjects { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<Message> Messages { get; set; }

    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
        //Database.EnsureDeleted();
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasData(User.GetTestInstance("Alex", "alexgoph@gmail.com", "123", UserRole.Admin, 1));
        });
        
        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasData(Project.GetTestInstance("Сделать курсовой проект", "Таск-трекер, для отслеживания задач", DateTime.Now.AddDays(1), 1));
            entity.HasData(Project.GetTestInstance("Убраться дома", "Пропылесосить, вынести мусор", DateTime.Now.AddDays(5), 2));
        });

        modelBuilder.Entity<Message>()
            .HasOne(m => m.User)
            .WithMany(u => u.Messages)
            .HasForeignKey(m => m.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}