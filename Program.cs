using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Simple Task API",
        Version = "v1"
    });
});

builder.Services.AddCors();

var app = builder.Build();

// ✅ Always enable Swagger (even in Production)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Simple Task API v1");
});

// ✅ Enable CORS
app.UseCors(policy =>
    policy.AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader());

// ✅ API Endpoints
app.MapGet("/", () => "Welcome to your C# Backend!");

List<TaskItem> tasks = new();

app.MapGet("/tasks", () => tasks);

app.MapPost("/tasks", (TaskItem task) =>
{
    tasks.Add(task);
    return Results.Ok(task);
});

app.MapDelete("/tasks/{id}", (int id) =>
{
    var task = tasks.FirstOrDefault(t => t.Id == id);
    if (task == null) return Results.NotFound();

    tasks.Remove(task);
    return Results.Ok(task);
});

app.Run();

record TaskItem(int Id, string Title, bool IsCompleted);
