using Microsoft.EntityFrameworkCore.Migrations;

namespace FootballApp.API.Migrations
{
    public partial class AddDateVisitedToPKVisit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Visits",
                table: "Visits");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Visits",
                table: "Visits",
                columns: new[] { "VisitorId", "VisitedId", "DateVisited" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Visits",
                table: "Visits");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Visits",
                table: "Visits",
                columns: new[] { "VisitorId", "VisitedId" });
        }
    }
}
