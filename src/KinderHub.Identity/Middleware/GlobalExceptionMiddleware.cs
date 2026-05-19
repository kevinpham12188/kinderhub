using KinderHub.Identity.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace KinderHub.Identity.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

    private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var problemDetails = ex switch
            {
                ConflictException => new ProblemDetails
                {
                    Status = 409,
                    Title = "Conflict",
                    Detail = ex.Message,
                    Type = "https://httpstatuses.io/409"
                },
                UnauthorizedException => new ProblemDetails
                {
                    Status = 401,
                    Title = "Unauthorized",
                    Detail = "Invalid email or password",
                    Type = "https://httpstatuses.io/401"
                },
                _ => new ProblemDetails
                {
                    Status = 500,
                    Title = "Server Error",
                    Detail = "An unexpected error occurred",
                    Type = "https://httpstatuses.io/500"
                }
            };

            context.Response.StatusCode = problemDetails.Status ?? 500;
            context.Response.ContentType = "application/problem+json";

            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}