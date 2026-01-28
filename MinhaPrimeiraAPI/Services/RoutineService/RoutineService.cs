using Microsoft.EntityFrameworkCore;
using MinhaPrimeiraAPI.Endpoints;
using MinhaPrimeiraAPI.Models;
using System.Security.Claims;
using MinhaPrimeiraAPI.Services.ExerciseService;

namespace MinhaPrimeiraAPI.Services.RoutineService
{
    public class RoutineService
    {
        public static async Task<IResult> GetCreateRoutine(ClaimsPrincipal jwt)
        {
            var email = jwt.FindFirst(ClaimTypes.Email)?.Value;
            var exercises = await EndpointsService.db.Exercises.Where(e => e.User.Email == email || !e.IsCustom).ToListAsync();
            var muscle = await EndpointsService.db.Muscles.ToListAsync();
            var query = (await EndpointsService.db.ExerciseMuscles.Where(e => e.Type == true &&
            exercises.Contains(e.Exercise)).ToListAsync()).Select(b => b.MuscleId).ToList();
            var mainmuscles = muscle.Where(m => query.Contains(m.Id));

            return TypedResults.Ok(new ExerciseDTO()
            {
                ExerciseName = exercises.Select(e => e.Name).ToList(),
                MuscleName = muscle.Select(m => m.Name).ToList(),
                MainMuscle = mainmuscles.Select(mm => mm.Name).ToList()
            });
        }
        public static async Task<IResult> CreateRoutine(ClaimsPrincipal jwt, RoutineDto routine) 
        {
            var email = jwt.FindFirst(ClaimTypes.Email)?.Value;
            var user = await EndpointsService.db.Users.FirstAsync(u =>  u.Email == email);
            var exercisesraw = await EndpointsService.db.Exercises.ToListAsync();
            var exerciseIds = routine.IdSet.Select(i => i.Item1).ToList();

            var result = new Routine
            {
                Name = routine.Name,
                User = user,
                Exercises = exercisesraw.Where(e => exerciseIds.Contains(e.Id)).ToList()
            };
            await EndpointsService.db.Routines.AddAsync(result);
            var exerciseRoutines = await EndpointsService.db.ExerciseRoutines.Where(e => e.RoutineId == result.Id).ToListAsync();
            foreach ((int, List<Set>) set in routine.IdSet)
            {
                exerciseRoutines.First(er => set.Item1 == er.ExerciseId).Sets = set.Item2;
            }
            EndpointsService.db.ExerciseRoutines.UpdateRange(exerciseRoutines);
            await EndpointsService.db.SaveChangesAsync();
            return Results.Created();
        }
        public static async Task<IResult> GetRoutine(ClaimsPrincipal jwt)
        {
            var email = jwt.FindFirst(ClaimTypes.Email)?.Value;
            var routines = await EndpointsService.db.Routines.Where(r => r.User.Email == email).ToListAsync();
            return TypedResults.Ok(routines);
        }
    }
}
