using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using MinhaPrimeiraAPI.Data;
using MinhaPrimeiraAPI.Models;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using MinhaPrimeiraAPI.Endpoints;

namespace MinhaPrimeiraAPI.Services
{
    public class AuthService : IAuthService
    {
        private static IConfiguration? _configuration;
        public AuthService(IConfiguration configuration) 
        {
            _configuration = configuration;
        }
        public async Task<IResult> RegisterAsync(User request)
        {
            var user = await EndpointsService.db.Users.FirstOrDefaultAsync(b => b.Email == request.Email || b.Username == request.Username);
            if(user == null)
            {
                request.PasswordHash = new PasswordHasher<User>().HashPassword(request, request.PasswordHash);
                await EndpointsService.db.Users.AddAsync(request);
                await EndpointsService.db.SaveChangesAsync();

                return Results.Created();
            }
            else if (user.Email == request.Email)
                return Results.Problem(statusCode: 400, extensions: new Dictionary<string, object?>
                {
                    { "Error: ", $"The requested email '{request.Email}' already exists"}
                });
            else
                return Results.Problem(statusCode: 400, extensions: new Dictionary<string, object?>
                {
                    { "Error: ", $"The requested username '{request.Username}' already exists"}
                });
        }
        public async Task<IResult> LoginAsync(User request)
        {
            var user = await EndpointsService.db.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
            {
                return Results.Problem(statusCode: 400, extensions: new Dictionary<string, object?>
                {
                    { "Error: ", $"The requested email '{request.Email}' do not exists"}
                });
            }
            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.PasswordHash) == PasswordVerificationResult.Failed)
            {
                return Results.Problem(statusCode: 400, extensions: new Dictionary<string, object?>
                {
                    { "Error: ", $"The password is wrong."}
                });
            }
            string token = CreateToken(user);
            return Results.Ok(token);
        }
        private static string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration.GetValue<string>("AppSettings:token")!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: _configuration.GetValue<string>("AppSettings:Issuer"),
                audience: _configuration.GetValue<string>("AppSettings:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}
