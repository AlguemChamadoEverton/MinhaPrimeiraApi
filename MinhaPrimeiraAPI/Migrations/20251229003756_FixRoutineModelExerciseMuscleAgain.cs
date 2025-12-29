using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MinhaPrimeiraAPI.Migrations
{
    /// <inheritdoc />
    public partial class FixRoutineModelExerciseMuscleAgain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseMuscles_Routines_RoutineModelId",
                table: "ExerciseMuscles");

            migrationBuilder.DropIndex(
                name: "IX_ExerciseMuscles_RoutineModelId",
                table: "ExerciseMuscles");

            migrationBuilder.DropColumn(
                name: "RoutineModelId",
                table: "ExerciseMuscles");

            migrationBuilder.CreateTable(
                name: "ExerciseMuscleModelRoutineModel",
                columns: table => new
                {
                    ExerciseMusclesId = table.Column<int>(type: "int", nullable: false),
                    RoutinesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseMuscleModelRoutineModel", x => new { x.ExerciseMusclesId, x.RoutinesId });
                    table.ForeignKey(
                        name: "FK_ExerciseMuscleModelRoutineModel_ExerciseMuscles_ExerciseMusclesId",
                        column: x => x.ExerciseMusclesId,
                        principalTable: "ExerciseMuscles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExerciseMuscleModelRoutineModel_Routines_RoutinesId",
                        column: x => x.RoutinesId,
                        principalTable: "Routines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseMuscleModelRoutineModel_RoutinesId",
                table: "ExerciseMuscleModelRoutineModel",
                column: "RoutinesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExerciseMuscleModelRoutineModel");

            migrationBuilder.AddColumn<int>(
                name: "RoutineModelId",
                table: "ExerciseMuscles",
                type: "int",
                nullable: true);

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
        }
    }
}
