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
    [RoleAccess(Role.Accountant)]
    public class PaymentController : BaseController
    {
        private readonly NexusDbContext _context;

        public PaymentController(NexusDbContext context)
        {
            _context = context;
        }

        // Get all payments
        [HttpGet]
        public IActionResult GetAllPayments()
        {
            var payments = _context.Payments.ToList();
            return Ok(payments);
        }

        // Get payment by ID
        [HttpGet("{id}")]
        public IActionResult GetPaymentById(int id)
        {
            var payment = _context.Payments.FirstOrDefault(p => p.PaymentId == id);
            if (payment == null)
            {
                return NotFound();
            }
            return Ok(payment);
        }

        // Create a new payment
        [HttpPost]
        public IActionResult CreatePayment([FromBody] Payment payment)
        {
            var order = _context.Orders.Find(payment.OrderId);
            if (order == null)
            {
                return BadRequest("Order does not exist.");
            }

            _context.Payments.Add(payment);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetPaymentById), new { id = payment.PaymentId }, payment);
        }

        // Update payment
        [HttpPut("{id}")]
        public IActionResult UpdatePayment(int id, [FromBody] Payment updatedPayment)
        {
            var payment = _context.Payments.Find(id);
            if (payment == null)
            {
                return NotFound();
            }

            payment.OrderId = updatedPayment.OrderId;
            payment.PaymentDate = updatedPayment.PaymentDate;
            payment.Amount = updatedPayment.Amount;
            payment.PaymentMethod = updatedPayment.PaymentMethod;
            payment.Status = updatedPayment.Status;
            _context.SaveChanges();

            return NoContent();
        }

        // Delete payment
        [HttpDelete("{id}")]
        public IActionResult DeletePayment(int id)
        {
            var payment = _context.Payments.Find(id);
            if (payment == null)
            {
                return NotFound();
            }

            _context.Payments.Remove(payment);
            _context.SaveChanges();

            return NoContent();
        }
    }

}
