using EIM.Attributes.FilterPipelines.Authorizations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nexus.Models;
using Nexus.Models.Enums;
using System;
using System.Data.Entity;

namespace Nexus.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [RoleAccess(Role.Admin)]
    public class DeviceController : BaseController
    {
        private readonly NexusDbContext _context;

        public DeviceController(NexusDbContext context)
        {
            _context = context;
        }

        // Get all devices
        [HttpGet]
        public IActionResult GetAllDevices()
        {
            var devices = _context.Devices.ToList();
            return Ok(devices);
        }

        // Get device by ID
        [HttpGet("{id}")]
        public IActionResult GetDeviceById(int id)
        {
            var device = _context.Devices.FirstOrDefault(d => d.DeviceId == id);
            if (device == null)
            {
                return NotFound();
            }
            return Ok(device);
        }

        // Create a new device
        [HttpPost]
        public IActionResult CreateDevice([FromBody] Device device)
        {
            _context.Devices.Add(device);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetDeviceById), new { id = device.DeviceId }, device);
        }

        // Update device
        [HttpPut("{id}")]
        public IActionResult UpdateDevice(int id, [FromBody] Device updatedDevice)
        {
            var device = _context.Devices.Find(id);
            if (device == null)
            {
                return NotFound();
            }

            device.DeviceName = updatedDevice.DeviceName;
            device.SupplierId = updatedDevice.SupplierId;
            device.PurchaseDate = updatedDevice.PurchaseDate;
            device.WarrantyPeriod = updatedDevice.WarrantyPeriod;
            _context.SaveChanges();

            return NoContent();
        }

        // Delete device
        [HttpDelete("{id}")]
        public IActionResult DeleteDevice(int id)
        {
            var device = _context.Devices.Find(id);
            if (device == null)
            {
                return NotFound();
            }

            _context.Devices.Remove(device);
            _context.SaveChanges();

            return NoContent();
        }
    }

}
