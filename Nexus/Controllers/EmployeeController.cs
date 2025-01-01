using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nexus.Models.Enums;
using Nexus.Models;
using System;
using System.Data.Entity;
using EIM.Attributes.FilterPipelines.Authorizations;
using Microsoft.AspNetCore.Authorization;

namespace Nexus.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [RoleAccess(Role.Admin)]
    public class EmployeeController : BaseController
    {
        private readonly NexusDbContext _context;

        public EmployeeController(NexusDbContext context)
        {
            _context = context;
        }

        // Get all users
        [HttpGet]
        public IActionResult GetAllEmployees(int page, int size)
        {
            var users = _context.Employees.Include(u => u.RoleId).Skip((page - 1) * size).Take(size).ToList();
            return Ok(users);
        }

        // Get user by ID
        [HttpGet("{id}")]
        public IActionResult GetEmployeeById(int id)
        {
            var user = _context.Employees.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // Create a new user
        [HttpPost]
        public IActionResult CreateEmployee([FromBody] Employee user)
        {
            user.CreatedAt = DateTime.Now;
            if (_context.Employees.Any(u => u.Username == user.Username))
            {
                return BadRequest("Username already exists.");
            }

            user.PasswordHash = Common.ToSHA256HashString(user.PasswordHash);
            _context.Employees.Add(user);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {

            }

            return CreatedAtAction(nameof(GetEmployeeById), new { id = user.EmployeeId }, user);
        }

        // Update user
        [HttpPut("{id}")]
        public IActionResult UpdateEmployee(int id, [FromBody] Employee updatedEmployee)
        {
            var user = _context.Employees.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            user.FullName = updatedEmployee.FullName;
            user.Email = updatedEmployee.Email;
            user.PhoneNumber = updatedEmployee.PhoneNumber;
            user.RoleId = updatedEmployee.RoleId;
            user.UpdatedAt = DateTime.Now;
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {

            }

            return NoContent();
        }

        // Delete user
        [HttpDelete("{id}")]
        public IActionResult DeleteEmployee(int id)
        {
            var user = _context.Employees.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(user);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {

            }

            return NoContent();
        }

        // Assign store to employee
        [HttpPost("AssignStore")]
        public IActionResult AssignStoreToEmployee(int userId, int storeId)
        {
            var user = _context.Employees.Find(userId);
            if (user == null || user.RoleId != Role.StoreEmployee)
            {
                return BadRequest("Employee not found or is not an employee.");
            }

            var employee = _context.Employees.FirstOrDefault(e => e.EmployeeId == userId);
            if (employee == null)
            {
                employee = new Employee { EmployeeId = userId, StoreId = storeId };
                _context.Employees.Add(employee);
            }
            else
            {
                employee.StoreId = storeId;
            }

            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {

            }
            return Ok(employee);
        }
    }

}
