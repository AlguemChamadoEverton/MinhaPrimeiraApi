using Microsoft.EntityFrameworkCore;
using MinhaPrimeiraAPI.Endpoints;
using MinhaPrimeiraAPI.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer();
builder.Services.AddAuthorizationBuilder();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
EndpointsRepository Route = new (app.Configuration);
Route.Endpoints(app);

app.UseHttpsRedirection();


app.Run();
