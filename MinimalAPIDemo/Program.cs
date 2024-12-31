// Importing necessary namespaces and classes
using MinimalAPIDemo.Models;

// Creating a builder for the application
var builder = WebApplication.CreateBuilder(args);

// Add services to the DI container
// Add API explorer for endpoint documentation
builder.Services.AddEndpointsApiExplorer();
// Add Swagger for API documentation
builder.Services.AddSwaggerGen();

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

// Create an in-memory list to store Employee data
var employeeList = new List<Employee>
{
    new Employee { Id = 1, Name = "John Doe", Position = "Software Engineer", Salary = 60000 },
    new Employee { Id = 2, Name = "Jane Smith", Position = "Project Manager", Salary = 80000 }
};

// CRUD operations for Employee model

// Endpoint to retrieve all employees
// Map a GET request to /employees to return the employee list
app.MapGet("/employees", () => employeeList);

// Endpoint to retrieve a single employee by their ID
app.MapGet("/employees/{id}", (int id) =>
{
    // Find the employee with the specified ID
    var employee = employeeList.FirstOrDefault(e => e.Id == id);
    // Return 200 OK if found, otherwise 404 Not Found
    return employee is not null ? Results.Ok(employee) : Results.NotFound();
});

// Endpoint to create a new employee
app.MapPost("/employees", (Employee newEmployee) =>
{
    // Determine the next ID for the new employee
    newEmployee.Id = employeeList.Count > 0 ? employeeList.Max(emp => emp.Id) + 1 : 1;
    // Add the new employee to the list
    employeeList.Add(newEmployee);
    // Return a 201 Created response with the new employee
    return Results.Created($"/employees/{newEmployee.Id}", newEmployee);
});

// Endpoint to update an existing employee
app.MapPut("/employees/{id}", (int id, Employee updatedEmployee) =>
{
    // Find the employee by ID
    var employee = employeeList.FirstOrDefault(emp => emp.Id == id);
    if (employee is null) return Results.NotFound(); // If not found, return 404

    // Update employee details
    employee.Name = updatedEmployee.Name;
    employee.Position = updatedEmployee.Position;
    employee.Salary = updatedEmployee.Salary;
    // Return the updated employee
    return Results.Ok(employee);
});

// Endpoint to delete an employee
app.MapDelete("/employees/{id}", (int id) =>
{
    // Find the employee by ID
    var employee = employeeList.FirstOrDefault(emp => emp.Id == id);
    if (employee is null) return Results.NotFound(); // If not found, return 404

    // Remove the employee from the list
    employeeList.Remove(employee);
    // Return a 204 No Content response
    return Results.NoContent();
});

// Run the application
app.Run();