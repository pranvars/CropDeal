namespace FarmerService.Repositories
{
    public interface IFarmerRepository
    {
        Task<Farmer> GetFarmerByIdAsync(int farmerId);
        Task<bool> UpdateFarmerAsync(int farmerId, Farmer farmer);

        //Task<bool> UpdateFarmersStatusAsync(List<Farmer> farmers);
        //Task<bool> UpdateFarmersStatusAsync(bool isActive);
        Task<bool> UpdateFarmersStatusAsync(List<int> farmerIds, bool isActive);
        Task<List<Farmer>> GetAllFarmersAsync();

        Task<Farmer> AddFarmer(Farmer farmer);
        Task<int?> GetFarmerIdByUserIdAsync(int userId);
    }
}
