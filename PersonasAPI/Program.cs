using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PersonasAPI.Configuration;
using PersonasAPI.Entities;
using PersonasAPI.Repository;
using PersonasAPI.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IPersonaRepository, PersonaRepository>();

//JWT
var jwtSecreto = builder.Configuration["AppSettings:JwtSecreto"];
var key = Encoding.ASCII.GetBytes(jwtSecreto);
builder.Services.AddAuthentication(d =>
{
    d.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    d.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(d => {

    d.RequireHttpsMetadata = false;
    d.SaveToken = true;
    d.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});
//JWT

var connString = builder.Configuration.GetConnectionString("SQLServerConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
{    options.UseSqlServer(connString);
});

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapPost("/generarsesion", (IJwtService jwtService) =>
{
    var jwtToken = jwtService.GenerateToken();

    return jwtToken != String.Empty ? Results.Ok(jwtToken) : Results.NoContent();
});


app.MapGet("/api/personasrepo", [Authorize] async (IPersonaRepository personaRepository) =>
{
    var personas = await personaRepository.GetAllPersonas();
    return personas != null ? Results.Ok(personas) : Results.NotFound();
});

app.MapGet("/api/personas", async (AppDbContext db) =>
{
    var personas = await db.Personas.ToListAsync();

    return personas != null ? Results.Ok(personas) : Results.NotFound();
});

app.MapGet("/api/personasbyid", async (AppDbContext db, int id) =>
{
    var persons = await db.Personas.FindAsync(id);

    return persons != null? Results.Ok(persons) : Results.NotFound();     
});

app.MapPost("/api/addpersonas", (AppDbContext db, Personas person) =>
{
    db.Personas.Add(person);
    db.SaveChanges();
    return Results.Created($"/api/personasbyid/{person.ID}", person);
});

app.MapPut("/api/updatepersonas/", (AppDbContext db, Personas person) =>
{
    var personas = db.Personas.FirstOrDefault(x => x.ID == person.ID);

    if (personas != null)
    {
        personas.NOMBRE = person.NOMBRE;
        personas.APELLIDO = person.APELLIDO;
        db.Personas.Update(personas);
        db.SaveChanges();
        //return Results.NoContent();
        return Results.Accepted("Se actualizo correctamente");
    }
    else {
        return Results.NotFound();       
    }    
});

//Delete persona
app.MapDelete("/api/removepersonas/", (AppDbContext db, int id) =>
{
    var persona = db.Personas.Find(id);
    db.Personas.Remove(persona);
    db.SaveChanges();
    return Results.NoContent();
});

app.Run();