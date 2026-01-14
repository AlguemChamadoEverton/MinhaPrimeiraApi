using Azure;
using System.ComponentModel.DataAnnotations;

namespace MinhaPrimeiraAPI.Models
{
    public class ExerciseMuscleRoutine
    {
        public int Id { get; set; }
        public int ExerciseMuscleId { get; set; }
        public int RoutineId { get; set; }

        public ExerciseMuscle ExerciseMuscle { get; set; } = null!;
        public Routine Routine { get; set; } = null!;
    }
}
