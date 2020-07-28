﻿// <auto-generated />
using System;
using FootballApp.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FootballApp.API.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.14-servicing-32113")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

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

            modelBuilder.Entity("FootballApp.API.Models.Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<bool>("IsActive");

                    b.Property<int>("LocationId");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("LocationId");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("FootballApp.API.Models.Location", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("City");

                    b.Property<string>("Country");

                    b.Property<decimal>("Latitude");

                    b.Property<decimal>("Longitude");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("FootballApp.API.Models.Membership", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("GroupId");

                    b.Property<bool>("Accepted");

                    b.Property<DateTime?>("DateAccepted");

                    b.Property<DateTime>("DateSent");

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

                    b.Property<string>("City");

                    b.Property<string>("Country");

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

            modelBuilder.Entity("FootballApp.API.Models.Group", b =>
                {
                    b.HasOne("FootballApp.API.Models.Location", "Location")
                        .WithMany("Groups")
                        .HasForeignKey("LocationId")
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

            modelBuilder.Entity("FootballApp.API.Models.Photo", b =>
                {
                    b.HasOne("FootballApp.API.Models.User", "User")
                        .WithMany("Photos")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
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
