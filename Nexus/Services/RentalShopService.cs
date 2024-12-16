using Nexus.Models;

namespace Nexus.Services
{
    public class RentalShopService : IRentalShopService
    {
        private readonly NexusDbContext _context;

        public RentalShopService(NexusDbContext context)
        {
            _context = context;
        }

        public RentalShop GetRentalShopById(int id)
        {
            return _context.RentalShops.Find(id);
        }

        public IEnumerable<RentalShop> GetAllRentalShops()
        {
            return _context.RentalShops.ToList();
        }

        public void AddRentalShop(RentalShop rentalShop)
        {
            _context.RentalShops.Add(rentalShop);
            _context.SaveChanges();
        }

        public void UpdateRentalShop(RentalShop rentalShop)
        {
            _context.RentalShops.Update(rentalShop);
            _context.SaveChanges();
        }

        public void DeleteRentalShop(int id)
        {
            var rentalShop = _context.RentalShops.Find(id);
            if (rentalShop != null)
            {
                _context.RentalShops.Remove(rentalShop);
                _context.SaveChanges();
            }
        }
    }
}
