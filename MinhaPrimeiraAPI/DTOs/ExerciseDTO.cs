using MinhaPrimeiraAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace MinhaPrimeiraAPI.DTOs
{
    public class ExerciseDTO
    {
        public List<string> ExerciseName { get; set; } = new List<string>();
        public List<string> MuscleName { get; set; } = new List<string>();
        [Required]
        public string MainMuscle { get; set; } = "";
    }
}
