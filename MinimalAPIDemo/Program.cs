using Microsoft.AspNetCore.Mvc;
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
builder.Services.AddSingleton<IEmployeeService, EmployeeService>();

// Build the application
var app = builder.Build();

// Use the custom error handling middleware
app.UseMiddleware<ErrorHandlerMiddleware>();

// Configure the HTTP request pipeline for the development environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// CRUD operations for Employee model
// The EmployeeService and ILogger Services are injected into the endpoints

// Endpoint to retrieve all employees
app.MapGet("/employees", (IEmployeeService employeeService, ILogger<Program> logger) =>
{
    try
    {
        logger.LogInformation("Retrieving all employees");
        return Results.Ok(employeeService.GetAllEmployees());
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while retrieving all employees");
        return Results.Problem(ex.Message);
    }
});

// Endpoint to retrieve a single employee by their ID
app.MapGet("/employees/{id}", (int id, IEmployeeService employeeService, ILogger<Program> logger) =>
{
    try
    {
        logger.LogInformation("Retrieving employee with ID {Id}", id);
        var employee = employeeService.GetEmployeeById(id);
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
});

// Endpoint to create a new employee with validation
app.MapPost("/employees", (Employee newEmployee, IEmployeeService employeeService, ILogger<Program> logger) =>
{
    try
    {
        if (!ValidationHelper.TryValidate(newEmployee, out var validationResults))
        {
            // Return 400 Bad Request if validation fails
            return Results.BadRequest(validationResults);
        }

        logger.LogInformation("Creating a new employee");
        var createdEmployee = employeeService.AddEmployee(newEmployee);
        return Results.Created($"/employees/{createdEmployee.Id}", createdEmployee);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while creating a new employee");
        return Results.Problem(ex.Message);
    }
});

// Endpoint to update an existing employee
app.MapPut("/employees/{id}", (int id, Employee updatedEmployee, IEmployeeService employeeService, ILogger<Program> logger) =>
{
    try
    {
        if (!ValidationHelper.TryValidate(updatedEmployee, out var validationResults))
        {
            // Return 400 Bad Request if validation fails
            return Results.BadRequest(validationResults);
        }

        logger.LogInformation("Updating employee with ID {Id}", id);
        var employee = employeeService.UpdateEmployee(id, updatedEmployee);
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
});

// Endpoint to delete an employee
app.MapDelete("/employees/{id}", (int id, IEmployeeService employeeService, ILogger<Program> logger) =>
{
    try
    {
        logger.LogInformation("Deleting employee with ID {Id}", id);
        var result = employeeService.DeleteEmployee(id);
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
});

// Run the application
app.Run();