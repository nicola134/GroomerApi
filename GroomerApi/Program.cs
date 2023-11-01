using GroomerApi;
using GroomerApi.Entities;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<GroomerDbContext>();
builder.Services.AddScoped<GroomerSeeder>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());



var app = builder.Build();

var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<GroomerSeeder>();
// Configure the HTTP request pipeline.

seeder.Seed();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
