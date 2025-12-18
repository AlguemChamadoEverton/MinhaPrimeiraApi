using MinhaPrimeiraAPI.Endpoints;
using MinhaPrimeiraAPI.Models;

namespace MinhaPrimeiraAPI.Services
{
    public interface IExerciseService
    {
        public static abstract Task<IResult> GetExerciseById(int id);

        public static abstract Task<IResult> CreateExercise(ExercicioModel ex);

        public static abstract Task<IResult> UpdateExercise(ExercicioModel ex);

        public static abstract Task<IResult> DeleteExerciseById(int id);
       

    }
}
