using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MinhaPrimeiraAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Muscles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Muscles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Exercises",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsCustom = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercises", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Exercises_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Routines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Routines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Routines_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExerciseMuscles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExerciseId = table.Column<int>(type: "int", nullable: false),
                    MuscleId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseMuscles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExerciseMuscles_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExerciseMuscles_Muscles_MuscleId",
                        column: x => x.MuscleId,
                        principalTable: "Muscles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExerciseMuscleRoutine",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExerciseMuscleId = table.Column<int>(type: "int", nullable: false),
                    RoutineId = table.Column<int>(type: "int", nullable: false),
                    ExerciseMuscleId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseMuscleRoutine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExerciseMuscleRoutine_ExerciseMuscles_ExerciseMuscleId",
                        column: x => x.ExerciseMuscleId,
                        principalTable: "ExerciseMuscles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExerciseMuscleRoutine_ExerciseMuscles_ExerciseMuscleId1",
                        column: x => x.ExerciseMuscleId1,
                        principalTable: "ExerciseMuscles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ExerciseMuscleRoutine_Routines_RoutineId",
                        column: x => x.RoutineId,
                        principalTable: "Routines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseMuscleRoutine_ExerciseMuscleId",
                table: "ExerciseMuscleRoutine",
                column: "ExerciseMuscleId");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseMuscleRoutine_ExerciseMuscleId1",
                table: "ExerciseMuscleRoutine",
                column: "ExerciseMuscleId1");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseMuscleRoutine_RoutineId",
                table: "ExerciseMuscleRoutine",
                column: "RoutineId");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseMuscles_ExerciseId",
                table: "ExerciseMuscles",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseMuscles_MuscleId",
                table: "ExerciseMuscles",
                column: "MuscleId");

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_UserId",
                table: "Exercises",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Routines_UserId",
                table: "Routines",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExerciseMuscleRoutine");

            migrationBuilder.DropTable(
                name: "ExerciseMuscles");

            migrationBuilder.DropTable(
                name: "Routines");

            migrationBuilder.DropTable(
                name: "Exercises");

            migrationBuilder.DropTable(
                name: "Muscles");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
