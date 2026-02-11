using Azure;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinhaPrimeiraAPI.Models
{
    public class Exercise
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public bool IsCustom { get; set; } = true;
        [Required]
        public List<Muscle> Muscles { get; set; } = [];
        public ICollection<ExerciseMuscle> ExerciseMuscles { get; set; } = [];
        public ICollection<Routine> Routines { get; set; } = [];
        [Required]
        public int EquipmentId { get; set; }
        [Required]
        public Equipment Equipment { get; set; } = new Equipment();
        public ICollection<ExerciseRoutine> ExerciseRoutines { get; set; } = null!;
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
