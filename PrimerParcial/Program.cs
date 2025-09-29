using BackendApi.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext (lee la cadena de appsettings o variables de entorno)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Swagger SIEMPRE (incluye producción)
app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/ping", () => Results.Ok("pong"));

app.MapControllers();

app.Run();d