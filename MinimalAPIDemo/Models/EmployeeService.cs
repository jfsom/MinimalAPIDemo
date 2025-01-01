using Microsoft.EntityFrameworkCore;

namespace MinimalAPIDemo.Models
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext _context;

        public EmployeeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            return await _context.Employees.ToListAsync();
        }

        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            return await _context.Employees.FindAsync(id);
        }

        public async Task<Employee> AddEmployeeAsync(Employee newEmployee)
        {
            _context.Employees.Add(newEmployee);
            await _context.SaveChangesAsync();
            return newEmployee;
        }

        public async Task<Employee> UpdateEmployeeAsync(int id, Employee updatedEmployee)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
                return null;

            employee.Name = updatedEmployee.Name;
            employee.Position = updatedEmployee.Position;
            employee.Salary = updatedEmployee.Salary;

            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
                return false;

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}