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
    public class UserController : BaseController
    {
        private readonly NexusDbContext _context;

        public UserController(NexusDbContext context)
        {
            _context = context;
        }

        // Get all users
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = _context.Users.Include(u => u.RoleId).ToList();
            return Ok(users);
        }

        // Get user by ID
        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // Create a new user
        [HttpPost]
        public IActionResult CreateUser([FromBody] User user)
        {
            user.CreatedAt = DateTime.Now;
            if (_context.Users.Any(u => u.Username == user.Username))
            {
                return BadRequest("Username already exists.");
            }

            user.PasswordHash = Common.ToSHA256HashString(user.PasswordHash);
            _context.Users.Add(user);
            try
            {
_context.SaveChanges();
}
            catch(Exception ex)
            {
               
            }

            return CreatedAtAction(nameof(GetUserById), new { id = user.UserId }, user);
        }

        // Update user
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] User updatedUser)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            user.FullName = updatedUser.FullName;
            user.Email = updatedUser.Email;
            user.PhoneNumber = updatedUser.PhoneNumber;
            user.RoleId = updatedUser.RoleId;
            user.UpdatedAt = DateTime.Now;
            try
            {
_context.SaveChanges();
}
            catch(Exception ex)
            {
               
            }

            return NoContent();
        }

        // Delete user
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            try
            {
_context.SaveChanges();
}
            catch(Exception ex)
            {
               
            }

            return NoContent();
        }

        // Assign store to employee
        [HttpPost("AssignStore")]
        public IActionResult AssignStoreToEmployee(int userId, int storeId)
        {
            var user = _context.Users.Find(userId);
            if (user == null || user.RoleId != Role.StoreEmployee)
            {
                return BadRequest("User not found or is not an employee.");
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
            catch(Exception ex)
            {
               
            }
            return Ok(employee);
        }
    }

}
