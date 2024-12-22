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
    [RoleAccess(Role.TechnicalEmployee)]
    public class ApplyConnectionController : BaseController
    {
        private readonly NexusDbContext _context;

        public ApplyConnectionController(NexusDbContext context)
        {
            _context = context;
        }

        // Add a new ApplyDevice
        [HttpPost]
        public IActionResult AddApplyConnection([FromBody] ApplyDevice applyDevice)
        {
            // Check if the connection and device exist
            var connection = _context.Connections.Find(applyDevice.ConnectionId);
            var device = _context.Devices.Find(applyDevice.DeviceId);

            if (connection == null || device == null)
            {
                return BadRequest("Connection or Device does not exist.");
            }

            _context.ApplyDevices.Add(applyDevice);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetApplyConnection), new { deviceId = applyDevice.DeviceId, connectionId = applyDevice.ConnectionId }, applyDevice);
        }

        // Get a specific ApplyConnection
        [HttpGet("{deviceId}/{connectionId}")]
        public IActionResult GetApplyConnection(int deviceId, int connectionId)
        {
            var applyDevice = _context.ApplyDevices
                .FirstOrDefault(ad => ad.DeviceId == deviceId && ad.ConnectionId == connectionId);

            if (applyDevice == null)
            {
                return NotFound();
            }

            return Ok(applyDevice);
        }

        // Update an ApplyConnection
        [HttpPut("{deviceId}/{connectionId}")]
        public IActionResult UpdateApplyConnection(int deviceId, int connectionId, [FromBody] ApplyDevice updatedApplyDevice)
        {
            var applyDevice = _context.ApplyDevices
                .FirstOrDefault(ad => ad.DeviceId == deviceId && ad.ConnectionId == connectionId);

            if (applyDevice == null)
            {
                return NotFound();
            }

            applyDevice.ConnectionId = updatedApplyDevice.ConnectionId;
            applyDevice.DeviceId = updatedApplyDevice.DeviceId;

            _context.SaveChanges();

            return NoContent();
        }

        // Delete an ApplyConnection
        [HttpDelete("{deviceId}/{connectionId}")]
        public IActionResult DeleteApplyConnection(int deviceId, int connectionId)
        {
            var applyDevice = _context.ApplyDevices
                .FirstOrDefault(ad => ad.DeviceId == deviceId && ad.ConnectionId == connectionId);

            if (applyDevice == null)
            {
                return NotFound();
            }

            _context.ApplyDevices.Remove(applyDevice);
            _context.SaveChanges();

            return NoContent();
        }
    }

}
