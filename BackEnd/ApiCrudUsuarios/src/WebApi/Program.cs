using System.Text;
using ApiCrudUsuarios.Application.Interfaces;
using ApiCrudUsuarios.Application.Services;
using ApiCrudUsuarios.Domain.Models;
using ApiCrudUsuarios.Infrastructure.Data;
using ApiCrudUsuarios.WebApi.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ApiCrudUsuarios.Application.Exceptions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

builder.Services.AddControllers();

var key = builder.Configuration["JWT:KEY"];

if (string.IsNullOrEmpty(key))
{
    throw new Exception("JWT:KEY no configurado");
}

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(key)
            )
        };

        options.Events = new JwtBearerEvents
        {
            OnChallenge = async context =>
            {
                context.HandleResponse();

                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync("""
                                                  {
                                                      "error": "No estás autenticado"
                                                  }
                                                  """);
            },

            OnForbidden = async context =>
            {
                context.Response.StatusCode = 403;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync("""
                                                  {
                                                      "error": "No tienes permisos para acceder a este recurso"
                                                  }
                                                  """);
            }
        };
    });


builder.Services.AddAuthorization();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IUserService, UserService>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(
    options =>
    {
        options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            Description = "Escribe: Bearer {tu token}"
        });

        options.OperationFilter<AuthOperationFilter>();
    });


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
            policy.WithOrigins("http://localhost:5191", "https://localhost:7025")
                .AllowAnyMethod()
                .AllowAnyHeader());
});


var app = builder.Build();

app.UseCors("AllowFrontend");

app.UseHttpsRedirection();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exceptionHandlerPathFeature =
            context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerPathFeature>();

        var exception = exceptionHandlerPathFeature?.Error;

        context.Response.ContentType = "application/json";

        switch (exception)
        {
            case UserNotFoundException:
                context.Response.StatusCode = 404;
                break;

            case UserAlreadyExistsException:
                context.Response.StatusCode = 400;
                break;

            case InvalidCredentialsException:
                context.Response.StatusCode = 401;
                break;

            default:
                context.Response.StatusCode = 500;
                break;
        }

        var response = new
        {
            error = exception?.Message
        };

        await context.Response.WriteAsJsonAsync(response);
    });
});

app.UseAuthentication();
app.UseAuthorization();

app.UseStatusCodePages(async context =>
{
    var response = context.HttpContext.Response;

    if (response.StatusCode == 404)
    {
        response.ContentType = "application/json";

        await response.WriteAsync("""
                                  {
                                      "error": "ruta no encontrada"
                                  }
                                  """);
    }
});

app.MapControllers();

app.Run();