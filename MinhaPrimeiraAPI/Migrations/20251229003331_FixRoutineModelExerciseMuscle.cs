using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MinhaPrimeiraAPI.Migrations
{
    /// <inheritdoc />
    public partial class FixRoutineModelExerciseMuscle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExerciseModelRoutineModel");

            migrationBuilder.AddColumn<int>(
                name: "ExerciseModelId",
                table: "Routines",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RoutineModelId",
                table: "ExerciseMuscles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Routines_ExerciseModelId",
                table: "Routines",
                column: "ExerciseModelId");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseMuscles_RoutineModelId",
                table: "ExerciseMuscles",
                column: "RoutineModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseMuscles_Routines_RoutineModelId",
                table: "ExerciseMuscles",
                column: "RoutineModelId",
                principalTable: "Routines",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Routines_Exercises_ExerciseModelId",
                table: "Routines",
                column: "ExerciseModelId",
                principalTable: "Exercises",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseMuscles_Routines_RoutineModelId",
                table: "ExerciseMuscles");

            migrationBuilder.DropForeignKey(
                name: "FK_Routines_Exercises_ExerciseModelId",
                table: "Routines");

            migrationBuilder.DropIndex(
                name: "IX_Routines_ExerciseModelId",
                table: "Routines");

            migrationBuilder.DropIndex(
                name: "IX_ExerciseMuscles_RoutineModelId",
                table: "ExerciseMuscles");

            migrationBuilder.DropColumn(
                name: "ExerciseModelId",
                table: "Routines");

            migrationBuilder.DropColumn(
                name: "RoutineModelId",
                table: "ExerciseMuscles");

            migrationBuilder.CreateTable(
                name: "ExerciseModelRoutineModel",
                columns: table => new
                {
                    ExercisesId = table.Column<int>(type: "int", nullable: false),
                    RoutinesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseModelRoutineModel", x => new { x.ExercisesId, x.RoutinesId });
                    table.ForeignKey(
                        name: "FK_ExerciseModelRoutineModel_Exercises_ExercisesId",
                        column: x => x.ExercisesId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExerciseModelRoutineModel_Routines_RoutinesId",
                        column: x => x.RoutinesId,
                        principalTable: "Routines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseModelRoutineModel_RoutinesId",
                table: "ExerciseModelRoutineModel",
                column: "RoutinesId");
        }
    }
}
