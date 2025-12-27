using System.ComponentModel.DataAnnotations;

namespace MinhaPrimeiraAPI.Models
{
    public class MuscleModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public List<ExerciseMuscleModel> ExerciseMuscles { get; } = [];
        public List<ExerciseModel> Exercises { get; } = [];
    }
}