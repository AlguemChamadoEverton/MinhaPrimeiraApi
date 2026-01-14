using Microsoft.EntityFrameworkCore;

namespace MinhaPrimeiraAPI.Models
{
    public class ExerciseMuscle
    {
        public int Id { get; set; }
        public int ExerciseId { get; set; }
        public int MuscleId { get; set; }
        public bool Type { get; set; } = false;
        public ICollection<ExerciseMuscleRoutine> ExerciseMuscleRoutines { get; set; } = null!;
        public Exercise Exercise { get; set; } = null!;
        public Muscle Muscle { get; set; } = null!;
    }
}
