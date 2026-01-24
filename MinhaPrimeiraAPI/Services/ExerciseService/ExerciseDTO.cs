using MinhaPrimeiraAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MinhaPrimeiraAPI.Services.ExerciseService
{
    public class ExerciseDTO
    {
        public List<string> ExerciseName { get; set; } = new List<string>();
        public List<string> MuscleName { get; set; } = new List<string>();
        [Required]
        [MinLength(1)]
        public List<string> MainMuscle { get; set; } = [];
    }
}
