using GroomerApi;
using GroomerApi.Entities;
using GroomerApi.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<GroomerDbContext>();
builder.Services.AddScoped<GroomerSeeder>(); //Ró¿nica pomiêdzy scope a singleton jest taka ¿e, Singleton jest taki sam przez ca³y czas czyli od pocz¹tku uruchomienia programu az do jego zamkniêcia, a scope jest taki sam tylko w okreœlonym czasie, tzn w api tego przyk³adem jest 
//zapytanie np.HTTPPost czyli podczas wykonywania dla tego danego requesta obiekt jest taki sam, a podczas nastêpnego nowego requessta np. httpost obiekt bêdzie ju¿ inny
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
