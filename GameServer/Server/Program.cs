using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MySqlConnector;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Server;
using Server.Controllers;
using Server.Models;
using Server.Services;

var builder = WebApplication.CreateBuilder(args);
var settings = new Settings();
builder.Configuration.Bind("Settings", settings);
builder.Services.AddSingleton(settings);
// Add services to the container.

builder.Services.AddDbContext<GameDbContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("Db")));
builder.Services.AddControllers().AddNewtonsoftJson(o  =>
{
   o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
}); 

builder.Services.AddScoped<IHeroService, HeroService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o =>
{
   o.TokenValidationParameters = new TokenValidationParameters()
   {
      IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(settings.BearerKey)),
      ValidateIssuerSigningKey = true,
      ValidateAudience = false,
      ValidateIssuer = false,
   };
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
   
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();