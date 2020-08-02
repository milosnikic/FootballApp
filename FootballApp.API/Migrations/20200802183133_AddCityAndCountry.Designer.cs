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
    [Migration("20200802183133_AddCityAndCountry")]
    partial class AddCityAndCountry
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

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Countries");
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

            modelBuilder.Entity("FootballApp.API.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CityId");

                    b.Property<int?>("CountryId");

                    b.Property<DateTime>("Created");

                    b.Property<DateTime>("DateOfBirth");

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
                    b.HasOne("FootballApp.API.Models.User", "CreatedBy")
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

            modelBuilder.Entity("FootballApp.API.Models.Photo", b =>
                {
                    b.HasOne("FootballApp.API.Models.User", "User")
                        .WithMany("Photos")
                        .HasForeignKey("UserId")
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
