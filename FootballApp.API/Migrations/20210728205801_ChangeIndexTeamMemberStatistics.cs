using Microsoft.EntityFrameworkCore.Migrations;

namespace FootballApp.API.Migrations
{
    public partial class ChangeIndexTeamMemberStatistics : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TeamMemberStatistics_TeamMemberId",
                table: "TeamMemberStatistics");

            migrationBuilder.CreateIndex(
                name: "IX_TeamMemberStatistics_TeamMemberId",
                table: "TeamMemberStatistics",
                column: "TeamMemberId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TeamMemberStatistics_TeamMemberId",
                table: "TeamMemberStatistics");

            migrationBuilder.CreateIndex(
                name: "IX_TeamMemberStatistics_TeamMemberId",
                table: "TeamMemberStatistics",
                column: "TeamMemberId");
        }
    }
}
