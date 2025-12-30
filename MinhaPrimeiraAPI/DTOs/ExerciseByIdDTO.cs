using MinhaPrimeiraAPI.Models;

namespace MinhaPrimeiraAPI.DTOs
{
    public class ExerciseByIdDTO
    {
        public string ExerciseName { get; set; } = String.Empty;
        public List<string> MuscleName { get; set; } = [];
    }
}
