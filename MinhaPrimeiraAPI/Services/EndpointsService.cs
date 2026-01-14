using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using MinhaPrimeiraAPI.Data;
using MinhaPrimeiraAPI.DTOs;
using MinhaPrimeiraAPI.Models;
using MinhaPrimeiraAPI.Services;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using MinimalApis.Extensions;

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
            app.MapPost("/", (User request) =>
            {
                return auth.RegisterAsync(request);
            }).WithParameterValidation();
            app.MapPost("/login", (User request) =>
            {
                return auth.LoginAsync(request);
            }).WithParameterValidation();
            app.MapGet("/exercise", () => 
            {
                return ExerciseService.GetExercises();
            }).WithParameterValidation();
            app.MapGet("/exercise/{id}", (int id) =>
            {
                return ExerciseService.GetExerciseById(id);
            }).WithParameterValidation();
            app.MapPost("/exercise", (ExerciseDTO model, ClaimsPrincipal jwt) =>
            {
                return ExerciseService.CreateExercise(model, jwt);
            }).RequireAuthorization().WithParameterValidation();
            app.MapDelete("/exercise/{id}", (int id) =>
            {
                return ExerciseService.DeleteExerciseById(id);
            }).WithParameterValidation();
            app.MapGet("/routines", (ClaimsPrincipal jwt) =>
            {
                return RoutineService.GetRoutine(jwt);
                
            }).RequireAuthorization().WithParameterValidation();
            app.MapPost("/routines", (ClaimsPrincipal jwt, Routine routine) =>
            {
                return RoutineService.CreateRoutine(jwt, routine);


            }).RequireAuthorization().WithParameterValidation();
        }
    }
}
