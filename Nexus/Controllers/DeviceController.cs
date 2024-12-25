using EIM.Attributes.FilterPipelines.Authorizations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nexus.Models;
using Nexus.Models.Enums;
using System;
using System.Data.Entity;

namespace Nexus.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [RoleAccess(Role.Admin, Role.TechnicalEmployee)]
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

            // Truy vấn Supplier mà không include danh sách Devices
            if (device.SupplierId.HasValue)
            {
                device.Supplier = _context.Suppliers
                                           .Where(x => x.SupplierId == device.SupplierId)
                                           .Select(s => new Supplier
                                           {
                                               SupplierId = s.SupplierId,
                                               SupplierName = s.SupplierName,
                                               PhoneNumber = s.PhoneNumber,
                                               Email = s.Email,
                                               Address = s.Address,
                                               CreatedAt = s.CreatedAt,
                                               UpdatedAt = s.UpdatedAt
                                           })
                                           .FirstOrDefault();
            }

            // Load danh sách ApplyDevices
            device.ApplyDevices = _context.ApplyDevices
                                          .Where(x => x.DeviceId == device.DeviceId)
                                          .Select(x => new ApplyDevice
                                          {
                                              ApplyDeviceId = x.ApplyDeviceId,
                                              ConnectionId = x.ConnectionId,
                                              DeviceId = x.DeviceId,
                                              Connection = _context.Connections.FirstOrDefault(y => y.ConnectionId == x.ConnectionId),
                                              Device = null

                                          })
                                          .ToList();

            return Ok(device);
        }


        // Create a new device
        [HttpPost]
        public IActionResult CreateDevice([FromBody] Device device)
        {
            _context.Devices.Add(device);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {

            }
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

            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {

            }

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
