using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpartanFitness.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Added_Savedtables_to_User : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SavedExerciseIds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExerciseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavedExerciseIds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SavedExerciseIds_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SavedMuscleGroupIds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MuscleGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavedMuscleGroupIds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SavedMuscleGroupIds_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SavedMuscleIds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MuscleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavedMuscleIds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SavedMuscleIds_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SavedExerciseIds_UserId",
                table: "SavedExerciseIds",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SavedMuscleGroupIds_UserId",
                table: "SavedMuscleGroupIds",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SavedMuscleIds_UserId",
                table: "SavedMuscleIds",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SavedExerciseIds");

            migrationBuilder.DropTable(
                name: "SavedMuscleGroupIds");

            migrationBuilder.DropTable(
                name: "SavedMuscleIds");
        }
    }
}
