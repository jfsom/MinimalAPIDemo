namespace MinimalAPIDemo.Models
{
    public class EmployeeService : IEmployeeService
    {
        private readonly List<Employee> _employeeList;

        public EmployeeService()
        {
            _employeeList = new List<Employee>
            {
                new Employee { Id = 1, Name = "John Doe", Position = "Software Engineer", Salary = 60000 },
                new Employee { Id = 2, Name = "Jane Smith", Position = "Project Manager", Salary = 80000 }
            };
        }

        public Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            return Task.FromResult<IEnumerable<Employee>>(_employeeList);
        }

        public Task<Employee> GetEmployeeByIdAsync(int id)
        {
            var employee = _employeeList.FirstOrDefault(e => e.Id == id);
            return Task.FromResult(employee);
        }

        public Task<Employee> AddEmployeeAsync(Employee newEmployee)
        {
            newEmployee.Id = _employeeList.Count > 0 ? _employeeList.Max(emp => emp.Id) + 1 : 1;
            _employeeList.Add(newEmployee);
            return Task.FromResult(newEmployee);
        }

        public Task<Employee> UpdateEmployeeAsync(int id, Employee updatedEmployee)
        {
            var employee = _employeeList.FirstOrDefault(emp => emp.Id == id);
            if (employee == null)
                return Task.FromResult<Employee>(null);

            employee.Name = updatedEmployee.Name;
            employee.Position = updatedEmployee.Position;
            employee.Salary = updatedEmployee.Salary;
            return Task.FromResult(employee);
        }

        public Task<bool> DeleteEmployeeAsync(int id)
        {
            var employee = _employeeList.FirstOrDefault(emp => emp.Id == id);
            if (employee == null)
                return Task.FromResult(false);

            _employeeList.Remove(employee);
            return Task.FromResult(true);
        }
    }
}