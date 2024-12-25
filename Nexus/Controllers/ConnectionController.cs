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
    [RoleAccess(Role.TechnicalEmployee)]
    public class ConnectionController : BaseController
    {
        private readonly NexusDbContext _context;

        public ConnectionController(NexusDbContext context)
        {
            _context = context;
        }

        // Get all connections
        [HttpGet]
        public IActionResult GetAllConnections()
        {
            var connections = _context.Connections.Include(c => c.Customer).ToList();
            return Ok(connections);
        }

        [HttpGet("{id}")]
        public IActionResult GetConnectionById(int id)
        {
            // Fetch the connection by ID
            var connection = _context.Connections.FirstOrDefault(c => c.ConnectionId == id);
            if (connection == null)
            {
                return NotFound();
            }

            // Manually load related Customer
            var customer = _context.Customers.FirstOrDefault(c => c.CustomerId == connection.CustomerId);

            // Manually load related ApplyDevices
            var applyDevices = _context.ApplyDevices
                .Where(ad => ad.ConnectionId == connection.ConnectionId)
                .ToList();

            // Manually load Devices for each ApplyDevice
            foreach (var applyDevice in applyDevices)
            {
                applyDevice.Device = _context.Devices.FirstOrDefault(d => d.DeviceId == applyDevice.DeviceId);
            }

            // Construct the result with related data
            var result = new
            {
                Connection = connection,
                Customer = customer,
                ApplyDevices = applyDevices
            };

            return Ok(result);
        }

        // Create a new connection
        [HttpPost]
        public IActionResult CreateConnection([FromBody] Connection connection)
        {
            connection.CreatedAt = DateTime.Now;
            _context.Connections.Add(connection);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message.Contains("customer_id")) return BadRequest("Invalid CustomerId");
            }
            return CreatedAtAction(nameof(GetConnectionById), new { id = connection.ConnectionId }, connection);
        }

        // Update connection
        [HttpPut("{id}")]
        public IActionResult UpdateConnection(int id, [FromBody] Connection updatedConnection)
        {
            var connection = _context.Connections.Find(id);
            if (connection == null)
            {
                return NotFound();
            }

            connection.ConnectionName = updatedConnection.ConnectionName;
            connection.CustomerId = updatedConnection.CustomerId;
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {

            }

            return NoContent();
        }

        // Delete connection
        [HttpDelete("{id}")]
        public IActionResult DeleteConnection(int id)
        {
            var connection = _context.Connections.Find(id);
            if (connection == null)
            {
                return NotFound();
            }

            _context.Connections.Remove(connection);
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
