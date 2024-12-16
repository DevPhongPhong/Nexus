using Nexus.Models;

namespace Nexus.Services
{
    public interface IRentalShopService
    {
        RentalShop GetRentalShopById(int id);
        IEnumerable<RentalShop> GetAllRentalShops();
        void AddRentalShop(RentalShop rentalShop);
        void UpdateRentalShop(RentalShop rentalShop);
        void DeleteRentalShop(int id);
    }

}
