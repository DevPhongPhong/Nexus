using EIM.Attributes.FilterPipelines.Authorizations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nexus.Models;
using Nexus.Models.Enums;
using System;

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
        public IActionResult GetAllSuppliers()
        {
            var suppliers = _context.Suppliers.ToList();
            return Ok(suppliers);
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
            _context.Suppliers.Add(supplier);
            _context.SaveChanges();
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
            supplier.ContactName = updatedSupplier.ContactName;
            supplier.PhoneNumber = updatedSupplier.PhoneNumber;
            supplier.Email = updatedSupplier.Email;
            supplier.Address = updatedSupplier.Address;
            _context.SaveChanges();

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
            _context.SaveChanges();

            return NoContent();
        }
    }

}
