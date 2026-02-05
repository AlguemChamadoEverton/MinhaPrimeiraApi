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
            var exerciseIds = routine.ErDtos.Select(i => i.ExerciseId).ToList();

            var result = new Routine
            {
                Name = routine.Name,
                User = user,
                Exercises = exercisesraw.Where(e => exerciseIds.Contains(e.Id)).ToList()
            };
            await EndpointsService.db.Routines.AddAsync(result);
            await EndpointsService.db.SaveChangesAsync();
            var exerciseRoutines = await EndpointsService.db.ExerciseRoutines.Where(e => e.RoutineId == result.Id).ToListAsync();
            foreach (ExerciseRoutineDto er in routine.ErDtos)
            {
                exerciseRoutines.First(e => e.ExerciseId == er.ExerciseId).Sets = er.Sets;
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

        public static async Task<IResult> GetEditRoutine(ClaimsPrincipal jwt, int id)
        {
            var email = jwt.FindFirst(ClaimTypes.Email)?.Value;
            var user = await EndpointsService.db.Users.FirstAsync(u =>  u.Email == email);
            var routine = await EndpointsService.db.Routines.FirstOrDefaultAsync(r => r.Id == id && r.User.Email == user.Email);
            if (routine is not null)
            {
                var exercises = await EndpointsService.db.ExerciseRoutines.Where(er => er.RoutineId == routine.Id).Select(em => em.ExerciseId).ToListAsync();
                var sets = await EndpointsService.db.ExerciseRoutines.Where(er => er.RoutineId == routine.Id).Select(er => er.Sets).ToListAsync();
                
                var dto = new List<ExerciseRoutineDto>();
                for (int i = 0; i < exercises.Count; i++)
                {
                    dto.Add(new ExerciseRoutineDto()
                    {
                        ExerciseId = exercises[i],
                        Sets = sets[i]
                    });
                }
                var result = new RoutineDto()
                {
                    Name = routine.Name,
                    ErDtos = dto,
                    Exercises = await EndpointsService.db.Exercises.Where(e=> e.User.Email.Equals(user.Email) || e.User.Id.Equals(0)).ToListAsync(),
                    Muscles = await EndpointsService.db.Muscles.ToListAsync()
                };
                return TypedResults.Ok(result);
            }
            return Results.Problem(statusCode: 404, extensions:
                new Dictionary<string, object?>
                {
                    { "Error: ", $"Routine not found"}
                });
        }
        public static async Task<IResult> EditRoutine(ClaimsPrincipal jwt, int id, RoutineDto routine)
        {
            var email = jwt.FindFirst(ClaimTypes.Email)?.Value;
            var user = await EndpointsService.db.Users.FirstAsync(u =>  u.Email == email);
            var check = await EndpointsService.db.Routines.FirstOrDefaultAsync(r => r.Id == id && r.User.Email.Equals(user.Email));
            if (check is not null)
            {
                var exercisesraw = await EndpointsService.db.Exercises.ToListAsync();
                var exerciseIds = routine.ErDtos.Select(i => i.ExerciseId).ToList();
                check.Name = routine.Name;
                check.User = user;
                check.Exercises = exercisesraw.Where(e => exerciseIds.Contains(e.Id)).ToList();
                await EndpointsService.db.SaveChangesAsync();
                var exerciseRoutines = await EndpointsService.db.ExerciseRoutines.Where(e => e.RoutineId == check.Id).ToListAsync();
                foreach (ExerciseRoutineDto er in routine.ErDtos)
                {
                    exerciseRoutines.First(e => e.ExerciseId == er.ExerciseId).Sets = er.Sets;
                }
                EndpointsService.db.ExerciseRoutines.UpdateRange(exerciseRoutines);
                await EndpointsService.db.SaveChangesAsync();
                return Results.Ok();
            }
            return Results.Problem(statusCode: 404, extensions:
                new Dictionary<string, object?>
                {
                    { "Error: ", $"Routine not found"}
                });
            
        }

        public static async Task<IResult> DeleteRoutine(ClaimsPrincipal jwt, int id)
        {
            var email = jwt.FindFirst(ClaimTypes.Email)?.Value;
            var user = await EndpointsService.db.Users.FirstAsync(u =>  u.Email == email);
            var check = await EndpointsService.db.Routines.FirstOrDefaultAsync(r => r.Id == id && r.User.Email.Equals(user.Email));
            if (check is not null)
            {
                EndpointsService.db.Routines.Remove(check);
                await EndpointsService.db.SaveChangesAsync();
                return Results.NoContent();
            }
            return Results.Problem(statusCode: 404, extensions:
                new Dictionary<string, object?>
                {
                    { "Error: ", $"Routine not found"}
                });
        }
    }
    
}
