using MinhaPrimeiraAPI.Endpoints;
using MinhaPrimeiraAPI.Models;

namespace MinhaPrimeiraAPI.Services
{
    public interface IExerciseService
    {
        public static abstract Task<IResult> GetExerciseById(int id);

        public static abstract Task<IResult> CreateExercise(ExerciseModel ex);

        public static abstract Task<IResult> UpdateExercise(ExerciseModel ex);

        public static abstract Task<IResult> DeleteExerciseById(int id);
       

    }
}
