using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpartanFitness.Infrastructure.Migrations
{
    public partial class AddedCoachApplications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "CoachApplications",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "CoachApplications");
        }
    }
}
