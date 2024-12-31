namespace MinimalAPIDemo.Models
{
    public class EmployeeService : IEmployeeService
    {
        private readonly List<Employee> _employeeList;

        public EmployeeService()
        {
            // Initialize the in-memory list with some sample data
            _employeeList = new List<Employee>
            {
                new Employee { Id = 1, Name = "John Doe", Position = "Software Engineer", Salary = 60000 },
                new Employee { Id = 2, Name = "Jane Smith", Position = "Project Manager", Salary = 80000 }
            };
        }

        public List<Employee> GetAllEmployees()
        {
            return _employeeList;
        }

        public Employee? GetEmployeeById(int id)
        {
            return _employeeList.FirstOrDefault(e => e.Id == id);
        }

        public Employee AddEmployee(Employee newEmployee)
        {
            newEmployee.Id = _employeeList.Count > 0 ? _employeeList.Max(emp => emp.Id) + 1 : 1;
            _employeeList.Add(newEmployee);
            return newEmployee;
        }

        public Employee? UpdateEmployee(int id, Employee updatedEmployee)
        {
            var employee = _employeeList.FirstOrDefault(emp => emp.Id == id);
            if (employee == null)
                return null;

            employee.Name = updatedEmployee.Name;
            employee.Position = updatedEmployee.Position;
            employee.Salary = updatedEmployee.Salary;
            return employee;
        }

        public bool DeleteEmployee(int id)
        {
            var employee = _employeeList.FirstOrDefault(emp => emp.Id == id);
            if (employee == null)
                return false;

            _employeeList.Remove(employee);
            return true;
        }
    }
}
