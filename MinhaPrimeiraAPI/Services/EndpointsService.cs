using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using MinhaPrimeiraAPI.Data;
using MinhaPrimeiraAPI.Models;
using MinhaPrimeiraAPI.Services;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;

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
            app.MapPost("/", (UserModel request) =>
            {
                return auth.RegisterAsync(request);
            });
            app.MapPost("/login", (UserModel request) =>
            {
                return auth.LoginAsync(request);
            });
            app.MapGet("/exercice", () => 
            {
                return ExerciseService.GetExercises();
            });
            app.MapGet("/exercice/{id}", (int id) =>
            {
                return ExerciseService.GetExerciseById(id);
            });
            app.MapPost("/exercice", (ExerciseModel model) =>
            {
                return ExerciseService.CreateExercise(model);
            });
            app.MapPut("/exercice", (ExerciseModel model) =>
            {
                return ExerciseService.UpdateExercise(model);
            });
            app.MapDelete("/exercice/{id}", (int id) =>
            {
                return ExerciseService.DeleteExerciseById(id);
            });
            app.MapGet("/routines", () => //para continuar precisarei de authenticação do usuário para que possa indicar de onde virá o treino, preciso criar uma tela de login e criar usuário e dar um jeito de usar uma key ou algo do gênero
            {
                return db.Routines;
            }).RequireAuthorization();
        }
    }
}
