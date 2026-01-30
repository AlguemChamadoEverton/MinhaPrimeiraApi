using System.ComponentModel.DataAnnotations;
using MinhaPrimeiraAPI.Models;

namespace MinhaPrimeiraAPI.Services.RoutineService
{
    public class RoutineDto
    {
        public string Name { get; set; } = string.Empty;
        [Required]
        public List<ExerciseRoutineDto> ErDtos {get; set; }
    }
    public class ExerciseRoutineDto
    {
        public int ExerciseId { get; set; }
        public List<Set> Sets { get; set; } = new List<Set>();
    }   
    
}
