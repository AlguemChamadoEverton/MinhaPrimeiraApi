using MinhaPrimeiraAPI.Endpoints;
using MinhaPrimeiraAPI.Models;
using System.Security.Claims;

namespace MinhaPrimeiraAPI.Services.ExerciseService
{
    public interface IExerciseService
    {
        public static abstract Task<IResult> GetExercises(ClaimsPrincipal jwt);
        public static abstract Task<IResult> GetExerciseById(int id, ClaimsPrincipal jwt);

        public static abstract Task<IResult> CreateExercise(ExerciseDto ex, ClaimsPrincipal jwt);

        public static abstract Task<IResult> DeleteExerciseById(int id, ClaimsPrincipal jwt);
    }
}
