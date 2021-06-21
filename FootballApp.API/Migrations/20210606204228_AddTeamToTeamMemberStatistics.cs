using Microsoft.EntityFrameworkCore.Migrations;

namespace FootballApp.API.Migrations
{
    public partial class AddTeamToTeamMemberStatistics : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TeamId",
                table: "TeamMemberStatistics",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeamMemberStatistics_TeamId",
                table: "TeamMemberStatistics",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_TeamMemberStatistics_Teams_TeamId",
                table: "TeamMemberStatistics",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeamMemberStatistics_Teams_TeamId",
                table: "TeamMemberStatistics");

            migrationBuilder.DropIndex(
                name: "IX_TeamMemberStatistics_TeamId",
                table: "TeamMemberStatistics");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "TeamMemberStatistics");
        }
    }
}
