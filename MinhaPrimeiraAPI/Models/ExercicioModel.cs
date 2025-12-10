using System.ComponentModel.DataAnnotations;

namespace MinhaPrimeiraAPI.Models
{
    public class ExercicioModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string TargetMuscle { get; set; }
    }
}
