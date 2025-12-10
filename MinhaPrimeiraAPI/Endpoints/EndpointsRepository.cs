using MinhaPrimeiraAPI.Data;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using MinhaPrimeiraAPI.Models;
using System.Runtime.CompilerServices;
using System.Data.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using MinhaPrimeiraAPI.Entities;
using Microsoft.Identity.Client;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace MinhaPrimeiraAPI.Endpoints
{
    public class EndpointsRepository(IConfiguration configuration)
    {
        public static AppDbContext db = new AppDbContext();

        public static User user = new();
        public void Endpoints(WebApplication app) 
        {
            app.MapGet("/", () =>
            {

            });
            app.MapPost("/", (UserModel request) =>
            {
                var hashedPassword = new PasswordHasher<User>().HashPassword(user, request.Password);

                user.Email = request.Email;
                user.PasswordHash = hashedPassword;

                return Results.Ok(user);
            });
            app.MapPost("/login", (UserModel request) =>
            {
                if (user.Email != request.Email) 
                {
                    return Results.Problem(statusCode: 400, extensions: new Dictionary<string, object?>
                    {
                        { "Error: ", $"The requested email '{request.Email}' do not exists"}
                    });
                }
                if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.Password) == PasswordVerificationResult.Failed) 
                {
                    return Results.Problem(statusCode: 400, extensions: new Dictionary<string, object?>
                    {
                        { "Error: ", $"The password is wrong."}
                    });
                }
                string token = CreateToken(user);
                return Results.Ok(token);
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
                return db.Routines
            }).RequireAuthorization();
        }
        private string CreateToken(User user) 
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:token")!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: configuration.GetValue<string>("AppSetting:Issuer"),
                audience: configuration.GetValue<string>("AppSetting:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}
