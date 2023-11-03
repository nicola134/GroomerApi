using GroomerApi;
using GroomerApi.Entities;
using GroomerApi.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<GroomerDbContext>();
builder.Services.AddScoped<GroomerSeeder>(); //R�nica pomi�dzy scope a singleton jest taka �e, Singleton jest taki sam przez ca�y czas czyli od pocz�tku uruchomienia programu az do jego zamkni�cia, a scope jest taki sam tylko w okre�lonym czasie, tzn w api tego przyk�adem jest 
//zapytanie np.HTTPPost czyli podczas wykonywania dla tego danego requesta obiekt jest taki sam, a podczas nast�pnego nowego requessta np. httpost obiekt b�dzie ju� inny
builder.Services.AddScoped<IUserService, UserService>();
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
