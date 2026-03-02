using Microsoft.EntityFrameworkCore;
using TodoList.Models;

namespace TodoList.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<TaskItem> Tasks { get; set; }
    public DbSet<Tag> Tags { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    { }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=TodoList;Username=postgres;Password=password");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
        modelBuilder.Entity<User>().Property(u => u.PasswordHash).HasMaxLength(200);
        modelBuilder.Entity<User>().Property(u => u.Name).HasMaxLength(50);
        modelBuilder.Entity<User>().Property(u => u.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

        modelBuilder.Entity<Project>().HasIndex(p => p.Name).IsUnique();
        modelBuilder.Entity<Project>().Property(p => p.Name).HasMaxLength(100);
        modelBuilder.Entity<Project>().Property(p => p.Description).HasMaxLength(500);
        modelBuilder.Entity<Project>().Property(p => p.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        modelBuilder.Entity<Project>().HasOne(p => p.User).WithMany(u => u.Projects).HasForeignKey(p => p.UserId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Project>().HasIndex(p => p.UserId);

        modelBuilder.Entity<TaskItem>().HasIndex(t => t.ProjectId);
        modelBuilder.Entity<TaskItem>().HasIndex(t => t.CreatedAt);
        modelBuilder.Entity<TaskItem>().Property(t => t.Title).HasMaxLength(200);
        modelBuilder.Entity<TaskItem>().Property(t => t.Description).HasMaxLength(1000);
        modelBuilder.Entity<TaskItem>().Property(t => t.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        modelBuilder.Entity<TaskItem>().HasOne(t => t.Project).WithMany(p => p.Tasks).HasForeignKey(t => t.ProjectId).OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Tag>().HasIndex(t => t.Name).IsUnique();
        modelBuilder.Entity<Tag>().Property(t => t.Name).HasMaxLength(50);
        modelBuilder.Entity<Tag>().HasMany(t => t.Tasks).WithMany(t => t.Tags);
    }
}
