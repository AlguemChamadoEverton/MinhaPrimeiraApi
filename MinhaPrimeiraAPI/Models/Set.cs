using System.ComponentModel.DataAnnotations;

namespace MinhaPrimeiraAPI.Models
{
    public class Set
    {
        public int Id { get; set; }
        [Required]
        public int Reps { get; set; }
        public ExerciseRoutine ExerciseRoutine { get; set; } = null!;
    }
}