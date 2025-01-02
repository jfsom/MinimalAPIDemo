using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinimalAPIDemo.Models;

// Create a builder for the web application
var builder = WebApplication.CreateBuilder(args);

// Configure logging
builder.Logging.ClearProviders(); // Clear default providers
builder.Logging.AddConsole(); // Add Console logging provider
builder.Logging.AddDebug(); // Add Debug logging provider

// Add services to the DI container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register EmployeeService in the DI container
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Build the application
var app = builder.Build();

// Configure the HTTP request pipeline for the development environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// CRUD operations for Employee model
// The EmployeeService and ILogger Services are injected into the endpoints

// Endpoint to retrieve all employees
app.MapGet("/employees", async (IEmployeeService employeeService, ILogger<Program> logger) =>
{
    try
    {
        logger.LogInformation("Retrieving all employees");
        int x = 0;
        int y = 10 / x;
        var employees = await employeeService.GetAllEmployeesAsync();

        return Results.Ok(employees);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while retrieving all employees");
        return Results.Problem(ex.Message);
    }

    // To Test the Exception Handling Filter Functionality, We can keep
    // the below mentioned commented code and above mentioned code we can comment 

    //logger.LogInformation("Retrieving all employees");
    //int x = 0;
    //int y = 10 / x;
    //var employees = await employeeService.GetAllEmployeesAsync();
    //return Results.Ok(employees);
})
.AddEndpointFilter<LoggingFilter>()
.AddEndpointFilter<ExceptionHandlingFilter>();

// Endpoint to retrieve a single employee by their ID
app.MapGet("/employees/{id}", async (int id, IEmployeeService employeeService, ILogger<Program> logger) =>
{
    try
    {
        logger.LogInformation("Retrieving employee with ID {Id}", id);
        var employee = await employeeService.GetEmployeeByIdAsync(id);
        if (employee == null)
        {
            logger.LogWarning("Employee with ID {Id} not found", id);
            return Results.NotFound(new { Message = $"Employee with ID {id} not found" });
        }
        return Results.Ok(employee);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while retrieving employee with ID {Id}", id);

        var problemDetails = new ProblemDetails
        {
            Status = 500,
            Title = "An unexpected error occurred.",
            Detail = ex.Message,
            Instance = $"/employees/{id}"
        };

        return Results.Problem(ex.Message);
    }
})
.AddEndpointFilter<LoggingFilter>()
.AddEndpointFilter<ExceptionHandlingFilter>();

// Endpoint to create a new employee with validation
app.MapPost("/employees", async (Employee newEmployee, IEmployeeService employeeService, ILogger<Program> logger) =>
{
    try
    {
        if (!ValidationHelper.TryValidate(newEmployee, out var validationResults))
        {
            // Return 400 Bad Request if validation fails
            return Results.BadRequest(validationResults);
        }

        logger.LogInformation("Creating a new employee");
        var createdEmployee = await employeeService.AddEmployeeAsync(newEmployee);
        return Results.Created($"/employees/{createdEmployee.Id}", createdEmployee);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while creating a new employee");
        return Results.Problem(ex.Message);
    }
})
.AddEndpointFilter<LoggingFilter>()
.AddEndpointFilter<ExceptionHandlingFilter>();

// Endpoint to update an existing employee
app.MapPut("/employees/{id}", async (int id, Employee updatedEmployee, IEmployeeService employeeService, ILogger<Program> logger) =>
{
    try
    {
        if (!ValidationHelper.TryValidate(updatedEmployee, out var validationResults))
        {
            // Return 400 Bad Request if validation fails
            return Results.BadRequest(validationResults);
        }

        logger.LogInformation("Updating employee with ID {Id}", id);
        var employee = await employeeService.UpdateEmployeeAsync(id, updatedEmployee);
        if (employee == null)
        {
            logger.LogWarning("Employee with ID {Id} not found", id);
            return Results.NotFound(new { Message = $"Employee with ID {id} not found" });
        }
        return Results.Ok(employee);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while updating employee with ID {Id}", id);
        return Results.Problem(ex.Message);
    }
})
.AddEndpointFilter<LoggingFilter>()
.AddEndpointFilter<ExceptionHandlingFilter>();

// Endpoint to delete an employee
app.MapDelete("/employees/{id}", async (int id, IEmployeeService employeeService, ILogger<Program> logger) =>
{
    try
    {
        logger.LogInformation("Deleting employee with ID {Id}", id);
        var result = await employeeService.DeleteEmployeeAsync(id);
        if (!result)
        {
            logger.LogWarning("Employee with ID {Id} not found", id);
            return Results.NotFound(new { Message = $"Employee with ID {id} not found" });
        }
        return Results.NoContent();
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while deleting employee with ID {Id}", id);
        return Results.Problem(ex.Message);
    }
})
.AddEndpointFilter<LoggingFilter>()
.AddEndpointFilter<ExceptionHandlingFilter>();

// Run the application
app.Run();