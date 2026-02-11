using System.ComponentModel.DataAnnotations;

namespace MinhaPrimeiraAPI.Models
{
    public class Equipment
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;

        public List<Exercise> Exercises = new List<Exercise>();
    }
}
