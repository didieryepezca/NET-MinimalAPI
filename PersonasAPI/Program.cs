using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonasAPI.Configuration;
using PersonasAPI.Entities;
using PersonasAPI.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connString = builder.Configuration.GetConnectionString("SQLServerConnection");
builder.Services.AddDbContext<AppDbContext>(options =>{    
    options.UseSqlServer(connString);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.MapGet("/api/personas", async (AppDbContext db) =>
{
    return await db.Personas.ToListAsync();

    //var peRepo = new PersonaRepository();
    //var personas = await peRepo.GetAllPersonas();
    //return personas;
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