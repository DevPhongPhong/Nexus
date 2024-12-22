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

        // Get connection by ID
        [HttpGet("{id}")]
        public IActionResult GetConnectionById(int id)
        {
            var connection = _context.Connections.Include(c => c.Customer).FirstOrDefault(c => c.ConnectionId == id);
            if (connection == null)
            {
                return NotFound();
            }
            return Ok(connection);
        }

        // Create a new connection
        [HttpPost]
        public IActionResult CreateConnection([FromBody] Connection connection)
        {
            _context.Connections.Add(connection);
            _context.SaveChanges();
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
            _context.SaveChanges();

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
            _context.SaveChanges();

            return NoContent();
        }
    }

}
