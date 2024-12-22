using EIM.Attributes.FilterPipelines.Authorizations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nexus.Models;
using Nexus.Models.Enums;
using System;

namespace Nexus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [RoleAccess(Role.Admin)]
    public class PackageController : BaseController
    {
        private readonly NexusDbContext _context;

        public PackageController(NexusDbContext context)
        {
            _context = context;
        }

        // Get all packages
        [HttpGet]
        public IActionResult GetAllPackages()
        {
            var packages = _context.Packages.ToList();
            return Ok(packages);
        }

        // Get package by ID
        [HttpGet("{id}")]
        public IActionResult GetPackageById(int id)
        {
            var package = _context.Packages.Find(id);
            if (package == null)
            {
                return NotFound();
            }
            return Ok(package);
        }

        // Create a new package
        [HttpPost]
        public IActionResult CreatePackage([FromBody] Package package)
        {
            _context.Packages.Add(package);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetPackageById), new { id = package.PackageId }, package);
        }

        // Update package
        [HttpPut("{id}")]
        public IActionResult UpdatePackage(int id, [FromBody] Package updatedPackage)
        {
            var package = _context.Packages.Find(id);
            if (package == null)
            {
                return NotFound();
            }

            package.PackageName = updatedPackage.PackageName;
            package.Description = updatedPackage.Description;
            package.Price = updatedPackage.Price;
            _context.SaveChanges();

            return NoContent();
        }

        // Delete package
        [HttpDelete("{id}")]
        public IActionResult DeletePackage(int id)
        {
            var package = _context.Packages.Find(id);
            if (package == null)
            {
                return NotFound();
            }

            _context.Packages.Remove(package);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
