using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MinhaPrimeiraAPI.Data;
using MinhaPrimeiraAPI.Models;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;

namespace MinhaPrimeiraAPI.Services
{
    public class AuthService : IAuthService
    {
        private static IConfiguration? _configuration;
        public AuthService(IConfiguration configuration) 
        {
            _configuration = configuration;
        }
        public static AppDbContext db = new AppDbContext();
        public async Task<IResult> RegisterAsync(UserModel request)
        {
            var hashedPassword = new PasswordHasher<UserModel>().HashPassword(request, request.PasswordHash);

            await db.AddAsync<UserModel>(new UserModel
            {
                Email = request.Email,
                PasswordHash = hashedPassword
            });
            await db.SaveChangesAsync();

            return Results.Ok();
        }
        public async Task<IResult> LoginAsync(UserModel request)
        {
            var user = await db.Users.Where
            if (user == null)
            {
                return Results.Problem(statusCode: 400, extensions: new Dictionary<string, object?>
                {
                    { "Error: ", $"The requested email '{request.Email}' do not exists"}
                });
            }
            if (new PasswordHasher<UserModel>().VerifyHashedPassword(user, user.PasswordHash, request.PasswordHash) == PasswordVerificationResult.Failed)
            {
                return Results.Problem(statusCode: 400, extensions: new Dictionary<string, object?>
                {
                    { "Error: ", $"The password is wrong."}
                });
            }
            string token = CreateToken(user);
            return Results.Ok(token);
        }
        private static string CreateToken(UserModel user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration.GetValue<string>("AppSettings:token")!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: _configuration.GetValue<string>("AppSetting:Issuer"),
                audience: _configuration.GetValue<string>("AppSetting:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}
