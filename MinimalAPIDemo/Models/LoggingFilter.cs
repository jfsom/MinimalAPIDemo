namespace MinimalAPIDemo.Models
{
    // Define the LoggingFilter class that implements the IEndpointFilter interface
    public class LoggingFilter : IEndpointFilter
    {
        // Declare a private readonly field for the ILogger<LoggingFilter> dependency
        private readonly ILogger<LoggingFilter> _logger;

        // Constructor for the LoggingFilter class that accepts an ILogger<LoggingFilter> parameter
        public LoggingFilter(ILogger<LoggingFilter> logger)
        {
            // Initialize the _logger field with the provided ILogger<LoggingFilter> instance
            _logger = logger;
        }

        // Implement the InvokeAsync method from the IEndpointFilter interface
        // This method is called when the filter is applied to an endpoint
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            // Log information about the incoming request using the logger
            _logger.LogInformation("Handling request: {RequestPath}", context.HttpContext.Request.Path);

            // Call the next filter or endpoint in the pipeline and wait for the result
            var result = await next(context);

            // Log information about the completed request using the logger
            _logger.LogInformation("Finished handling request: {RequestPath}", context.HttpContext.Request.Path);

            // Return the result of the next filter or endpoint in the pipeline
            return result;
        }
    }
}