using System.ComponentModel.DataAnnotations;

namespace MinhaPrimeiraAPI.Models
{
    public class Muscle
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public ICollection<ExerciseMuscle> ExerciseMuscles { get; set; } = [];
    }
}