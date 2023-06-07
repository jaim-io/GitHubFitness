using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpartanFitness.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Changed_table_names : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SavedExerciseIds");

            migrationBuilder.DropTable(
                name: "SavedMuscleGroupIds");

            migrationBuilder.DropTable(
                name: "SavedMuscleIds");

            migrationBuilder.DropColumn(
                name: "MuscleGroupId",
                table: "Muscles");

            migrationBuilder.CreateTable(
                name: "MuscleGroupMuscleIds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MuscleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MuscleGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MuscleGroupMuscleIds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MuscleGroupMuscleIds_MuscleGroups_MuscleGroupId",
                        column: x => x.MuscleGroupId,
                        principalTable: "MuscleGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSavedExerciseIds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExerciseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSavedExerciseIds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSavedExerciseIds_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSavedMuscleGroupIds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MuscleGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSavedMuscleGroupIds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSavedMuscleGroupIds_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSavedMuscleIds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MuscleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSavedMuscleIds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSavedMuscleIds_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MuscleGroupMuscleIds_MuscleGroupId",
                table: "MuscleGroupMuscleIds",
                column: "MuscleGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSavedExerciseIds_UserId",
                table: "UserSavedExerciseIds",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSavedMuscleGroupIds_UserId",
                table: "UserSavedMuscleGroupIds",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSavedMuscleIds_UserId",
                table: "UserSavedMuscleIds",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MuscleGroupMuscleIds");

            migrationBuilder.DropTable(
                name: "UserSavedExerciseIds");

            migrationBuilder.DropTable(
                name: "UserSavedMuscleGroupIds");

            migrationBuilder.DropTable(
                name: "UserSavedMuscleIds");

            migrationBuilder.AddColumn<Guid>(
                name: "MuscleGroupId",
                table: "Muscles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "SavedExerciseIds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExerciseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MuscleGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MuscleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
    }
}
