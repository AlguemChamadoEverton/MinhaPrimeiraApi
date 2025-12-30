using MinhaPrimeiraAPI.Models;

namespace MinhaPrimeiraAPI.DTOs
{
    public class ExerciseMuscleDTO
    {
        public List<ExerciseModel> Exercises { get; set; } = [];
        public List<MuscleModel> Muscles { get; set; } = [];
    }
}
