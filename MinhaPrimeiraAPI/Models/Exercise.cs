using Azure;
using System.ComponentModel.DataAnnotations;

namespace MinhaPrimeiraAPI.Models
{
    public class Exercise
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public bool IsCustom { get; set; } = true;
        [Required]
        public List<Muscle> Muscles { get; set; } = [];
        public List<ExerciseMuscle> ExerciseMuscles { get; set; } = [];
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
