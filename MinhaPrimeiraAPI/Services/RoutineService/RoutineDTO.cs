using MinhaPrimeiraAPI.Models;

namespace MinhaPrimeiraAPI.Services.RoutineService
{
    public class RoutineDTO
    {
        public string Name { get; set; } = string.Empty;
        public List<(int, List<Set>)> Exercise { get; set; } = new List<(int, List<Set>)> ();
    }
}
