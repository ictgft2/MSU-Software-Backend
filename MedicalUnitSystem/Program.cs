using FluentValidation;
using FluentValidation.AspNetCore;
using MedicalUnitSystem;
using MedicalUnitSystem.Data;
using MedicalUnitSystem.Helpers;
using MedicalUnitSystem.Models;
using MedicalUnitSystem.Repositories;
using MedicalUnitSystem.Repositories.Contracts;
using MedicalUnitSystem.Services;
using MedicalUnitSystem.Services.Contracts;
using MedicalUnitSystem.Validators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// customize fluent validation errors
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var validationErrors = context.ModelState
           .Where(x => x.Value.Errors.Any())
           .ToDictionary(
               kvp => kvp.Key,
               kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
           );

        var result = Result<Dictionary<string, string[]>>.Failure(
            "Validation Failed",
            validationErrors
        );

        return new BadRequestObjectResult(result);
    };
});


// Configure EF Core to use Npgsql with connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<HospitalContext>(options =>
    options.UseNpgsql(connectionString));

// Configure Identity services
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<HospitalContext>()
    .AddDefaultTokenProviders();

// Configure repositories
builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();

// Configure Custom Services
builder.Services.AddScoped<IServiceWrapper, ServiceWrapper>();
builder.Services.AddTransient<IPropertyCheckingService, PropertyCheckingService>();
// For Validators
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreatePatientRequestValidator>();

// JWT Authentication configuration

//Jwt configuration starts here
var jwtIssuer = builder.Configuration.GetSection("Jwt:Issuer").Get<string>();
var jwtKey = builder.Configuration.GetSection("Jwt:Key").Get<string>();


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true; 
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtIssuer, 
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

// Add Automapper
builder.Services.AddAutoMapper(
           AppDomain.CurrentDomain.GetAssemblies());

// Configure Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlerMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
