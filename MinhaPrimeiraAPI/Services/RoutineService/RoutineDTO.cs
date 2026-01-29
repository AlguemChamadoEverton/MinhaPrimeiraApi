using System.ComponentModel.DataAnnotations;
using MinhaPrimeiraAPI.Models;

namespace MinhaPrimeiraAPI.Services.RoutineService
{
    public class RoutineDto
    {
        public string Name { get; set; } = string.Empty;
        [Required]
        public List<(int, List<Set>)> IdSet { get; set; }
    }
}
