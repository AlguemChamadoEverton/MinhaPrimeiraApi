using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using MinhaPrimeiraAPI.Data;
using MinhaPrimeiraAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using MinimalApis.Extensions;
using MinhaPrimeiraAPI.Services.AuthService;
using MinhaPrimeiraAPI.Services.ExerciseService;
using MinhaPrimeiraAPI.Services.RoutineService;

namespace MinhaPrimeiraAPI.Endpoints
{
    public class EndpointsService(IConfiguration configuration)
    {
        public static AppDbContext db = new AppDbContext();
        public AuthService auth = new AuthService(configuration);
        public void Endpoints(WebApplication app) 
        {
            app.MapGet("/", () =>
            {

            });

            app.MapPost("/", () =>
            {
                return Results.Ok(); // Implementar a tela inicial posteriormente
            }).WithParameterValidation().RequireAuthorization();

            app.MapPost("/register", (User request) =>
            {
                return auth.RegisterAsync(request);
            }).WithParameterValidation();

            app.MapPost("/login", (User request) =>
            {
                return auth.LoginAsync(request);
            });

            app.MapGet("/exercise", (ClaimsPrincipal jwt) => 
            {
                return ExerciseService.GetExercises(jwt);
            }).WithParameterValidation().RequireAuthorization();

            app.MapGet("/exercise/{id}", (int id, ClaimsPrincipal jwt) =>
            {
                return ExerciseService.GetExerciseById(id, jwt);
            }).WithParameterValidation().RequireAuthorization();

            app.MapPost("/exercise", (ExerciseDTO model, ClaimsPrincipal jwt) =>
            {
                return ExerciseService.CreateExercise(model, jwt);
            }).RequireAuthorization().WithParameterValidation();

            app.MapDelete("/exercise/{id}", (int id, ClaimsPrincipal jwt) =>
            {
                return ExerciseService.DeleteExerciseById(id, jwt);
            }).WithParameterValidation().RequireAuthorization();

            app.MapGet("/routine/", (ClaimsPrincipal jwt) =>
            {
                return RoutineService.GetRoutine(jwt);
            }).RequireAuthorization().WithParameterValidation();

            app.MapGet("/create-routine", (ClaimsPrincipal jwt) =>
            {
                return RoutineService.GetCreateRoutine(jwt);


            }).RequireAuthorization().WithParameterValidation();

            app.MapPost("/create-routine", (ClaimsPrincipal jwt, RoutineDto routine) =>
            {
                return RoutineService.CreateRoutine(jwt, routine);


            }).RequireAuthorization().WithParameterValidation();
            app.MapGet("/edit-routine/{id}", (int id, ClaimsPrincipal jwt) =>
            {
                return RoutineService.GetEditRoutine(jwt, id);
            }).RequireAuthorization().WithParameterValidation();
            app.MapPut("/edit-routine/{id}", (ClaimsPrincipal jwt,int id, RoutineDto routine) =>
            {
                return RoutineService.EditRoutine(jwt, id, routine);
            }).RequireAuthorization().WithParameterValidation();

        }
    }
}
