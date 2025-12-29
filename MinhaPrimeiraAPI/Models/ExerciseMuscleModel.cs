using Microsoft.EntityFrameworkCore;

namespace MinhaPrimeiraAPI.Models
{
    public class ExerciseMuscleModel
    {
        public int Id { get; set; }
        public int ExerciseId { get; set; }
        public int MuscleId { get; set; }
        public bool Type { get; set; }
        public ICollection<RoutineModel> Routines { get; set; } = new List<RoutineModel>();
        public ExerciseModel Exercise { get; set; } = null!;
        public MuscleModel Muscle { get; set; } = null!;
    }
}
