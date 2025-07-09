using CropService;

namespace FarmerService.Process
{
    public interface IFarmerProcess
    {
        Task<Farmer> GetFarmerByIdAsync(int farmerId);
        Task<bool> UpdateFarmerAsync(int farmerId, Farmer farmer);
        Task<bool> PublishCropAsync(Crop crop);
        //Task<bool> UpdateFarmersStatusAsync(List<Farmer> farmers);
        //Task<bool> UpdateFarmersStatusAsync(bool isActive);
        Task<bool> UpdateFarmersStatusAsync(List<int> farmerIds, bool isActive);

        Task<List<Farmer>> GetAllFarmersAsync();
        Task<Farmer> RegisterFarmer(Farmer farmer);
        Task<int?> GetFarmerIdByUserIdAsync(int userId);
    }
}
