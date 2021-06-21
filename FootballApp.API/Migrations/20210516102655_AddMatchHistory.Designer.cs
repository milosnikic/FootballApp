﻿// <auto-generated />
using System;
using FootballApp.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FootballApp.API.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20210516102655_AddMatchHistory")]
    partial class AddMatchHistory
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.14-servicing-32113")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("FootballApp.API.Models.Achievement", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Icon");

                    b.Property<string>("Name");

                    b.Property<int>("Points");

                    b.Property<int>("Value");

                    b.HasKey("Id");

                    b.ToTable("Achievements");
                });

            modelBuilder.Entity("FootballApp.API.Models.Chat", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.ToTable("Chats");
                });

            modelBuilder.Entity("FootballApp.API.Models.ChatUser", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("ChatId");

                    b.Property<int>("Role");

                    b.HasKey("UserId", "ChatId");

                    b.HasIndex("ChatId");

                    b.ToTable("ChatUsers");
                });

            modelBuilder.Entity("FootballApp.API.Models.City", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CountryId");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.ToTable("Cities");
                });

            modelBuilder.Entity("FootballApp.API.Models.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CommenterId");

                    b.Property<int>("CommentedId");

                    b.Property<string>("Content");

                    b.Property<DateTime>("Created");

                    b.HasKey("Id", "CommenterId", "CommentedId");

                    b.HasIndex("CommentedId");

                    b.HasIndex("CommenterId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("FootballApp.API.Models.Country", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Flag");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("FootballApp.API.Models.Friendship", b =>
                {
                    b.Property<int>("SenderId");

                    b.Property<int>("ReceiverId");

                    b.Property<bool>("Accepted");

                    b.HasKey("SenderId", "ReceiverId");

                    b.HasIndex("ReceiverId");

                    b.ToTable("Friendships");
                });

            modelBuilder.Entity("FootballApp.API.Models.GainedAchievement", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AchievementId");

                    b.Property<int>("UserId");

                    b.Property<DateTime>("DateAchieved");

                    b.HasKey("Id", "AchievementId", "UserId");

                    b.HasIndex("AchievementId");

                    b.HasIndex("UserId");

                    b.ToTable("GainedAchievements");
                });

            modelBuilder.Entity("FootballApp.API.Models.Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CreatedById");

                    b.Property<DateTime>("DateCreated");

                    b.Property<string>("Description");

                    b.Property<byte[]>("Image");

                    b.Property<bool>("IsActive");

                    b.Property<int>("LocationId");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("LocationId");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("FootballApp.API.Models.Location", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CityId");

                    b.Property<int>("CountryId");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.HasIndex("CountryId");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("FootballApp.API.Models.Matchday", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DatePlaying");

                    b.Property<string>("Description");

                    b.Property<int?>("GroupId");

                    b.Property<int?>("LocationId");

                    b.Property<string>("Name");

                    b.Property<int>("NumberOfPlayers");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("LocationId");

                    b.ToTable("Matchdays");
                });

            modelBuilder.Entity("FootballApp.API.Models.MatchPlayed", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AwayGoals");

                    b.Property<int>("AwayId");

                    b.Property<DateTime>("DatePlayed");

                    b.Property<int>("HomeGoals");

                    b.Property<int>("HomeId");

                    b.HasKey("Id");

                    b.HasIndex("AwayId");

                    b.HasIndex("HomeId");

                    b.ToTable("MatchPlayeds");
                });

            modelBuilder.Entity("FootballApp.API.Models.MatchStatus", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("MatchdayId");

                    b.Property<bool?>("Checked");

                    b.Property<bool?>("Confirmed");

                    b.HasKey("UserId", "MatchdayId");

                    b.HasIndex("MatchdayId");

                    b.ToTable("MatchStatuses");
                });

            modelBuilder.Entity("FootballApp.API.Models.Membership", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("GroupId");

                    b.Property<bool>("Accepted");

                    b.Property<DateTime?>("DateAccepted");

                    b.Property<DateTime>("DateSent");

                    b.Property<bool>("Favorite");

                    b.Property<int>("MembershipStatus");

                    b.Property<int>("Role");

                    b.HasKey("UserId", "GroupId");

                    b.HasIndex("GroupId");

                    b.ToTable("Memberships");
                });

            modelBuilder.Entity("FootballApp.API.Models.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ChatId");

                    b.Property<string>("Content");

                    b.Property<DateTime?>("DateRead");

                    b.Property<bool>("IsRead");

                    b.Property<DateTime>("MessageSent");

                    b.Property<int>("SenderId");

                    b.HasKey("Id");

                    b.HasIndex("ChatId");

                    b.HasIndex("SenderId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("FootballApp.API.Models.Photo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateAdded");

                    b.Property<string>("Description");

                    b.Property<byte[]>("Image");

                    b.Property<bool>("IsMain");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Photos");
                });

            modelBuilder.Entity("FootballApp.API.Models.Team", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("MatchdayId");

                    b.Property<string>("Name")
                        .HasMaxLength(30);

                    b.HasKey("Id");

                    b.HasIndex("MatchdayId");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("FootballApp.API.Models.TeamMember", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("Position");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("TeamMembers");
                });

            modelBuilder.Entity("FootballApp.API.Models.TeamMemberStatistic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Assists");

                    b.Property<int>("Goals");

                    b.Property<int>("MatchPlayedId");

                    b.Property<int>("MinutesPlayed");

                    b.Property<double>("Rating");

                    b.Property<int>("TeamMemberId");

                    b.HasKey("Id");

                    b.HasIndex("MatchPlayedId");

                    b.HasIndex("TeamMemberId");

                    b.ToTable("TeamMemberStatistics");
                });

            modelBuilder.Entity("FootballApp.API.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CityId");

                    b.Property<int?>("CountryId");

                    b.Property<DateTime>("Created");

                    b.Property<DateTime>("DateOfBirth");

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<string>("Email");

                    b.Property<string>("Firstname");

                    b.Property<int>("Gender");

                    b.Property<bool>("IsActive");

                    b.Property<DateTime?>("LastActive");

                    b.Property<string>("Lastname");

                    b.Property<byte[]>("PasswordHash");

                    b.Property<byte[]>("PasswordSalt");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.HasIndex("CountryId");

                    b.ToTable("Users");

                    b.HasDiscriminator<string>("Discriminator").HasValue("User");
                });

            modelBuilder.Entity("FootballApp.API.Models.Visit", b =>
                {
                    b.Property<int>("VisitorId");

                    b.Property<int>("VisitedId");

                    b.Property<DateTime>("DateVisited");

                    b.HasKey("VisitorId", "VisitedId", "DateVisited");

                    b.HasIndex("VisitedId");

                    b.ToTable("Visits");
                });

            modelBuilder.Entity("FootballApp.API.Models.CommonUser", b =>
                {
                    b.HasBaseType("FootballApp.API.Models.User");

                    b.Property<bool>("IsSubscribed");

                    b.ToTable("CommonUsers");

                    b.HasDiscriminator().HasValue("CommonUser");
                });

            modelBuilder.Entity("FootballApp.API.Models.PowerUser", b =>
                {
                    b.HasBaseType("FootballApp.API.Models.User");

                    b.Property<int>("NumberOfGroupsCreated");

                    b.ToTable("PowerUsers");

                    b.HasDiscriminator().HasValue("PowerUser");
                });

            modelBuilder.Entity("FootballApp.API.Models.ChatUser", b =>
                {
                    b.HasOne("FootballApp.API.Models.Chat", "Chat")
                        .WithMany("Users")
                        .HasForeignKey("ChatId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FootballApp.API.Models.User", "User")
                        .WithMany("Chats")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FootballApp.API.Models.City", b =>
                {
                    b.HasOne("FootballApp.API.Models.Country", "Country")
                        .WithMany("Cities")
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FootballApp.API.Models.Comment", b =>
                {
                    b.HasOne("FootballApp.API.Models.User", "Commented")
                        .WithMany("Commented")
                        .HasForeignKey("CommentedId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("FootballApp.API.Models.User", "Commenter")
                        .WithMany("Comments")
                        .HasForeignKey("CommenterId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("FootballApp.API.Models.Friendship", b =>
                {
                    b.HasOne("FootballApp.API.Models.User", "Receiver")
                        .WithMany("FriendshipsReceived")
                        .HasForeignKey("ReceiverId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("FootballApp.API.Models.User", "Sender")
                        .WithMany("FriendshipsSent")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("FootballApp.API.Models.GainedAchievement", b =>
                {
                    b.HasOne("FootballApp.API.Models.Achievement", "Achievement")
                        .WithMany("GainedAchievements")
                        .HasForeignKey("AchievementId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("FootballApp.API.Models.User", "User")
                        .WithMany("GainedAchievements")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("FootballApp.API.Models.Group", b =>
                {
                    b.HasOne("FootballApp.API.Models.PowerUser", "CreatedBy")
                        .WithMany("GroupsCreated")
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("FootballApp.API.Models.Location", "Location")
                        .WithMany("Groups")
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FootballApp.API.Models.Location", b =>
                {
                    b.HasOne("FootballApp.API.Models.City", "City")
                        .WithMany("Locations")
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("FootballApp.API.Models.Country", "Country")
                        .WithMany("Locations")
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("FootballApp.API.Models.Matchday", b =>
                {
                    b.HasOne("FootballApp.API.Models.Group", "Group")
                        .WithMany("Matchdays")
                        .HasForeignKey("GroupId");

                    b.HasOne("FootballApp.API.Models.Location", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId");
                });

            modelBuilder.Entity("FootballApp.API.Models.MatchPlayed", b =>
                {
                    b.HasOne("FootballApp.API.Models.Team", "Away")
                        .WithMany("AwayMatches")
                        .HasForeignKey("AwayId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("FootballApp.API.Models.Team", "Home")
                        .WithMany("HomeMatches")
                        .HasForeignKey("HomeId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("FootballApp.API.Models.MatchStatus", b =>
                {
                    b.HasOne("FootballApp.API.Models.Matchday", "Matchday")
                        .WithMany("MatchStatuses")
                        .HasForeignKey("MatchdayId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FootballApp.API.Models.User", "User")
                        .WithMany("MatchStatuses")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FootballApp.API.Models.Membership", b =>
                {
                    b.HasOne("FootballApp.API.Models.Group", "Group")
                        .WithMany("Memberships")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FootballApp.API.Models.User", "User")
                        .WithMany("Memberships")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FootballApp.API.Models.Message", b =>
                {
                    b.HasOne("FootballApp.API.Models.Chat", "Chat")
                        .WithMany("Messages")
                        .HasForeignKey("ChatId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FootballApp.API.Models.User", "Sender")
                        .WithMany("Messages")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("FootballApp.API.Models.Photo", b =>
                {
                    b.HasOne("FootballApp.API.Models.User", "User")
                        .WithMany("Photos")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FootballApp.API.Models.Team", b =>
                {
                    b.HasOne("FootballApp.API.Models.Matchday", "Matchday")
                        .WithMany("Teams")
                        .HasForeignKey("MatchdayId");
                });

            modelBuilder.Entity("FootballApp.API.Models.TeamMember", b =>
                {
                    b.HasOne("FootballApp.API.Models.User", "User")
                        .WithMany("TeamMembers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FootballApp.API.Models.TeamMemberStatistic", b =>
                {
                    b.HasOne("FootballApp.API.Models.MatchPlayed", "MatchPlayed")
                        .WithMany("TeamMemberStatistics")
                        .HasForeignKey("MatchPlayedId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FootballApp.API.Models.TeamMember", "TeamMember")
                        .WithMany("TeamMemberStatistics")
                        .HasForeignKey("TeamMemberId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FootballApp.API.Models.User", b =>
                {
                    b.HasOne("FootballApp.API.Models.City", "City")
                        .WithMany("Users")
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("FootballApp.API.Models.Country", "Country")
                        .WithMany("Users")
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("FootballApp.API.Models.Visit", b =>
                {
                    b.HasOne("FootballApp.API.Models.User", "Visited")
                        .WithMany("Visiteds")
                        .HasForeignKey("VisitedId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("FootballApp.API.Models.User", "Visitor")
                        .WithMany("Visitors")
                        .HasForeignKey("VisitorId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
