using CropService;

namespace DealerService.Process
{
    public interface IDealerProcess
    {
        Task<Dealer> GetDealerByIdAsync(int dealerId);
        Task<bool> UpdateDealerAsync(int dealerId, Dealer dealer);
        Task<List<Crop>> GetAllCropsAsync();
        Task<Crop> GetCropByNameAsync(string cropName);
        Task<Crop> GetCropByIdAsync(int cropId);
        Task<bool> UpdateDealersStatusAsync(List<int> dealerIds, bool isActive);
        Task<List<Dealer>> GetAllDealersAsync();
        Task<Dealer> RegisterDealer(Dealer dealer);
        Task<int?> GetDealerIdByUserIdAsync(int userId);
    }
}
