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
            payment.CreatedAt = DateTime.Now;
            var order = _context.Customers.Find(payment.CustomerId);
            if (order == null)
            {
                return BadRequest("Customer does not exist.");
            }

            _context.Payments.Add(payment);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {

            }

            return CreatedAtAction(nameof(GetPaymentById), new { id = payment.PaymentId }, payment);
        }

        // Update payment
        [HttpPut("{id}")]
        public IActionResult UpdatePayment(int id, [FromBody] Payment updatedPayment)
        {
            updatedPayment.UpdatedAt = DateTime.Now;

            var payment = _context.Payments.Find(id);
            if (payment == null)
            {
                return NotFound();
            }

            payment.CustomerId = updatedPayment.CustomerId;
            payment.Amount = updatedPayment.Amount;
            payment.UpdatedAt = DateTime.Now;
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {

            }

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
