using System.ComponentModel.DataAnnotations;

namespace MinhaPrimeiraAPI.Models
{
    public class ExerciseModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public ICollection<ExerciseMuscleModel> ExerciseMuscles { get; set; } = [];
    }
}
