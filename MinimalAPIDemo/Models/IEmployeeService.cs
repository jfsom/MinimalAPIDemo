namespace MinimalAPIDemo.Models
{
    public interface IEmployeeService
    {
        List<Employee> GetAllEmployees();
        Employee? GetEmployeeById(int id);
        Employee AddEmployee(Employee employee);
        Employee? UpdateEmployee(int id, Employee updatedEmployee);
        bool DeleteEmployee(int id);
    }
}
