using MedicalUnitSystem.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace MedicalUnitSystem
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
		{
			_next = next;
			_logger = logger;
		}

        public async Task Invoke(HttpContext context)
        {
			try
			{
				await _next(context);
			}
			catch (Exception error)
			{
				var response = context.Response;
				response.ContentType = "application/json";
				response.StatusCode = error switch
				{
					MedicalAppException a => (int)a.StatusCode,
					_ => StatusCodes.Status500InternalServerError,
				};
				var problemDetails = new ProblemDetails
				{
					Status = response.StatusCode,
					Title = error.Message
				};

				_logger.LogError(error.Message);
				var result = JsonSerializer.Serialize(problemDetails);
				await response.WriteAsync(result);
			}
        }
    }
}
