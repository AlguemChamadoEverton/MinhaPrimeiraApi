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
                var teste = db.Exercicios;
                return Results.Ok(teste);
            });
            app.MapGet("/exercice/{ex}", (int ex) =>
            {
                var exercice = db.Exercicios.FirstOrDefault(b => b.Id == ex);
                if (exercice is null) 
                return Results.Problem(statusCode: 404, extensions:
                    new Dictionary<string, object?>
                    {
                        { "Error: ", $"The requested Id {ex} was not found"}
                    }
                );
                return TypedResults.Ok(exercice);
            });
            app.MapPost("/exercice", (ExercicioModel model) =>
            {
                var exercice = db.Exercicios.FirstOrDefault(b => b.Name == model.Name);
                if (exercice is null)
                {
                    db.Exercicios.Add(model);
                    db.SaveChanges(); // tem um bug aqui que quando o usuário envia um id que já existe e passou nas outras aprovações ele trava tudo
                    return TypedResults.Created($"/exercice/{model.Id}", model);
                }
                return Results.Problem(statusCode: 400, extensions: new Dictionary<string, object?>
                {
                    { "Error: ", $"The requested name '{model.Name}' is already registered"}
                });
            });
            app.MapPut("/exercice", (ExercicioModel model) =>
            {
                if (model.Id == 0) return Results.Problem(statusCode: 400, extensions: new Dictionary<string, object?>
                {
                    { "Error: ", $"Please, specify the id"}
                });
                var exercice = db.Exercicios.FirstOrDefault(b => b.Id == model.Id);
                if (exercice is not null)
                {
                    exercice.Name = model.Name;
                    exercice.TargetMuscle = model.TargetMuscle;
                    db.Exercicios.Update(exercice);
                    db.SaveChanges();
                    return TypedResults.Created($"/exercice/{model.Id}", model);
                }
                return Results.Problem(statusCode: 404, extensions: new Dictionary<string, object?>
                {
                    { "Error: ", $"The requested id '{model.Id}' was not found"}
                });
            });
            app.MapDelete("/exercice/{ex}", (int ex) =>
            {
                var result = db.Exercicios.FirstOrDefault(exercice => exercice.Id == ex);
                if (result is not null) 
                {
                    db.Exercicios.Remove(result);
                    db.SaveChanges();
                    return Results.NoContent();
                }
                return Results.Problem(statusCode: 404, extensions: new Dictionary<string, object?> 
                {
                    {"Error: ", $"The requested Id {ex} was not found"}
                });
            });
            app.MapGet("/routines", () => //para continuar precisarei de authenticação do usuário para que possa indicar de onde virá o treino, preciso criar uma tela de login e criar usuário e dar um jeito de usar uma key ou algo do gênero
            {
                return db.Routines;
            }).RequireAuthorization();
        }
    }
}
