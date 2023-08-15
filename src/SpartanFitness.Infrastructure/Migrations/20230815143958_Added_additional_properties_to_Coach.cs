using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpartanFitness.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Added_additional_properties_to_Coach : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Biography",
                table: "Coaches",
                type: "nvarchar(2048)",
                maxLength: 2048,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SocialMedia_FacebookUrl",
                table: "Coaches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SocialMedia_InstagramUrl",
                table: "Coaches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SocialMedia_LinkedInUrl",
                table: "Coaches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SocialMedia_WebsiteUrl",
                table: "Coaches",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Biography",
                table: "Coaches");

            migrationBuilder.DropColumn(
                name: "SocialMedia_FacebookUrl",
                table: "Coaches");

            migrationBuilder.DropColumn(
                name: "SocialMedia_InstagramUrl",
                table: "Coaches");

            migrationBuilder.DropColumn(
                name: "SocialMedia_LinkedInUrl",
                table: "Coaches");

            migrationBuilder.DropColumn(
                name: "SocialMedia_WebsiteUrl",
                table: "Coaches");
        }
    }
}
