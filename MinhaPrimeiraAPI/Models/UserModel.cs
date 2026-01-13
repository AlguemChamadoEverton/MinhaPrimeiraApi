using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MinhaPrimeiraAPI.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        public List<ExerciseModel> Exercises { get; set; } = new List<ExerciseModel>();
        public ICollection<RoutineModel> Routines { get; set; } = new List<RoutineModel>();
    }
}
