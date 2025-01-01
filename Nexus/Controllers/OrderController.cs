using EIM.Attributes.FilterPipelines.Authorizations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nexus.Models;
using Nexus.Models.Enums;
using System;
using System.Data.Entity;
using System.Drawing;

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
        public IActionResult GetAllOrders(int page, int size)
        {
            var orders = _context.Orders.Skip((page - 1) * size).Take(size).ToList();

            return Ok(orders);
        }

        // Get order by ID (include order details)
        [HttpGet("{id}")]
        public IActionResult GetOrderById(int id)
        {
            var order = _context.Orders.FirstOrDefault(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound($"Order with ID {id} not found.");
            }

            order.OrderDetails = _context.OrderDetails.Where(od => od.OrderId == id).ToList();

            return Ok(order);
        }

        // Create a new order
        [HttpPost]
        public IActionResult CreateOrder([FromBody] Order order)
        {
            try
            {
                order.CreatedAt = DateTime.Now;
                order.UpdatedAt = DateTime.Now;
                _context.Orders.Add(order);
                _context.SaveChanges();
                return CreatedAtAction(nameof(GetOrderById), new { id = order.OrderId }, order);
            }
            catch (Exception ex)
            {
                return HandleForeignKeyException(ex);
            }
        }

        // Update order
        [HttpPut("{id}")]
        public IActionResult UpdateOrder(int id, [FromBody] Order updatedOrder)
        {
            try
            {
                var existingOrder = _context.Orders.FirstOrDefault(o => o.OrderId == id);
                if (existingOrder == null)
                {
                    return NotFound($"Order with ID {id} not found.");
                }

                existingOrder.TotalPrice = updatedOrder.TotalPrice;
                existingOrder.Status = updatedOrder.Status;
                existingOrder.CustomerId = updatedOrder.CustomerId;
                existingOrder.EmployeeId = updatedOrder.EmployeeId;
                existingOrder.UpdatedAt = DateTime.Now;

                _context.Orders.Update(existingOrder);
                _context.SaveChanges();

                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleForeignKeyException(ex);
            }
        }

        // Delete order (and its details)
        [HttpDelete("{id}")]
        public IActionResult DeleteOrder(int id)
        {
            try
            {
                var order = _context.Orders.FirstOrDefault(o => o.OrderId == id);
                if (order == null)
                {
                    return NotFound($"Order with ID {id} not found.");
                }

                var orderDetails = _context.OrderDetails.Where(od => od.OrderId == id).ToList();
                _context.OrderDetails.RemoveRange(orderDetails);
                _context.Orders.Remove(order);
                _context.SaveChanges();

                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleForeignKeyException(ex);
            }
        }

        // Update order status
        [HttpPatch("{id}/status")]
        public IActionResult UpdateOrderStatus(int id, [FromBody] int status)
        {
            var order = _context.Orders.FirstOrDefault(o => o.OrderId == id);
            if (order == null)
            {
                return NotFound($"Order with ID {id} not found.");
            }

            order.Status = status;
            order.UpdatedAt = DateTime.Now;

            _context.Orders.Update(order);
            _context.SaveChanges();

            return NoContent();
        }

        // Handle foreign key exceptions
        private IActionResult HandleForeignKeyException(Exception ex)
        {
            if (ex.InnerException != null && ex.InnerException.Message.Contains("FOREIGN KEY"))
            {
                if (ex.InnerException.Message.Contains("customers"))
                {
                    return BadRequest("Customer does not exist.");
                }
                if (ex.InnerException.Message.Contains("employees"))
                {
                    return BadRequest("Employee does not exist.");
                }
            }

            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

}
