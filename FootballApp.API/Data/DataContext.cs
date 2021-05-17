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
        public DbSet<GainedAchievement> GainedAchievements { get; set; }
        public DbSet<Achievement> Achievements { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<PowerUser> PowerUsers { get; set; }
        public DbSet<CommonUser> CommonUsers { get; set; }
        public DbSet<MatchStatus> MatchStatuses { get; set; }
        public DbSet<Matchday> Matchdays { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<ChatUser> ChatUsers { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }
        public DbSet<MatchPlayed> MatchPlayeds { get; set; }
        public DbSet<TeamMemberStatistic> TeamMemberStatistics { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Locations
            modelBuilder.Entity<Location>()
                .HasOne(l => l.Country)
                .WithMany(c => c.Locations)
                .HasForeignKey(l => l.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Location>()
                .HasOne(l => l.City)
                .WithMany(c => c.Locations)
                .HasForeignKey(l => l.CityId)
                .OnDelete(DeleteBehavior.Restrict);

            // Memberships fluent api creation
            modelBuilder.Entity<Membership>().HasKey(m => new { m.UserId, m.GroupId });

            modelBuilder.Entity<Membership>()
                .HasOne<User>(m => m.User)
                .WithMany(u => u.Memberships)
                .HasForeignKey(m => m.UserId);

            modelBuilder.Entity<Membership>()
                .HasOne<Group>(m => m.Group)
                .WithMany(g => g.Memberships)
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

            // Friendships fluent api creation
            modelBuilder.Entity<Friendship>().HasKey(f => new { f.SenderId, f.ReceiverId });

            modelBuilder.Entity<Friendship>()
                .HasOne<User>(f => f.Sender)
                .WithMany(u => u.FriendshipsSent)
                .HasForeignKey(f => f.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Friendship>()
                .HasOne<User>(f => f.Receiver)
                .WithMany(u => u.FriendshipsReceived)
                .HasForeignKey(f => f.ReceiverId)
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

            // Achievements fluent api creation
            modelBuilder.Entity<GainedAchievement>().HasKey(a => new { a.Id, a.AchievementId, a.UserId });

            modelBuilder.Entity<GainedAchievement>()
                .HasOne<User>(a => a.User)
                .WithMany(u => u.GainedAchievements)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<GainedAchievement>()
                .HasOne<Achievement>(a => a.Achievement)
                .WithMany(a => a.GainedAchievements)
                .HasForeignKey(a => a.AchievementId)
                .OnDelete(DeleteBehavior.Restrict);

            // Messages fluent api creation
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany(u => u.Messages)
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            // Groups fluent api
            modelBuilder.Entity<Group>()
                .HasOne(g => g.CreatedBy)
                .WithMany(pu => pu.GroupsCreated)
                .OnDelete(DeleteBehavior.Restrict);

            // City fluent api
            modelBuilder.Entity<City>()
                .HasMany(c => c.Users)
                .WithOne(u => u.City)
                .OnDelete(DeleteBehavior.Restrict);

            // Country fluent api
            modelBuilder.Entity<Country>()
                .HasMany(c => c.Users)
                .WithOne(u => u.Country)
                .OnDelete(DeleteBehavior.Restrict);

            // User inheritance implemented
            modelBuilder.Entity<CommonUser>().ToTable("CommonUsers");
            modelBuilder.Entity<PowerUser>().ToTable("PowerUsers");

            // Match status fluent api
            modelBuilder.Entity<MatchStatus>().HasKey(s => new { s.UserId, s.MatchdayId });

            modelBuilder.Entity<MatchStatus>()
                .HasOne(s => s.Matchday)
                .WithMany(m => m.MatchStatuses)
                .HasForeignKey(s => s.MatchdayId);

            modelBuilder.Entity<MatchStatus>()
                .HasOne(s => s.User)
                .WithMany(u => u.MatchStatuses)
                .HasForeignKey(s => s.UserId);

            // Chat user fluent api
            modelBuilder.Entity<ChatUser>()
                .HasKey(x => new { x.UserId, x.ChatId });

            // Matchplayed fluent api
            modelBuilder.Entity<MatchPlayed>()
                .HasOne<Team>(h => h.Home)
                .WithMany(t => t.HomeMatches)
                .HasForeignKey(h => h.HomeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MatchPlayed>()
                .HasOne<Team>(a => a.Away)
                .WithMany(t => t.AwayMatches)
                .HasForeignKey(a => a.AwayId)
                .OnDelete(DeleteBehavior.Restrict);

            // Team member fluent api
            modelBuilder.Entity<TeamMember>()
                .Property(x => x.Position)
                .IsRequired(false);
            
            // Team fluent api
            modelBuilder.Entity<Team>()
                .Property(x => x.Name)
                .HasMaxLength(30)
                .IsRequired(false);
        }
    }
}