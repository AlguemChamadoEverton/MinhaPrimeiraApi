using System.ComponentModel.DataAnnotations;

namespace MinhaPrimeiraAPI.Models
{
    public class Routine
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public List<ExerciseMuscleRoutine> ExerciseMuscleRoutine { get; set; } = null!;
    }
}
