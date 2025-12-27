using System.ComponentModel.DataAnnotations;

namespace MinhaPrimeiraAPI.Models
{
    public class ExerciseModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string TargetMuscle { get; set; } = string.Empty;
        public ICollection<RoutineModel> Routines { get; set; } = new List<RoutineModel>();
        public List<ExerciseMuscleModel> ExerciseMuscles { get; } = [];
        public List<MuscleModel> Muscles { get; } = [];
    }
}
