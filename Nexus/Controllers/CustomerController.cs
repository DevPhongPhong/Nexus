using EIM.Attributes.FilterPipelines.Authorizations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nexus.Models;
using Nexus.Models.Enums;
using System;
using System.Drawing;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Nexus.Controllers;

[ApiController]
[Route("api/[controller]")]
[RoleAccess(Role.StoreEmployee)]

public class CustomerController : BaseController
{
    private readonly NexusDbContext _context;

    public CustomerController(NexusDbContext context)
    {
        _context = context;
    }

    // Get all customers
    [HttpGet]
    public IActionResult GetAllCustomers(int page, int size, string? query)
    {
        var count = _context.Customers.AsQueryable().Query(query).Count();
        var customers = _context.Customers.AsQueryable().Query(query).Skip((page - 1) * size).Take(size).ToList();
        return Ok(new { customers, count });
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
        try
        {
            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            if (ex.InnerException.Message.Contains("email")) return BadRequest("email is existed");
            if (ex.InnerException.Message.Contains("phone_number")) return BadRequest("phone_number is existed");
        }
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
        customer.UpdatedAt = DateTime.Now;
        try
        {
            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            if (ex.InnerException.Message.Contains("email")) return BadRequest("email is existed");
            if (ex.InnerException.Message.Contains("phone_number")) return BadRequest("phone_number is existed");
        }

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
        try
        {
            _context.SaveChanges();
        }
        catch (Exception ex)
        {

        }

        return NoContent();
    }
}
