using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FootballApp.API.Migrations
{
    public partial class AddMatchHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TeamMembers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Position = table.Column<int>(nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamMembers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 30, nullable: true),
                    MatchdayId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teams_Matchdays_MatchdayId",
                        column: x => x.MatchdayId,
                        principalTable: "Matchdays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MatchPlayeds",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DatePlayed = table.Column<DateTime>(nullable: false),
                    HomeGoals = table.Column<int>(nullable: false),
                    AwayGoals = table.Column<int>(nullable: false),
                    HomeId = table.Column<int>(nullable: false),
                    AwayId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchPlayeds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MatchPlayeds_Teams_AwayId",
                        column: x => x.AwayId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MatchPlayeds_Teams_HomeId",
                        column: x => x.HomeId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TeamMemberStatistics",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Goals = table.Column<int>(nullable: false),
                    Assists = table.Column<int>(nullable: false),
                    MinutesPlayed = table.Column<int>(nullable: false),
                    Rating = table.Column<double>(nullable: false),
                    TeamMemberId = table.Column<int>(nullable: false),
                    MatchPlayedId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamMemberStatistics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamMemberStatistics_MatchPlayeds_MatchPlayedId",
                        column: x => x.MatchPlayedId,
                        principalTable: "MatchPlayeds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamMemberStatistics_TeamMembers_TeamMemberId",
                        column: x => x.TeamMemberId,
                        principalTable: "TeamMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MatchPlayeds_AwayId",
                table: "MatchPlayeds",
                column: "AwayId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchPlayeds_HomeId",
                table: "MatchPlayeds",
                column: "HomeId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamMembers_UserId",
                table: "TeamMembers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamMemberStatistics_MatchPlayedId",
                table: "TeamMemberStatistics",
                column: "MatchPlayedId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamMemberStatistics_TeamMemberId",
                table: "TeamMemberStatistics",
                column: "TeamMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_MatchdayId",
                table: "Teams",
                column: "MatchdayId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TeamMemberStatistics");

            migrationBuilder.DropTable(
                name: "MatchPlayeds");

            migrationBuilder.DropTable(
                name: "TeamMembers");

            migrationBuilder.DropTable(
                name: "Teams");
        }
    }
}
