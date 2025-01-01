using EIM.Attributes.FilterPipelines.Authorizations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nexus.Models;
using Nexus.Models.Enums;
using System;
using System.Drawing;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Nexus.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [RoleAccess(Role.Admin, Role.TechnicalEmployee)]
    public class PaymentController : BaseController
    {
        private readonly NexusDbContext _context;

        public PaymentController(NexusDbContext context)
        {
            _context = context;
        }

        // Get all payments
        [HttpGet]
        public IActionResult GetAllPayments(int page, int size, string? query)
        {
            var count = _context.Payments.AsQueryable().Query(query).Count();

            var payments = _context.Payments.AsQueryable().Query(query).Skip((page - 1) * size).Take(size).ToList();

            if (!payments.Any())
            {
                return NotFound("No payments found.");
            }

            return Ok(new { payments, count });

        }

        // Get payment by ID (include connections)
        [HttpGet("{id}")]
        public IActionResult GetPaymentById(int id)
        {
            var payment = _context.Payments.FirstOrDefault(p => p.PaymentId == id);

            if (payment == null)
            {
                return NotFound($"Payment with ID {id} not found.");
            }

            payment.Connections = _context.Connections.Where(c => c.PaymentId == id).ToList();

            return Ok(payment);
        }

        // Create a new payment
        [HttpPost]
        public IActionResult CreatePayment([FromBody] Payment payment)
        {
            try
            {
                payment.CreatedAt = DateTime.Now;
                payment.UpdatedAt = DateTime.Now;
                _context.Payments.Add(payment);
                _context.SaveChanges();
                return CreatedAtAction(nameof(GetPaymentById), new { id = payment.PaymentId }, payment);
            }
            catch (Exception ex)
            {
                return HandleForeignKeyException(ex);
            }
        }

        // Update payment
        [HttpPut("{id}")]
        public IActionResult UpdatePayment(int id, [FromBody] Payment updatedPayment)
        {
            try
            {
                var existingPayment = _context.Payments.FirstOrDefault(p => p.PaymentId == id);
                if (existingPayment == null)
                {
                    return NotFound($"Payment with ID {id} not found.");
                }

                existingPayment.Amount = updatedPayment.Amount;
                existingPayment.Description = updatedPayment.Description;
                existingPayment.OrderId = updatedPayment.OrderId;
                existingPayment.UpdatedAt = DateTime.Now;

                _context.Payments.Update(existingPayment);
                _context.SaveChanges();

                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleForeignKeyException(ex);
            }
        }

        // Delete payment (and its connections)
        [HttpDelete("{id}")]
        public IActionResult DeletePayment(int id)
        {
            try
            {
                var payment = _context.Payments.FirstOrDefault(p => p.PaymentId == id);
                if (payment == null)
                {
                    return NotFound($"Payment with ID {id} not found.");
                }

                var connections = _context.Connections.Where(c => c.PaymentId == id).ToList();
                _context.Connections.RemoveRange(connections);
                _context.Payments.Remove(payment);
                _context.SaveChanges();

                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleForeignKeyException(ex);
            }
        }

        // Handle foreign key exceptions
        private IActionResult HandleForeignKeyException(Exception ex)
        {
            if (ex.InnerException != null && ex.InnerException.Message.Contains("FOREIGN KEY"))
            {
                if (ex.InnerException.Message.Contains("orders"))
                {
                    return BadRequest("Order does not exist.");
                }
            }

            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

}
