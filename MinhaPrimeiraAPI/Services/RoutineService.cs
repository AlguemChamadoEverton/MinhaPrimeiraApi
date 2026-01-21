using Azure.Core;
using MinhaPrimeiraAPI.Endpoints;
using MinhaPrimeiraAPI.Models;
using System.Data.Entity;
using System.Security.Claims;

namespace MinhaPrimeiraAPI.Services
{
    public class RoutineService
    {
        public static async Task<IResult> CreateRoutine(ClaimsPrincipal jwt, Routine routine) 
        {
            var email = jwt.FindFirst(ClaimTypes.Email)?.Value;
            routine.UserId = (await EndpointsService.db.Users.FirstAsync(x => x.Email == email)).Id ;
            await EndpointsService.db.Routines.AddAsync(routine);
            await EndpointsService.db.SaveChangesAsync();
            return Results.Created();
            //Preciso dar um jeito de criar uma rotina, ainda estava raciocinando como faria isso.

        }
        public static async Task<IResult> GetRoutine(ClaimsPrincipal jwt)
        {
            var email = jwt.FindFirst(ClaimTypes.Email)?.Value;
            var routines = await EndpointsService.db.Routines.Where(r => r.User.Email == email).ToListAsync();
            return TypedResults.Ok(routines);
        }
    }
}
