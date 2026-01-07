using Azure;
using System.ComponentModel.DataAnnotations;

namespace MinhaPrimeiraAPI.Models
{
    public class ExerciseModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public List<MuscleModel> Muscles { get; set; } = [];
        public List<ExerciseMuscleModel> ExerciseMuscles { get; set; } = [];
    }
}
