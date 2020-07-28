using FootballApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FootballApp.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { }
        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Visit> Visits { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Memberships fluent api creation
            modelBuilder.Entity<Membership>().HasKey(m => new { m.UserId, m.GroupId });

            modelBuilder.Entity<Membership>()
                .HasOne<User>(m => m.User)
                .WithMany(u => u.Memberships)
                .HasForeignKey(m => m.UserId);

            modelBuilder.Entity<Membership>()
                .HasOne<Group>(m => m.Group)
                .WithMany(u => u.Memberships)
                .HasForeignKey(m => m.GroupId);

            // Visits fluent api creation
            modelBuilder.Entity<Visit>().HasKey(v => new { v.VisitorId, v.VisitedId, v.DateVisited });

            modelBuilder.Entity<Visit>()
                .HasOne<User>(v => v.Visitor)
                .WithMany(u => u.Visitors)
                .HasForeignKey(v => v.VisitorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Visit>()
                .HasOne<User>(v => v.Visited)
                .WithMany(u => u.Visiteds)
                .HasForeignKey(v => v.VisitedId)
                .OnDelete(DeleteBehavior.Restrict);

            // Comments fluent api creation
            modelBuilder.Entity<Comment>().HasKey(c => new { c.Id, c.CommenterId, c.CommentedId });

            modelBuilder.Entity<Comment>()
                .HasOne<User>(c => c.Commenter)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.CommenterId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comment>()
                .HasOne<User>(c => c.Commented)
                .WithMany(u => u.Commented)
                .HasForeignKey(c => c.CommentedId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}