using Microsoft.EntityFrameworkCore;

namespace MinhaPrimeiraAPI.Models
{
    public class ExerciseRoutine
    {
        public int Id { get; set; }
        public int ExerciseId { get; set; }
        public int RoutineId { get; set; }
        public int Sets { get; set; }
        public Exercise Exercise { get; set; } = null!;
        public Routine Routine { get; set; } = null!;
    }
}
