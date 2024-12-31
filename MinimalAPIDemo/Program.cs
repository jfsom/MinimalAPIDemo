using MinimalAPIDemo.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the DI container
// Add API explorer for endpoint documentation
builder.Services.AddEndpointsApiExplorer();

// Add Swagger for API documentation
builder.Services.AddSwaggerGen();

// Register EmployeeService in the DI container
builder.Services.AddSingleton<IEmployeeService, EmployeeService>();

// Build the application
var app = builder.Build();

// Configure the HTTP request pipeline for the development environment
if (app.Environment.IsDevelopment())
{
    // Use Swagger middleware to generate Swagger Documentation
    app.UseSwagger();
    // Use Swagger UI middleware to interact with the Swagger documentation
    app.UseSwaggerUI();
}

// CRUD operations for Employee model
// The EmployeeService is injected into the endpoints

// Endpoint to retrieve all employees
app.MapGet("/employees", (IEmployeeService employeeService) => employeeService.GetAllEmployees());

// Endpoint to retrieve a single employee by their ID
app.MapGet("/employees/{id}", (int id, IEmployeeService employeeService) =>
{
    var employee = employeeService.GetEmployeeById(id);
    return employee is not null ? Results.Ok(employee) : Results.NotFound();
});

// Endpoint to create a new employee
app.MapPost("/employees", (Employee newEmployee, IEmployeeService employeeService) =>
{
    var createdEmployee = employeeService.AddEmployee(newEmployee);
    return Results.Created($"/employees/{createdEmployee.Id}", createdEmployee);
});

// Endpoint to update an existing employee
app.MapPut("/employees/{id}", (int id, Employee updatedEmployee, IEmployeeService employeeService) =>
{
    var employee = employeeService.UpdateEmployee(id, updatedEmployee);
    return employee is not null ? Results.Ok(employee) : Results.NotFound();
});

// Endpoint to delete an employee
app.MapDelete("/employees/{id}", (int id, IEmployeeService employeeService) =>
{
    var result = employeeService.DeleteEmployee(id);
    return result ? Results.NoContent() : Results.NotFound();
});

// Run the application
app.Run();