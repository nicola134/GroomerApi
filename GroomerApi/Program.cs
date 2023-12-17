using FluentValidation;
using FluentValidation.AspNetCore;
using GroomerApi;
using GroomerApi.Authorization;
using GroomerApi.Entities;
using GroomerApi.Middleware;
using GroomerApi.Models;
using GroomerApi.Models.Validators;
using GroomerApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using NLog.Web;
using System.Reflection;
using System.Text;



var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
builder.Host.UseNLog();

// Add services to the container.
var authenticationsSettings = new AuthenticationSettings();
// dodajemy do obiektu authenticationsSettings dane z appsetings.json informacje z nag³ówka  Authentication
builder.Configuration.GetSection("Authentication").Bind(authenticationsSettings);

builder.Services.AddSingleton(authenticationsSettings);
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = "Bearer";
    option.DefaultScheme = "Bearer";
    option.DefaultChallengeScheme = "Bearer";
}).AddJwtBearer(cfg =>
{
    cfg.RequireHttpsMetadata = false;
    cfg.SaveToken = true;
    cfg.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = authenticationsSettings.JwtIssuer,
        ValidAudience = authenticationsSettings.JwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationsSettings.JwtKey)),
    };
});

builder.Services.AddControllers().AddFluentValidation();
builder.Services.AddDbContext<GroomerDbContext>();
builder.Services.AddScoped<IAuthorizationHandler, ResourceOperationReirementHandler>();
builder.Services.AddScoped<GroomerSeeder>(); //Różnica pomiędzy scope a singleton jest taka że, Singleton jest taki sam przez cały czas czyli od początku uruchomienia programu az do jego zamknięcia, a scope jest taki sam tylko w określonym czasie, tzn w api tego przykładem jest 
//zapytanie np.HTTPPost czyli podczas wykonywania dla tego danego requesta obiekt jest taki sam, a podczas następnego nowego requessta np. httpost obiekt będzie już inny
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAnimalService, AnimalService>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddScoped<RequestTimeMiddleware>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IValidator<CreateUserDto>, CreateUserDtoValidator>(); //Również trzeba pamiętać aby dodać 'AddFluentValidation()' przy impletementowaniu controllerow wiersz 21
builder.Services.AddScoped<IValidator<UserQuery>,UserQueryValidator>();
builder.Services.AddScoped<IUserContextService, UserContextService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontEndClient", builder =>

        builder.AllowAnyMethod()
        .AllowAnyHeader()
        .WithOrigins("http://localhost:5139")
        );
});



var app = builder.Build();

var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<GroomerSeeder>();
// Configure the HTTP request pipeline.
app.UseCors("FrontEndClient");
seeder.Seed();
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<RequestTimeMiddleware>();
app.UseAuthentication();
app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Groomer API");
});

app.UseRouting();
app.UseAuthorization();// ta autozyzacja musi byc pomiêdzy UseRouting a UseEndpoitns


app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
public partial class Program { }