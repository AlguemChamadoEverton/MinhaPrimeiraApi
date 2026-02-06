using MinhaPrimeiraAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MinhaPrimeiraAPI.Services.ExerciseService
{
    public class ExerciseDTO
    {
        public List<string> MuscleName { get; set; } = new List<string>();
        [Required]
        public List<ExerciseMainMuscle> ExercisesMainMuscle { get; set; } = [];
    }

    public class ExerciseMainMuscle
    {
        public string ExerciseName { get; set; }
        [Required] [MinLength(1)] 
        public string MainMuscle { get; set; }
    }
}
