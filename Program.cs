using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

                    /* Builder */
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthentication(o =>
{
    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey
            (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});

builder.Services.AddAuthorization();

builder.Services.AddDbContext<SmarthomeContext>(options =>
    options.UseMySQL(builder.Configuration["ConnectionStrings:MySql"]));

var securityScheme = new OpenApiSecurityScheme()
{
    Name = "Authorization",
    Type = SecuritySchemeType.ApiKey,
    Scheme = "Bearer",
    BearerFormat = "JWT",
    In = ParameterLocation.Header,
    Description = "JSON Web Token based security",
};

var securityReq = new OpenApiSecurityRequirement()
{
    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        },
        new string[] {}
    }
};
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Mi SmartHome API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", securityScheme);
    c.AddSecurityRequirement(securityReq);
});

                /*  APP */
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", [AllowAnonymous] () => "Bienevenido a la API");

app.MapPost("/login", [AllowAnonymous] async (User user, SmarthomeContext db) =>
{
    var userdb = await db.Users.FindAsync(user.Username);
    if(userdb is null) return Results.NotFound(user.Username);
    if (userdb.Password != user.Password) return Results.Unauthorized();
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]));
    var jwtTokenHandler = new JwtSecurityTokenHandler();
    var descriptor = new SecurityTokenDescriptor()
    {
        SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256),
        Expires = DateTime.UtcNow.AddHours(1)
    };
    var token = jwtTokenHandler.CreateToken(descriptor);
    var jwtToken = jwtTokenHandler.WriteToken(token);
    return Results.Ok(jwtToken);
});

/* CRUD */
app.MapGet("/sensores", [Authorize] async (SmarthomeContext db) =>
{
    return await db.Sensors.ToListAsync();
});

app.MapGet("/sensores/{id}", [Authorize] async (int id, SmarthomeContext db) =>
{
    var sensor = await db.Sensors.FindAsync(id);
    if (sensor is null)
    {
        return Results.NotFound();
    }
    return Results.Ok(sensor);
});

app.MapPost("/sensores", [Authorize] async (Sensor s, SmarthomeContext db) =>
{
    s.Date = DateTime.Now;
    db.Sensors.Add(s);
    await db.SaveChangesAsync();
    return Results.Created($"/sensores/{s.Id}", s);
});

app.MapPut("/sensores/{id}", [Authorize] async (int id, Sensor s, SmarthomeContext db) =>
{
    var sensor = await db.Sensors.FindAsync(id);
    if (sensor is null)
    {
        return Results.NotFound();
    }
    sensor.Name = s.Name;
    sensor.Value = s.Value;
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/sensores/{id}", [Authorize] async (int id, SmarthomeContext db) =>
{
    var sensor = await db.Sensors.FindAsync(id);
    if (sensor is null)
    {
        return Results.NotFound();
    }
    db.Sensors.Remove(sensor);
    await db.SaveChangesAsync();
    return Results.NoContent();
});
/*****************************************************************************************/

app.Run();

class User
{
    [Key]
    public string? Username { get; set; }
    public string? Password { get; set; }
}
class Sensor
{
    [Key]
    public int Id { get; set; }
    public string? Name { get; set; }
    public double Value { get; set; }
    public DateTime Date { get; set; }
}

class SmarthomeContext : DbContext
{
    public DbSet<Sensor> Sensors => Set<Sensor>();
    public DbSet<User> Users => Set<User>();
    public SmarthomeContext(DbContextOptions<SmarthomeContext> options) : base(options)
    {
    }
}