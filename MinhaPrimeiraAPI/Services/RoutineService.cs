using MinhaPrimeiraAPI.Models;
using System.Security.Claims;

namespace MinhaPrimeiraAPI.Services
{
    public class RoutineService
    {
        public Task<IResult> CreateRoutine(ClaimsPrincipal jwt, RoutineModel routine) 
        {
            var email = jwt.FindFirst(ClaimTypes.Email)?.Value;

        }
        public Task<IResult> GetRoutine(ClaimsPrincipal jwt)
        {
            var email = jwt.FindFirst(ClaimTypes.Email)?.Value;
        }
    }
}
