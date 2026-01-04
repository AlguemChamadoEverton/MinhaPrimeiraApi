using MinhaPrimeiraAPI.Endpoints;
using MinhaPrimeiraAPI.Models;
using Microsoft.EntityFrameworkCore;
using MinhaPrimeiraAPI.DTOs;

namespace MinhaPrimeiraAPI.Services
{
    public class ExerciseService : IExerciseService
    {
        public static async Task<IResult> GetExercises()
        {
            var exercisesTask = await EndpointsService.db.Exercises.ToListAsync();
            var musclesTask = await EndpointsService.db.Muscles.ToListAsync();

            var result = new ExerciseMuscleDTO
            {
                Exercises = exercisesTask,
                Muscles = musclesTask
            };

            return Results.Ok(result);
        }
        public static async Task<IResult> GetExerciseById(int id)
        {
            var exercisemuscle = await EndpointsService.db.ExerciseMuscles.Where(em => em.ExerciseId == id).ToListAsync();
            if (exercisemuscle.Count == 0)
            {
                return Results.Problem(statusCode: 404, extensions:
                    new Dictionary<string, object?>
                    {
                        { "Error: ", $"The requested Id {id} was not found"}
                    }
                );
            }
            var exercise = await EndpointsService.db.Exercises.FirstOrDefaultAsync(ex => ex.Id == id);
            var idsToSearch = exercisemuscle.Select(e => e.MuscleId).ToList();
            var muscles = await EndpointsService.db.Muscles
            .Where(m => idsToSearch.Contains(m.Id)).ToListAsync();
            var result = new ExerciseByIdDTO()
            {
                ExerciseName = exercise.Name,
                MuscleName = muscles.Select(e => e.Name).ToList()
            };
            return TypedResults.Ok(result);
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
                }
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
                exercice.ExerciseMuscles = ex.ExerciseMuscles;
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
