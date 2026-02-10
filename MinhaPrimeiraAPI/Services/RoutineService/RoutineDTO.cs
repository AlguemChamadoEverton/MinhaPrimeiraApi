using System.ComponentModel.DataAnnotations;
using MinhaPrimeiraAPI.Models;

namespace MinhaPrimeiraAPI.Services.RoutineService
{
    public class RoutineDto
    {
        public string Name { get; set; } = string.Empty;
        [Required]
        public List<ExerciseRoutineDto> ErDtos {get; set; }
        public List<ExerciseMuscleDto> Exercises { get; set;} = new List<ExerciseMuscleDto>();
        public List<string> Muscles { get; set; } = new List<string>();
    }
    public class ExerciseRoutineDto
    {
        public int ExerciseId { get; set; }
        public List<SetsDto> Sets { get; set; } = new List<SetsDto>();
    }

    public class ExerciseMuscleDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Muscle { get; set; }
    }

    public class SetsDto
    {
        public int Id { get; set; }
        [Required]
        public int Reps { get; set; }
        [Required]
        public int Weight { get; set; }
    }
}
