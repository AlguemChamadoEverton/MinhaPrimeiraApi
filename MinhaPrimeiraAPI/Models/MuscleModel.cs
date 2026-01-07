using System.ComponentModel.DataAnnotations;

namespace MinhaPrimeiraAPI.Models
{
    public class MuscleModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public List<ExerciseModel> Exercises { get; set; } = [];
        public ICollection<ExerciseMuscleModel> ExerciseMuscles { get; set; } = [];
    }
}