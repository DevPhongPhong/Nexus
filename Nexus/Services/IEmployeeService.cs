using Nexus.Models.Enums;
using Nexus.Models;

namespace Nexus.Services
{
    public interface IEmployeeService
    {
        Employee GetEmployeeById(int employeeId);
        EmployeeType GetEmployeeType(int employeeId);
        Employee? ValidateEmployee(string username, string password);
        IEnumerable<Employee> GetAllEmployees();
        void AddEmployee(Employee employee);
        void UpdateEmployee(Employee updatedEmployee);
        void DeleteEmployee(int employeeId);
    }
}
