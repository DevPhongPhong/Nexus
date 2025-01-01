using EIM.Attributes.FilterPipelines.Authorizations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nexus.Models;
using Nexus.Models.Enums;
using System;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Nexus.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [RoleAccess(Role.Admin)]
    public class SupplierController : BaseController
    {
        private readonly NexusDbContext _context;

        public SupplierController(NexusDbContext context)
        {
            _context = context;
        }

        // Get all suppliers
        [HttpGet]
        public IActionResult GetAllSuppliers(int page, int size, string? query)
        {
            var count = _context.Suppliers.AsQueryable().Query(query).Count();

            var suppliers = _context.Suppliers.AsQueryable().Query(query).Skip((page - 1) * size).Take(size).ToList();
            return Ok(new { suppliers, count });

        }

        // Get supplier by ID
        [HttpGet("{id}")]
        public IActionResult GetSupplierById(int id)
        {
            var supplier = _context.Suppliers.Find(id);
            if (supplier == null)
            {
                return NotFound();
            }
            return Ok(supplier);
        }

        // Create a new supplier
        [HttpPost]
        public IActionResult CreateSupplier([FromBody] Supplier supplier)
        {
            supplier.CreatedAt = DateTime.Now;
            _context.Suppliers.Add(supplier);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message.Contains("email_unique")) return BadRequest("Email is existed");
                if (ex.InnerException.Message.Contains("phone_number_unique")) return BadRequest("Phonenum is existed");
            }
            return CreatedAtAction(nameof(GetSupplierById), new { id = supplier.SupplierId }, supplier);
        }

        // Update supplier
        [HttpPut("{id}")]
        public IActionResult UpdateSupplier(int id, [FromBody] Supplier updatedSupplier)
        {
            var supplier = _context.Suppliers.Find(id);
            if (supplier == null)
            {
                return NotFound();
            }

            supplier.SupplierName = updatedSupplier.SupplierName;
            supplier.PhoneNumber = updatedSupplier.PhoneNumber;
            supplier.Email = updatedSupplier.Email;
            supplier.Address = updatedSupplier.Address;
            supplier.UpdatedAt = DateTime.Now;

            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message.Contains("email_unique")) return BadRequest("Email is existed");
                if (ex.InnerException.Message.Contains("phone_number_unique")) return BadRequest("Phonenum is existed");
            }

            return NoContent();
        }

        // Delete supplier
        [HttpDelete("{id}")]
        public IActionResult DeleteSupplier(int id)
        {
            var supplier = _context.Suppliers.Find(id);
            if (supplier == null)
            {
                return NotFound();
            }

            _context.Suppliers.Remove(supplier);
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

}
