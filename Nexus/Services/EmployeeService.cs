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
        public IEnumerable<Employee> GetAllEmployees()
        {
            // Sử dụng EF Core để lấy danh sách tất cả nhân viên
            return _context.Employees.ToList();
        }
        public void AddEmployee(Employee employee)
        {
            // Sử dụng EF Core để thêm một nhân viên mới
            _context.Employees.Add(employee);
            _context.SaveChanges(); // Lưu thay đổi vào database
            employee.Password = null;
        }
        public void UpdateEmployee(Employee updatedEmployee)
        {
            var existingEmployee = _context.Employees.FirstOrDefault(e => e.EmployeeId == updatedEmployee.EmployeeId);
            if (existingEmployee != null)
            {
                // Cập nhật các thuộc tính
                existingEmployee.Name = updatedEmployee.Name;
                existingEmployee.EmployeeType = updatedEmployee.EmployeeType;
                existingEmployee.UpdatedBy = updatedEmployee.UpdatedBy;
                existingEmployee.UpdatedTime = updatedEmployee.UpdatedTime;

                // Lưu thay đổi vào database
                _context.SaveChanges();
            }
            updatedEmployee.Password = null;
        }
        public void DeleteEmployee(int employeeId)
        {
            var employee = _context.Employees.FirstOrDefault(e => e.EmployeeId == employeeId);
            if (employee != null)
            {
                // Xóa nhân viên khỏi DbSet
                _context.Employees.Remove(employee);
                _context.SaveChanges(); // Lưu thay đổi vào database
            }
        }

    }
}
