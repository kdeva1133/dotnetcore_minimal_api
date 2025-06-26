using Microsoft.EntityFrameworkCore;
using WebApplication1;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddCors(options => { 
    options.AddPolicy("AllowReactApp", policy => policy.WithOrigins("http://localhost:3000 http://localhost:4200")
    .AllowAnyMethod()
    .AllowAnyHeader()); 
});
var app = builder.Build();

app.UseCors("AllowReactApp");

app.MapGet("/todoitems",async (TodoDb db) =>
    await db.Todos.ToListAsync());

app.MapGet("/gettodoitem/{id}", async (int id, TodoDb db) => 
    await db.Todos.FindAsync(id) is Todo todo ? Results.Ok(todo) : Results.NotFound());

app.MapPost("/createtodoitem", async (Todo todo,TodoDb db)=>
{
    db.Todos.Add(todo);
    await db.SaveChangesAsync();
    return Results.Created();
});
app.MapPut("/updatetodoitem", async (Todo todo, TodoDb db)=>
{
    db.Todos.Update(todo);
    await db.SaveChangesAsync();
});

app.Run();
