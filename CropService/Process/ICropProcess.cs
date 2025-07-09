namespace CropService.Process
{
    public interface ICropProcess
    {
        Task<IEnumerable<Crop>> GetAllCropsAsync();
        Task<Crop> GetCropByIdAsync(int id);
        Task<Crop> GetCropByNameAsync(string cropName);
        Task<Crop> CreateCropAsync(Crop crop);
        Task UpdateCropAsync(Crop crop);
        Task DeleteCropAsync(int id);
    }
}
