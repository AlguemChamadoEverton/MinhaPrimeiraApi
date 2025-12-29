using System.ComponentModel.DataAnnotations;

namespace MinhaPrimeiraAPI.Models
{
    public class RoutineModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public int UserId { get; set; }
        public UserModel User { get; set; } = null!;
        [Required]
        public ICollection<ExerciseMuscleModel> ExerciseMuscles { get; set; } = new List<ExerciseMuscleModel>();
    }
}
