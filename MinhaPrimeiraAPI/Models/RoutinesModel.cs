using System.ComponentModel.DataAnnotations;

namespace MinhaPrimeiraAPI.Models
{
    public class RoutinesModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
