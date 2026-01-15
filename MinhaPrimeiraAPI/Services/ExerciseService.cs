using Microsoft.EntityFrameworkCore;
using MinhaPrimeiraAPI.DTOs;
using MinhaPrimeiraAPI.Endpoints;
using MinhaPrimeiraAPI.Models;
using System.Security.Claims;

namespace MinhaPrimeiraAPI.Services
{
    public class ExerciseService : IExerciseService
    {
        public static async Task<IResult> GetExercises()
        {
            var exercisesTask = await EndpointsService.db.Exercises.ToListAsync();
            var musclesTask = await EndpointsService.db.Muscles.ToListAsync();
            var exercisemusclesTask = await EndpointsService.db.ExerciseMuscles.ToListAsync();
            
            var exIds = exercisesTask.Select(x => x.Id).ToList();

            var result = new ExerciseDTO
            {
                ExerciseName = exercisesTask.Select(ex => ex.Name).ToList(),
                MuscleName = musclesTask.Select(m => m.Name).ToList(),
                MainMuscle = exercisemusclesTask.Where(exm => exIds.Contains(exm.ExerciseId)).Select(x => x.Muscle.Name).ToList()
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
            var mainmuscle = exercisemuscle.First(a => a.ExerciseId == id && a.Type == true);
            var muscles = await EndpointsService.db.Muscles
            .Where(m => idsToSearch.Contains(m.Id)).ToListAsync();
            var result = new ExerciseDTO()
            {
                ExerciseName = [exercise.Name],
                MuscleName = muscles.Select(e => e.Name).ToList(),
                MainMuscle = [muscles.First(b => b.Id == mainmuscle.MuscleId).Name]
            };
            return TypedResults.Ok(result);
        }
        public static async Task<IResult> CreateExercise(ExerciseDTO ex, ClaimsPrincipal jwt) 
        {
            string exercise = ex.ExerciseName.First();
            if (!await EndpointsService.db.Exercises.AnyAsync(b => b.Name.ToLower() == exercise.ToLower()))
            {
                ex.MuscleName.Add(ex.MainMuscle.First());
                var muscleobject = (await EndpointsService.db.Muscles.Where(
                    m => ex.MuscleName.Contains(m.Name)).ToListAsync());
                if ((muscleobject.Count == ex.MuscleName.Count) && muscleobject.Count > 0)
                {
                    var email = jwt.FindFirst(ClaimTypes.Email)?.Value;
                    var user = await EndpointsService.db.Users.FirstAsync(x => x.Email == email);
                    Exercise result = new()
                    {
                        Name = exercise,
                        Muscles = muscleobject,
                        User = user,
                        UserId = user.Id
                    };
                    await EndpointsService.db.Exercises.AddAsync(result);
                    var main = muscleobject.First(a => a.Name == ex.MainMuscle.First());
                    await EndpointsService.db.SaveChangesAsync();
                    EndpointsService.db.ExerciseMuscles.First(
                        b => (b.ExerciseId == result.Id) && (b.MuscleId == main.Id)).Type = true;
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
