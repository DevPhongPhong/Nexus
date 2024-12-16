using Nexus.Models.Enums;
using Nexus.Models;

namespace Nexus.Services
{
    public interface IEmployeeService
    {
        Employee GetEmployeeById(int employeeId);
        EmployeeType GetEmployeeType(int employeeId);
        Employee? ValidateEmployee(string username, string password);
    }
}
