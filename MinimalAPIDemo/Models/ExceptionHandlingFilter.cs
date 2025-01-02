namespace MinimalAPIDemo.Models
{
    // Define the ExceptionHandlingFilter class that implements the IEndpointFilter interface
    public class ExceptionHandlingFilter : IEndpointFilter
    {
        // Declare a private readonly field for the ILogger<ExceptionHandlingFilter> dependency
        private readonly ILogger<ExceptionHandlingFilter> _logger;

        // Constructor for the ExceptionHandlingFilter class that accepts an ILogger<ExceptionHandlingFilter> parameter
        public ExceptionHandlingFilter(ILogger<ExceptionHandlingFilter> logger)
        {
            // Initialize the _logger field with the provided ILogger<ExceptionHandlingFilter> instance
            _logger = logger;
        }

        // Implement the InvokeAsync method from the IEndpointFilter interface
        // This method is called when the filter is applied to an endpoint
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            try
            {
                // Call the next filter or endpoint in the pipeline and return the result
                return await next(context);
            }
            catch (Exception ex)
            {
                // Log the exception using the logger
                _logger.LogError(ex, "An unhandled exception occurred while processing the request");

                // Return a standardized error response
                return Results.Problem("An unexpected error occurred. Please try again later.");
            }
        }
    }
}