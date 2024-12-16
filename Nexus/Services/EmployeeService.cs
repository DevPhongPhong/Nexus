using Nexus.Models.Enums;
using Nexus.Models;

namespace Nexus.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly NexusDbContext _context;

        public EmployeeService(NexusDbContext context)
        {
            _context = context;
        }

        public Employee GetEmployeeById(int employeeId)
        {
            return _context.Employees.FirstOrDefault(e => e.EmployeeId == employeeId);
        }

        public EmployeeType GetEmployeeType(int employeeId)
        {
            if (_context.AdminEmployees.Any(a => a.AdminEmployeeId == employeeId))
                return EmployeeType.Admin;

            if (_context.RentalShopEmployees.Any(r => r.RentalShopEmployeeId == employeeId))
                return EmployeeType.RentalShopEmp;

            if (_context.TechnicalEmployees.Any(t => t.TechnicalEmployeeId == employeeId))
                return EmployeeType.TechnicalEmp;

            return EmployeeType.Other;
        }
        public Employee? ValidateEmployee(string username, string password)
        {
            // Kiểm tra username và mật khẩu
            return _context.Employees
                .FirstOrDefault(e => e.Username == username && e.Password == password);
        }
    }
}
