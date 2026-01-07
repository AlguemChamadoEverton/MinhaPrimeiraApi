using Microsoft.EntityFrameworkCore;
using MinhaPrimeiraAPI.DTOs;
using MinhaPrimeiraAPI.Endpoints;
using MinhaPrimeiraAPI.Models;

namespace MinhaPrimeiraAPI.Services
{
    public class ExerciseService : IExerciseService
    {
        public static async Task<IResult> GetExercises()
        {
            var exercisesTask = await EndpointsService.db.Exercises.ToListAsync();
            var musclesTask = await EndpointsService.db.Muscles.ToListAsync();

            var result = new ExerciseDTO
            {
                ExerciseName = exercisesTask.Select(ex => ex.Name).ToList(),
                MuscleName = musclesTask.Select(ex => ex.Name).ToList()
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
            var result = new ExerciseDTO()
            {
                ExerciseName = [exercise.Name],
                MuscleName = muscles.Select(e => e.Name).ToList()
            };
            return TypedResults.Ok(result);
        }
        public static async Task<IResult> CreateExercise(ExerciseDTO ex) 
        {
            string exercise = ex.ExerciseName.First();
            if (!await EndpointsService.db.Exercises.AnyAsync(b => b.Name.ToLower() == exercise.ToLower()))
            {
                var muscleobject = await EndpointsService.db.Muscles.Where(
                    m => ex.MuscleName.Contains(m.Name)).ToListAsync();
                if ((muscleobject.Count == ex.MuscleName.Count) && muscleobject.Count > 0)
                {
                    ExerciseModel result = new()
                    {
                        Name = exercise,
                        Muscles = muscleobject
                    };
                    await EndpointsService.db.Exercises.AddAsync(result);
                    await EndpointsService.db.SaveChangesAsync();
                    return TypedResults.Created($"/exercise/{result.Id}", ex);
                }
                return Results.Problem(statusCode: 400, extensions: new Dictionary<string, object?>
                {
                    { "Error: ", $"All muscles needs to be valid!"}
                });
            }
            return Results.Problem(statusCode: 400, extensions: new Dictionary<string, object?>
                {
                    { "Error: ", $"The requested name '{exercise}' is already registered"}
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
