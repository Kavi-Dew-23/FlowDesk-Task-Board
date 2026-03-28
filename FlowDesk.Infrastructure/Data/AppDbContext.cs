using FlowDesk.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace FlowDesk.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Project> Projects => Set<Project>();
        public DbSet<TaskItem> Tasks => Set<TaskItem>();
        public DbSet<ProjectMember> ProjectMembers => Set<ProjectMember>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ProjectMember uses a composite key (two columns together = unique)
            modelBuilder.Entity<ProjectMember>()
                .HasKey(pm => new { pm.ProjectId, pm.UserId });

            // Store enums as readable strings in the database
            modelBuilder.Entity<TaskItem>()
                .Property(t => t.Status).HasConversion<string>();

            modelBuilder.Entity<TaskItem>()
                .Property(t => t.Priority).HasConversion<string>();

            modelBuilder.Entity<User>()
                .Property(u => u.Role).HasConversion<string>();

            // Email must be unique across all users
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email).IsUnique();
        }
    }
}
