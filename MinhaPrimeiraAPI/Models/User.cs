using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MinhaPrimeiraAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        public List<Exercise> Exercises { get; set; } = new List<Exercise>();
        public ICollection<Routine> Routines { get; set; } = new List<Routine>();
    }
}
