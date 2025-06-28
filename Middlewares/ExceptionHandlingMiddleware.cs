﻿using System.Net;
using System.Text.Json;

namespace DLARS.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred.");

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var isDevelopment = context.RequestServices
                    .GetRequiredService<IWebHostEnvironment>().IsDevelopment();

                var response = new
                {
                    statusCode = context.Response.StatusCode,
                    message = "An unexpected error occurred.",
                    detailed = isDevelopment ? ex.Message : null
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}
