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
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {

            }

            return Ok(applyDevice);
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
