using MinhaPrimeiraAPI.Endpoints;
using MinhaPrimeiraAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MinhaPrimeiraAPI.Services
{
    public class ExerciseService : IExerciseService
    {
        public static async Task<IResult> GetExercises()
        {
            var teste = await EndpointsService.db.Exercises.ToListAsync();
            return Results.Ok(teste);
        }
        public static async Task<IResult> GetExerciseById(int id) 
        {
            var exercice = await EndpointsService.db.Exercises.FirstOrDefaultAsync(b => b.Id == id);
            if (exercice is null)
                return Results.Problem(statusCode: 404, extensions:
                    new Dictionary<string, object?>
                    {
                        { "Error: ", $"The requested Id {id} was not found"}
                    }
                );
            return TypedResults.Ok(exercice);
        }
        public static async Task<IResult> CreateExercise(ExerciseModel ex) 
        {
            var exercice = await EndpointsService.db.Exercises.FirstOrDefaultAsync(b => b.Name == ex.Name);
            if (exercice is null)
            {
                await EndpointsService.db.Exercises.AddAsync(ex);
                try { EndpointsService.db.SaveChanges(); }
                catch(Exception)
                {
                    return Results.Problem(statusCode: 400, extensions: new Dictionary<string, object?>
                {
                    { "Error: ", $"Please do not insert the id"}
                });
                }//tem um bug aqui que quando o usuário envia um id que já existe e passou nas outras aprovações ele trava tudo
                return TypedResults.Created($"/exercice/{ex.Id}", ex);
            }
            return Results.Problem(statusCode: 400, extensions: new Dictionary<string, object?>
                {
                    { "Error: ", $"The requested name '{ex.Name}' is already registered"}
                });
        }
        public static async Task<IResult> UpdateExercise(ExerciseModel ex)
        {
            if (ex.Id == 0) return Results.Problem(statusCode: 400, extensions: new Dictionary<string, object?>
                {
                    { "Error: ", $"Please, specify the id"}
                });
            var exercice = await EndpointsService.db.Exercises.FirstOrDefaultAsync(b => b.Id == ex.Id);
            if (exercice is not null)
            {
                exercice.Name = ex.Name;
                exercice.Muscles = ex.Muscles;
                EndpointsService.db.Exercises.Update(exercice);
                EndpointsService.db.SaveChanges();
                return TypedResults.Created($"/exercice/{ex.Id}", ex);
            }
            return Results.Problem(statusCode: 404, extensions: new Dictionary<string, object?>
                {
                    { "Error: ", $"The requested id '{ex.Id}' was not found"}
                });
        }
        public static async Task<IResult> DeleteExerciseById(int id)
        {
            var result = await EndpointsService.db.Exercises.FirstOrDefaultAsync(exercice => exercice.Id == id);
            if (result is not null)
            {
                EndpointsService.db.Exercises.Remove(result);
                await EndpointsService.db.SaveChangesAsync();
                return Results.NoContent();
            }
            return Results.Problem(statusCode: 404, extensions: new Dictionary<string, object?>
                {
                    {"Error: ", $"The requested Id {id} was not found"}
                });
        }

    }
}
