using KinderHub.Enrollment.Data;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using FluentValidation.AspNetCore;
using KinderHub.Enrollment.Validators;
using KinderHub.Enrollment.Repositories.Interfaces;
using KinderHub.Enrollment.Repositories;
using KinderHub.Enrollment.Services.Interfaces;
using KinderHub.Enrollment.Services;
using KinderHub.Enrollment.Middleware;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<EnrollmentDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
        .UseSnakeCaseNamingConvention()
        .LogTo(Console.WriteLine, LogLevel.Information));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!))
        };
        options.Events = new JwtBearerEvents
        {
            OnChallenge = async context =>
            {
                context.HandleResponse();
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/problem+json";
                await context.Response.WriteAsJsonAsync(new ProblemDetails
                {
                    Status = 401,
                    Title = "Unauthorized",
                    Detail = "Authentication required",
                    Type = "https://httpstatuses.io/401"
                });
            },
            OnForbidden = async context =>
            {
                context.Response.StatusCode = 403;
                context.Response.ContentType = "application/problem+json";
                await context.Response.WriteAsJsonAsync(new ProblemDetails
                {
                    Status = 403,
                    Title = "Forbidden",
                    Detail = "You do not have permission to access this resource",
                    Type = "https://httpstatuses.io/403"
                });
            }
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddProblemDetails();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateClassroomRequestValidator>();
builder.Services.AddScoped<IClassroomRepository, ClassroomRepository>();
builder.Services.AddScoped<IClassroomService, ClassroomService>();
builder.Services.AddScoped<IClassroomTeacherRepository, ClassroomTeacherRepository>();
builder.Services.AddScoped<IClassroomTeacherService, ClassroomTeacherService>();


builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();