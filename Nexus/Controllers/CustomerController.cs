using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nexus.Models;
using System;

namespace Nexus.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : BaseController
    {
        private readonly NexusDbContext _context;

        public CustomerController(NexusDbContext context)
        {
            _context = context;
        }

        // Get all customers
        [HttpGet]
        public IActionResult GetAllCustomers()
        {
            var customers = _context.Customers.ToList();
            return Ok(customers);
        }

        // Get customer by ID
        [HttpGet("{id}")]
        public IActionResult GetCustomerById(int id)
        {
            var customer = _context.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer);
        }

        // Create a new customer
        [HttpPost]
        public IActionResult CreateCustomer([FromBody] Customer customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetCustomerById), new { id = customer.CustomerId }, customer);
        }

        // Update customer
        [HttpPut("{id}")]
        public IActionResult UpdateCustomer(int id, [FromBody] Customer updatedCustomer)
        {
            var customer = _context.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }

            customer.FullName = updatedCustomer.FullName;
            customer.Email = updatedCustomer.Email;
            customer.PhoneNumber = updatedCustomer.PhoneNumber;
            customer.Address = updatedCustomer.Address;
            _context.SaveChanges();

            return NoContent();
        }

        // Delete customer
        [HttpDelete("{id}")]
        public IActionResult DeleteCustomer(int id)
        {
            var customer = _context.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customer);
            _context.SaveChanges();

            return NoContent();
        }
    }

}
