using EIM.Attributes.FilterPipelines.Authorizations;
using Microsoft.AspNetCore.Mvc;
using Nexus.Models.Enums;
using Nexus;
using Nexus.Models;

namespace Nexus.Controllers;

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
    public IActionResult GetAllConnections(int page, int size, string? query)
    {
        var count = _context.Connections.AsQueryable().Query(query).Count();
        var connections = _context.Connections.AsQueryable().Query(query).Skip((page - 1) * size).Take(size).ToList();

        // Thực hiện thủ công để lấy dữ liệu liên quan
        foreach (var connection in connections)
        {
            connection.Payment = _context.Payments
                .FirstOrDefault(p => p.PaymentId == connection.PaymentId);
        }

        return Ok(new { connections, count });
    }

    // Get connection by ID
    [HttpGet("{id}")]
    public IActionResult GetConnectionById(int id)
    {
        var connection = _context.Connections
            .FirstOrDefault(c => c.ConnectionId == id);

        if (connection == null)
        {
            return NotFound($"Connection with ID {id} not found.");
        }

        // Lấy dữ liệu liên quan thủ công
        connection.Payment = _context.Payments
            .FirstOrDefault(p => p.PaymentId == connection.PaymentId);

        return Ok(connection);
    }

    // Create a new connection
    [HttpPost]
    public IActionResult CreateConnection([FromBody] Connection connection)
    {
        if (connection == null)
        {
            return BadRequest("Invalid connection data.");
        }

        _context.Connections.Add(connection);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetConnectionById), new { id = connection.ConnectionId }, connection);
    }

    // Update connection
    [HttpPut("{id}")]
    public IActionResult UpdateConnection(int id, [FromBody] Connection updatedConnection)
    {
        if (updatedConnection == null || updatedConnection.ConnectionId != id)
        {
            return BadRequest("Connection data is invalid or mismatched.");
        }

        var connection = _context.Connections.Find(id);
        if (connection == null)
        {
            return NotFound($"Connection with ID {id} not found.");
        }

        // Update properties
        connection.ConnectionName = updatedConnection.ConnectionName;
        connection.PaymentId = updatedConnection.PaymentId;
        connection.CreatedAt = updatedConnection.CreatedAt;

        _context.Connections.Update(connection);
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
            return NotFound($"Connection with ID {id} not found.");
        }

        _context.Connections.Remove(connection);
        _context.SaveChanges();

        return NoContent();
    }
}