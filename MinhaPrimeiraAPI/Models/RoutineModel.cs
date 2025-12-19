using System.ComponentModel.DataAnnotations;

namespace MinhaPrimeiraAPI.Models
{
    public class RoutineModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public int UserId { get; set; }
        public UserModel User { get; set; } = null!;
        public ICollection<ExerciseModel> Exercises { get; set; } = new List<ExerciseModel>();
    }
}
