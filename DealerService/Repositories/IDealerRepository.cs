using FarmerService;

namespace DealerService.Repositories
{
    public interface IDealerRepository
    {
        Task<Dealer> GetDealerByIdAsync(int dealerId);
        Task<bool> UpdateDealerAsync(int dealerId, Dealer dealer);
        Task<bool> UpdateDealersStatusAsync(List<int> dealerIds, bool isActive);
        Task<List<Dealer>> GetAllDealersAsync();
        Task<Dealer> AddDealer(Dealer dealer);
        Task<int?> GetDealerIdByUserIdAsync(int userId);
    }
}
