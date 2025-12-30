using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MinhaPrimeiraAPI.Migrations
{
    /// <inheritdoc />
    public partial class ArrumandoAlgunsProblemasDoExerciseMuscle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Routines_Exercises_ExerciseModelId",
                table: "Routines");

            migrationBuilder.DropTable(
                name: "ExerciseModelMuscleModel");

            migrationBuilder.DropIndex(
                name: "IX_Routines_ExerciseModelId",
                table: "Routines");

            migrationBuilder.DropColumn(
                name: "ExerciseModelId",
                table: "Routines");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExerciseModelId",
                table: "Routines",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ExerciseModelMuscleModel",
                columns: table => new
                {
                    ExercisesId = table.Column<int>(type: "int", nullable: false),
                    MusclesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseModelMuscleModel", x => new { x.ExercisesId, x.MusclesId });
                    table.ForeignKey(
                        name: "FK_ExerciseModelMuscleModel_Exercises_ExercisesId",
                        column: x => x.ExercisesId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExerciseModelMuscleModel_Muscles_MusclesId",
                        column: x => x.MusclesId,
                        principalTable: "Muscles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Routines_ExerciseModelId",
                table: "Routines",
                column: "ExerciseModelId");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseModelMuscleModel_MusclesId",
                table: "ExerciseModelMuscleModel",
                column: "MusclesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Routines_Exercises_ExerciseModelId",
                table: "Routines",
                column: "ExerciseModelId",
                principalTable: "Exercises",
                principalColumn: "Id");
        }
    }
}
