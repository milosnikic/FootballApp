using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FootballApp.API.Migrations
{
    public partial class AddMatchdayAndMatchStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Matchdays",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    DatePlaying = table.Column<DateTime>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    NumberOfPlayers = table.Column<int>(nullable: false),
                    LocationId = table.Column<int>(nullable: true),
                    GroupId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matchdays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Matchdays_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Matchdays_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MatchStatuses",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    MatchdayId = table.Column<int>(nullable: false),
                    Checked = table.Column<bool>(nullable: true),
                    Confirmed = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchStatuses", x => new { x.UserId, x.MatchdayId });
                    table.ForeignKey(
                        name: "FK_MatchStatuses_Matchdays_MatchdayId",
                        column: x => x.MatchdayId,
                        principalTable: "Matchdays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MatchStatuses_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Matchdays_GroupId",
                table: "Matchdays",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Matchdays_LocationId",
                table: "Matchdays",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchStatuses_MatchdayId",
                table: "MatchStatuses",
                column: "MatchdayId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MatchStatuses");

            migrationBuilder.DropTable(
                name: "Matchdays");
        }
    }
}
