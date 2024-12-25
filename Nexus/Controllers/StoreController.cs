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
    [RoleAccess(Role.Admin)]
    public class StoreController : BaseController
    {
        private readonly NexusDbContext _context;

        public StoreController(NexusDbContext context)
        {
            _context = context;
        }

        // Get all stores
        [HttpGet]
        public IActionResult GetAllStores()
        {
            var stores = _context.Stores.ToList();
            return Ok(stores);
        }

        // Get store by ID
        [HttpGet("{id}")]
        public IActionResult GetStoreById(int id)
        {
            var store = _context.Stores.Find(id);
            if (store == null)
            {
                return NotFound();
            }
            return Ok(store);
        }

        // Create a new store
        [HttpPost]
        public IActionResult CreateStore([FromBody] Store store)
        {
            store.CreatedAt = DateTime.Now;
            _context.Stores.Add(store);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {

            }
            return CreatedAtAction(nameof(GetStoreById), new { id = store.StoreId }, store);
        }

        // Update store
        [HttpPut("{id}")]
        public IActionResult UpdateStore(int id, [FromBody] Store updatedStore)
        {
            var store = _context.Stores.Find(id);
            if (store == null)
            {
                return NotFound();
            }

            store.StoreName = updatedStore.StoreName;
            store.Address = updatedStore.Address;
            store.PhoneNumber = updatedStore.PhoneNumber;
            store.UpdatedAt = DateTime.Now;
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {

            }

            return NoContent();
        }

        // Delete store
        [HttpDelete("{id}")]
        public IActionResult DeleteStore(int id)
        {
            var store = _context.Stores.Find(id);
            if (store == null)
            {
                return NotFound();
            }

            _context.Stores.Remove(store);
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
