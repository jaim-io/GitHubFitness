using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpartanFitness.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Added_UniqueCode_to_PasswordRestToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UniqueCode",
                table: "PasswordResetTokens",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UniqueCode",
                table: "PasswordResetTokens");
        }
    }
}
