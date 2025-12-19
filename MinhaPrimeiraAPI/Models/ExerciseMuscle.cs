namespace MinhaPrimeiraAPI.Models
{
    public class ExerciseMuscle
    {
        public int ExerciseId { get; set; }
        public int MuscleId { get; set; }
        public bool Type { get; set; }
        public ExerciseModel Exercise { get; set; } = null!;
        public MuscleModel Muscle { get; set; } = null!;
    }
}
