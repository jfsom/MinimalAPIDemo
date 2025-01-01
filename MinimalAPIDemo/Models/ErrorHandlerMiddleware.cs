// Import namespace for HTTP status codes
using System.Net;

// Import namespace for JSON serialization
using System.Text.Json;

namespace MinimalAPIDemo.Models
{
    public class ErrorHandlerMiddleware
    {
        // Field to store the next middleware in the pipeline
        private readonly RequestDelegate _next;

        // Constructor to initialize the middleware with the next delegate
        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            // Assign the next delegate to the field
            _next = next;
        }

        // Method to handle the HTTP request
        public async Task Invoke(HttpContext context)
        {
            try
            {
                // Pass the context to the next middleware in the pipeline
                await _next(context);
            }
            catch (Exception ex) // Catch any exceptions that occur
            {
                // Handle the exception
                await HandleExceptionAsync(context, ex);
            }
        }

        // Method to handle exceptions
        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // Set the response content type to JSON
            context.Response.ContentType = "application/json";

            // Set the status code to 500 (Internal Server Error)
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            // Create a JSON response with the error message and details
            var result = JsonSerializer.Serialize(new { Message = "An unexpected error occurred.", Detail = exception.Message });

            // Write the JSON response to the HTTP response
            return context.Response.WriteAsync(result);
        }
    }
}