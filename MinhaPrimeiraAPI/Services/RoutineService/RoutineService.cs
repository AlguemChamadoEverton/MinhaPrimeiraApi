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
            var user = await EndpointsService.db.Users.FirstAsync(u => u.Email == email);
            var exercises = await EndpointsService.db.ExerciseMuscles.Include(em => em.Exercise).
                Include(em => em.Muscle)
                .Where(em => em.Type == true && (em.Exercise.User.Email.Equals(user.Email) || em.Exercise.User.Email.Equals("admin"))).ToListAsync();
            var exerciseMainMuscle = new List<ExerciseMainMuscle>();
            
            foreach (ExerciseMuscle exercise in exercises)
            {
                exerciseMainMuscle.Add(new ExerciseMainMuscle()
                {
                    ExerciseName = exercise.Exercise.Name,
                    MainMuscle = exercise.Muscle.Name
                });
            }
            return TypedResults.Ok(new ExerciseDto()
            {
                MuscleName = await EndpointsService.db.Muscles.Select(m => m.Name).ToListAsync(),
                ExercisesMainMuscle = exerciseMainMuscle
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
            List<Set> sets = new List<Set>();
            foreach (ExerciseRoutineDto er in routine.ErDtos)
            {
                for(int i = 0; i < er.Sets.Count; i++)
                {
                    sets.Add(new Set()
                    {   
                        Reps = er.Sets[i].Reps,
                        Weight = er.Sets[i].Weight
                    });
                }
                exerciseRoutines.First(e => e.ExerciseId == er.ExerciseId).Sets = sets;
            }
            EndpointsService.db.ExerciseRoutines.UpdateRange(exerciseRoutines);
            await EndpointsService.db.SaveChangesAsync();
            return Results.Created();
        }
        public static async Task<IResult> GetRoutine(ClaimsPrincipal jwt)
        {
            var email = jwt.FindFirst(ClaimTypes.Email)?.Value;
            var routines = await EndpointsService.db.Routines.Include(r => r.Exercises).Where(r => r.User.Email == email).ToListAsync();
            var result = new List<RoutineDto>();
            foreach (Routine routine in routines)
            {
                List<ExerciseMuscleDto> listEx = new List<ExerciseMuscleDto>();
                foreach (Exercise ex in routine.Exercises)
                {
                    listEx.Add(new ExerciseMuscleDto()
                    {
                        Name = ex.Name
                    });
                }
                result.Add(new RoutineDto()
                {
                    Name = routine.Name,
                    Exercises = listEx
                });
            }
            return TypedResults.Ok(result);
        }

        public static async Task<IResult> GetEditRoutine(ClaimsPrincipal jwt, int id)
        {
            var email = jwt.FindFirst(ClaimTypes.Email)?.Value;
            var user = await EndpointsService.db.Users.FirstAsync(u =>  u.Email == email);
            var routine = await EndpointsService.db.Routines.FirstOrDefaultAsync(r => r.Id == id && r.User.Email == user.Email);
            if (routine is not null)
            {
                var exercisesroutine = await EndpointsService.db.ExerciseRoutines.Include(er => er.Sets).Where(er => er.RoutineId == routine.Id).ToListAsync();
                var exercisemuscles = await EndpointsService.db.ExerciseMuscles.Include(em => em.Exercise).Include(em => em.Muscle)
                    .Where(e => (e.Exercise.User.Email.Equals(user.Email) || e.Exercise.User.Id.Equals(0)) && e.Type).ToListAsync();
                
                List<ExerciseRoutineDto> dto = new List<ExerciseRoutineDto>();
                List<SetsDto> sets = new List<SetsDto>();
                for (int i = 0; i < exercisesroutine.Count; i++)
                {
                    for (int s = 0; s < exercisesroutine[i].Sets.Count; s++)
                    {
                        sets.Add(new SetsDto()
                        {
                            Id = exercisesroutine[i].Sets[s].Id,
                            Reps = exercisesroutine[i].Sets[s].Reps,
                            Weight = exercisesroutine[i].Sets[s].Weight
                            
                        });
                    }
                    dto.Add(new ExerciseRoutineDto()
                    {
                        ExerciseId = exercisesroutine[i].ExerciseId,
                        Sets = sets
                    });
                }

                List<ExerciseMuscleDto> exercises = new List<ExerciseMuscleDto>();
                for (int i = 0; i < exercisemuscles.Count; i++)
                {
                    exercises.Add(new ExerciseMuscleDto()
                    {
                        Id = exercisemuscles[i].Id,
                        Name = exercisemuscles[i].Exercise.Name,
                        Muscle = exercisemuscles[i].Muscle.Name
                    });
                }
                var result = new RoutineDto()
                {
                    Name = routine.Name,
                    ErDtos = dto,
                    Exercises = exercises,
                    Muscles = await EndpointsService.db.Muscles.Select(m => m.Name).ToListAsync()
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
                List<Set> sets = new List<Set>();
                foreach (ExerciseRoutineDto er in routine.ErDtos)
                {
                    for (int i = 0; i < er.Sets.Count; i++)
                    {
                        sets.Add(new Set()
                        {
                            Reps = er.Sets[i].Reps,
                            Weight = er.Sets[i].Weight
                        });
                    }

                    exerciseRoutines.First(e => e.ExerciseId == er.ExerciseId).Sets = sets;
                    EndpointsService.db.ExerciseRoutines.UpdateRange(exerciseRoutines);
                    await EndpointsService.db.SaveChangesAsync();
                    return Results.Ok();
                }
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
