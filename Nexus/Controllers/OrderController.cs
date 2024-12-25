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
            // Find the order
            var order = _context.Orders.FirstOrDefault(o => o.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            // Manually load related Customer
            var customer = _context.Customers.FirstOrDefault(c => c.CustomerId == order.CustomerId);

            // Manually load OrderDetails
            var orderDetails = _context.OrderDetails
                .Where(od => od.OrderId == order.OrderId)
                .ToList();

            // Manually load related packages for each order detail
            foreach (var detail in orderDetails)
            {
                detail.Package = _context.Packages.FirstOrDefault(p => p.PackageId == detail.PackageId);
            }

            // Construct the result object with included relations
            var result = new
            {
                Order = order,
                Customer = customer,
                OrderDetails = orderDetails
            };

            return Ok(result);
        }


        // Create a new order
        [HttpPost]
        public IActionResult CreateOrder([FromBody] Order order)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                // Set the creation timestamp
                order.CreatedAt = DateTime.Now;

                // Check if the customer exists
                var customer = _context.Customers.Find(order.CustomerId);
                if (customer == null)
                {
                    return BadRequest("Customer does not exist.");
                }

                var store = _context.Stores.Find(order.StoreId);
                if (store == null)
                {
                    return BadRequest("Store does not exist.");
                }

                // Calculate the total price
                order.TotalPrice = order.OrderDetails?.Sum(od => od.Quantity * od.Price) ?? 0;

                // Add the order to the database
                _context.Orders.Add(order);
                _context.SaveChanges(); // Save to generate the OrderId

                // Handle OrderDetails if provided
                if (order.OrderDetails != null && order.OrderDetails.Any())
                {
                    foreach (var detail in order.OrderDetails)
                    {
                        // Check if the package exists
                        var package = _context.Packages.Find(detail.PackageId);
                        if (package == null)
                        {
                            transaction.Rollback();
                            return BadRequest($"Package with ID {detail.PackageId} does not exist.");
                        }

                        detail.OrderId = order.OrderId; // Link the detail to the order
                        _context.OrderDetails.Add(detail);
                    }
                }

                // Save all changes
                _context.SaveChanges();

                // Commit the transaction
                transaction.Commit();

                // Return the created order
                return CreatedAtAction(nameof(GetOrderById), new { id = order.OrderId }, order);
            }
            catch (Exception ex)
            {
                // Rollback the transaction in case of an error
                transaction.Rollback();
                return StatusCode(500, "An error occurred while saving the order: " + ex.Message);
            }
        }

        [HttpPatch("{id}/status")]
        public IActionResult UpdateOrderStatus(int id, [FromBody] int newStatus)
        {
            try
            {
                // Find the order
                var order = _context.Orders.Find(id);
                if (order == null)
                {
                    return NotFound("Order does not exist.");
                }

                // Update the status
                order.Status = newStatus;
                order.UpdatedAt = DateTime.Now;

                // Save changes to the database
                _context.SaveChanges();

                return NoContent(); // Return 204 No Content
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating the status: " + ex.Message);
            }
        }


        [HttpPut("{id}")]
        public IActionResult UpdateOrder(int id, [FromBody] Order updatedOrder)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                // Find the existing order
                var order = _context.Orders.Find(id);
                if (order == null)
                {
                    return NotFound("Order does not exist.");
                }

                // Check if the customer exists
                var customer = _context.Customers.Find(updatedOrder.CustomerId);
                if (customer == null)
                {
                    return BadRequest("Customer does not exist.");
                }

                // Check if the store exists
                var store = _context.Stores.Find(updatedOrder.StoreId);
                if (store == null)
                {
                    return BadRequest("Store does not exist.");
                }

                // Update the order details
                order.CustomerId = updatedOrder.CustomerId;
                order.StoreId = updatedOrder.StoreId;
                order.Status = updatedOrder.Status;
                order.UpdatedAt = DateTime.Now;

                // Delete existing OrderDetails
                var existingOrderDetails = _context.OrderDetails.Where(od => od.OrderId == id).ToList();
                _context.OrderDetails.RemoveRange(existingOrderDetails);

                // Insert new OrderDetails
                if (updatedOrder.OrderDetails != null && updatedOrder.OrderDetails.Any())
                {
                    foreach (var detail in updatedOrder.OrderDetails)
                    {
                        // Check if the package exists
                        var package = _context.Packages.Find(detail.PackageId);
                        if (package == null)
                        {
                            transaction.Rollback();
                            return BadRequest($"Package with ID {detail.PackageId} does not exist.");
                        }

                        detail.OrderId = order.OrderId; // Link the detail to the order
                        _context.OrderDetails.Add(detail);
                    }
                }

                // Calculate the total price
                order.TotalPrice = updatedOrder.OrderDetails?.Sum(od => od.Quantity * od.Price) ?? 0;

                // Save all changes
                _context.SaveChanges();

                // Commit the transaction
                transaction.Commit();

                return NoContent();
            }
            catch (Exception ex)
            {
                // Rollback the transaction in case of an error
                transaction.Rollback();
                return StatusCode(500, "An error occurred while updating the order: " + ex.Message);
            }
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteOrder(int id)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                // Find the order
                var order = _context.Orders.Find(id);
                if (order == null)
                {
                    return NotFound("Order does not exist.");
                }

                // Find and delete related OrderDetails
                var orderDetails = _context.OrderDetails.Where(od => od.OrderId == id).ToList();
                if (orderDetails.Any())
                {
                    _context.OrderDetails.RemoveRange(orderDetails);
                }

                // Delete the order
                _context.Orders.Remove(order);

                // Save changes
                _context.SaveChanges();

                // Commit the transaction
                transaction.Commit();

                return NoContent();
            }
            catch (Exception ex)
            {
                // Rollback the transaction in case of an error
                transaction.Rollback();
                return StatusCode(500, "An error occurred while deleting the order: " + ex.Message);
            }
        }

    }

}
