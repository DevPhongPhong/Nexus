using EIM.Attributes.FilterPipelines.Authorizations;
using Microsoft.AspNetCore.Mvc;
using Nexus.Models;
using Nexus.Models.Enums;
using Nexus.Services;

namespace Nexus.Controllers
{
    namespace Nexus.Controllers
    {
        [Route("api/Employee")]
        [ApiController]
        [EmployeeTypeAccess(EmployeeType.Admin)] // Áp dụng quyền Admin cho toàn bộ controller
        public class EmployeeController : BaseController
        {
            private readonly IEmployeeService _employeeService;

            public EmployeeController(IEmployeeService employeeService)
            {
                _employeeService = employeeService;
            }

            [HttpGet]
            public IActionResult GetAllEmployees()
            {
                var employees = _employeeService.GetAllEmployees();
                return Ok(employees);
            }

            [HttpGet("{id:int}")]
            public IActionResult GetEmployeeById(int id)
            {
                var employee = _employeeService.GetEmployeeById(id);
                if (employee == null)
                    return NotFound();

                return Ok(employee);
            }

            [HttpPost]
            public IActionResult AddEmployee(Employee employee)
            {
                try
                {
                    employee.UpdatedBy = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "EmployeeType").Value);
                    employee.UpdatedTime = DateTime.Now;
                    employee.Password = AccountController.ToSHA256HashString(employee.Password);
                    _employeeService.AddEmployee(employee);
                    return CreatedAtAction(nameof(GetEmployeeById), new { id = employee.EmployeeId }, employee);
                }
                catch (Exception ex)
                {
                    if (ex.InnerException.Message.Contains("employees.employees_unique")) return BadRequest("Username đã tồn tại");
                    return BadRequest("Có lỗi xảy ra");
                }

            }

            [HttpPut]
            public IActionResult UpdateEmployee(Employee updatedEmployee)
            {
                try
                {
                    updatedEmployee.UpdatedBy = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "EmployeeType").Value);
                    updatedEmployee.UpdatedTime = DateTime.Now;
                    _employeeService.UpdateEmployee(updatedEmployee);
                    return NoContent();
                }
                catch (Exception ex)
                {
                     return BadRequest("Có lỗi xảy ra");
                }
            }


            [HttpDelete("{id:int}")]
            public IActionResult DeleteEmployee(int id)
            {
                var employee = _employeeService.GetEmployeeById(id);
                if (employee == null)
                    return NotFound();

                _employeeService.DeleteEmployee(id);
                return NoContent();
            }
        }
    }
}
