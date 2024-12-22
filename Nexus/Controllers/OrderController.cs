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
    [RoleAccess(Role.StoreEmployee)]
    public class OrderController : BaseController
    {
        private readonly NexusDbContext _context;

        public OrderController(NexusDbContext context)
        {
            _context = context;
        }

        // Get all orders
        [HttpGet]
        public IActionResult GetAllOrders()
        {
            var orders = _context.Orders.Include(o => o.Customer).ToList();
            return Ok(orders);
        }

        // Get order by ID
        [HttpGet("{id}")]
        public IActionResult GetOrderById(int id)
        {
            var order = _context.Orders.Include(o => o.Customer).FirstOrDefault(o => o.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        // Create a new order
        [HttpPost]
        public IActionResult CreateOrder([FromBody] Order order)
        {
            var customer = _context.Customers.Find(order.CustomerId);
            if (customer == null)
            {
                return BadRequest("Customer does not exist.");
            }

            _context.Orders.Add(order);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetOrderById), new { id = order.OrderId }, order);
        }

        // Update order
        [HttpPut("{id}")]
        public IActionResult UpdateOrder(int id, [FromBody] Order updatedOrder)
        {
            var order = _context.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }

            order.CustomerId = updatedOrder.CustomerId;
            order.OrderDate = updatedOrder.OrderDate;
            order.TotalPrice = updatedOrder.TotalPrice;
            order.Status = updatedOrder.Status;
            _context.SaveChanges();

            return NoContent();
        }

        // Delete order
        [HttpDelete("{id}")]
        public IActionResult DeleteOrder(int id)
        {
            var order = _context.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            _context.SaveChanges();

            return NoContent();
        }
    }

}
