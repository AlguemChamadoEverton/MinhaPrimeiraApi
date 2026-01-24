using Microsoft.EntityFrameworkCore;
using MinhaPrimeiraAPI.Endpoints;
using MinhaPrimeiraAPI.Models;
using System.Security.Claims;

namespace MinhaPrimeiraAPI.Services.ExerciseService
{
    public class ExerciseService : IExerciseService
    {
        public static async Task<IResult> GetExercises(ClaimsPrincipal jwt)
        {
            var email = jwt.FindFirst(ClaimTypes.Email)?.Value;
            var exercisesTask = await EndpointsService.db.Exercises.Where(
                ex => ex.User.Email == email || ex.IsCustom == false).ToListAsync();
            var musclesTask = await EndpointsService.db.Muscles.ToListAsync();
            var exercisemusclesTask = await EndpointsService.db.ExerciseMuscles.Where(
                exm => exm.Exercise.User.Email == email || exm.Exercise.IsCustom == false).ToListAsync();

            var exIds = exercisesTask.Select(x => x.Id).ToList();

            var result = new ExerciseDTO
            {
                ExerciseName = exercisesTask.Select(ex => ex.Name).ToList(),
                MuscleName = musclesTask.Select(m => m.Name).ToList(),
                MainMuscle = exercisemusclesTask.Where(exm => exIds.Contains(exm.ExerciseId)).Select(x => x.Muscle.Name).ToList()
            };
            return Results.Ok(result);
        }
        public static async Task<IResult> GetExerciseById(int id, ClaimsPrincipal jwt)
        {
            var exercise = await EndpointsService.db.Exercises.FirstOrDefaultAsync(ex => ex.Id == id);
            if (exercise is not null)
            {
                exercise.User = await EndpointsService.db.Users.FirstAsync(x => x.Id == exercise.UserId);
                if (jwt.FindFirst(ClaimTypes.Email)?.Value ==
                    exercise.User.Email || exercise.IsCustom == false)
                {
                    var exercisemuscle = await EndpointsService.db.ExerciseMuscles.Where(em => em.ExerciseId == id).ToListAsync();
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
                return Results.Problem(statusCode: 404, extensions:
                new Dictionary<string, object?>
                {
                    { "Error: ", $"You can only see your own exercises"}
                });
            }
            return Results.Problem(statusCode: 404, extensions:
                new Dictionary<string, object?>
                {
                    { "Error: ", $"The requested Id {id} was not found"}
                }
            );
        }
        public static async Task<IResult> CreateExercise(ExerciseDTO ex, ClaimsPrincipal jwt)
        {
            ex.MuscleName.Add(ex.MainMuscle.First());
            var muscleobject = await EndpointsService.db.Muscles.Where(
                m => ex.MuscleName.Contains(m.Name)).ToListAsync();
            if (muscleobject.Count == ex.MuscleName.Count && muscleobject.Count > 0)
            {
                var email = jwt.FindFirst(ClaimTypes.Email)?.Value;
                var user = await EndpointsService.db.Users.FirstAsync(x => x.Email == email);
                Exercise result = new()
                {
                    Name = ex.ExerciseName.First(),
                    Muscles = muscleobject,
                    User = user,
                    UserId = user.Id
                };
                await EndpointsService.db.Exercises.AddAsync(result);
                var main = muscleobject.First(a => a.Name == ex.MainMuscle.First());
                await EndpointsService.db.SaveChangesAsync();
                EndpointsService.db.ExerciseMuscles.First(
                    b => b.ExerciseId == result.Id && b.MuscleId == main.Id).Type = true;
                await EndpointsService.db.SaveChangesAsync();
                return TypedResults.Created($"/exercise/{result.Id}", ex);
            }
            return Results.Problem(statusCode: 400, extensions: new Dictionary<string, object?>
            {
                { "Error: ", $"All muscles needs to be valid!"}
            });
        }
        public static async Task<IResult> DeleteExerciseById(int id, ClaimsPrincipal jwt)
        {
            var exercise = await EndpointsService.db.Exercises.FirstOrDefaultAsync(m => m.Id == id);
            if (exercise == null) return Results.Problem(statusCode: 404, extensions: new Dictionary<string, object?>
            {
                {"Error: ", $"The requested Id {id} was not found"}
            });
            exercise.User = await EndpointsService.db.Users.FirstAsync(f => exercise.UserId == f.Id);
            if (jwt.FindFirst(ClaimTypes.Email)?.Value ==
                exercise.User.Email)
            {
                EndpointsService.db.Exercises.Remove(exercise);
                await EndpointsService.db.SaveChangesAsync();
                return Results.NoContent();
            }
            return Results.Problem(statusCode: 404, extensions: new Dictionary<string, object?>
            {
                {"Error: ", $"You can only delete your own exercises"}
            });
        }
    }
}
