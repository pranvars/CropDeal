namespace CropService.Repositories
{
    public interface ICropRepository
    {
        Task<IEnumerable<Crop>> GetAllCropsAsync();
        Task<Crop?> GetCropByIdAsync(int cropId);
        Task<Crop> GetCropByNameAsync(string cropName);
        Task CreateCropAsync(Crop crop);
        Task UpdateCropAsync(Crop crop);
        Task DeleteCropAsync(int cropId);
    }
}
