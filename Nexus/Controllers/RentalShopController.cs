using EIM.Attributes.FilterPipelines.Authorizations;
using Microsoft.AspNetCore.Mvc;
using Nexus.Models;
using Nexus.Models.Enums;
using Nexus.Services;

namespace Nexus.Controllers
{
    [Route("api/RentalShop")]
    [ApiController]
    public class RentalShopController : BaseController
    {
        private readonly IRentalShopService _rentalShopService;

        public RentalShopController(IRentalShopService rentalShopService)
        {
            _rentalShopService = rentalShopService;
        }

        // Action: Get All Rental Shops - Không cần quyền Admin
        [HttpGet]
        public IActionResult GetAllRentalShops()
        {
            var rentalShops = _rentalShopService.GetAllRentalShops();
            return Ok(rentalShops);
        }

        // Action: Get Rental Shop by Id - Không cần quyền Admin
        [HttpGet("{id}")]
        public IActionResult GetRentalShopById(int id)
        {
            var rentalShop = _rentalShopService.GetRentalShopById(id);
            if (rentalShop == null)
            {
                return NotFound();
            }

            return Ok(rentalShop);
        }

        // Action: Add Rental Shop - Chỉ có quyền Admin
        [HttpPost]
        [EmployeeTypeAccess(EmployeeType.Admin)] // Cần quyền Admin
        public IActionResult AddRentalShop([FromBody] RentalShop rentalShop)
        {
            _rentalShopService.AddRentalShop(rentalShop);
            return CreatedAtAction(nameof(GetRentalShopById), new { id = rentalShop.RentalShopId }, rentalShop);
        }

        // Action: Update Rental Shop - Chỉ có quyền Admin
        [HttpPut("{id}")]
        [EmployeeTypeAccess(EmployeeType.Admin)] // Cần quyền Admin
        public IActionResult UpdateRentalShop(int id, [FromBody] RentalShop rentalShop)
        {
            if (id != rentalShop.RentalShopId)
            {
                return BadRequest();
            }

            _rentalShopService.UpdateRentalShop(rentalShop);
            return NoContent();
        }

        // Action: Delete Rental Shop - Chỉ có quyền Admin
        [HttpDelete("{id}")]
        [EmployeeTypeAccess(EmployeeType.Admin)] // Cần quyền Admin
        public IActionResult DeleteRentalShop(int id)
        {
            _rentalShopService.DeleteRentalShop(id);
            return NoContent();
        }
    }
}
