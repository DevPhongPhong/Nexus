using EIM.Attributes.FilterPipelines.Authorizations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nexus.Models;
using Nexus.Models.Enums;
using System;
using System.Drawing;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
        public IActionResult GetAllPackages(int page, int size, string? query)
        {
            var count = _context.Packages.AsQueryable().Query(query).Count();

            var packages = _context.Packages.AsQueryable().Query(query).Skip((page - 1) * size).Take(size).ToList();
            return Ok(new { packages, count });

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
            package.CreatedAt = DateTime.Now;
            _context.Packages.Add(package);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {

            }
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
            package.UpdatedAt = DateTime.Now;

            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {

            }

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
