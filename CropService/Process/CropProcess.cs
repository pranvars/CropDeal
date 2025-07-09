using CropService.Repositories;

namespace CropService.Process
{
    public class CropProcess : ICropProcess
    {
        private readonly ICropRepository _repository;

        public CropProcess(ICropRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Crop>> GetAllCropsAsync()
        {
            var crops = await _repository.GetAllCropsAsync();
            return crops ?? new List<Crop>(); // Return empty list instead of null
        }

        public async Task<Crop> GetCropByIdAsync(int id)
        {
            var crop = await _repository.GetCropByIdAsync(id);
            if (crop == null) throw new KeyNotFoundException("Crop not found.");
            return crop;
        }

        public async Task<Crop> GetCropByNameAsync(string cropName)
        {
            if (string.IsNullOrWhiteSpace(cropName)) 
                throw new ArgumentException("Crop name cannot be empty.", nameof(cropName));

            var crop = await _repository.GetCropByNameAsync(cropName);
            if (crop == null) throw new KeyNotFoundException("Crop not found.");
            return crop;
        }

        public async Task<Crop> CreateCropAsync(Crop crop)
        {
            if (crop == null) throw new ArgumentNullException(nameof(crop), "Crop data cannot be null.");
            await _repository.CreateCropAsync(crop);
            return crop;
        }

        public async Task UpdateCropAsync(Crop crop)
        {
            if (crop == null) throw new ArgumentNullException(nameof(crop), "Crop data cannot be null.");
            var existingCrop = await _repository.GetCropByIdAsync(crop.CropId);
            if (existingCrop == null) throw new KeyNotFoundException("Crop not found.");

            await _repository.UpdateCropAsync(crop);
        }

        public async Task DeleteCropAsync(int id)
        {
            var existingCrop = await _repository.GetCropByIdAsync(id);
            if (existingCrop == null) throw new KeyNotFoundException("Crop not found.");

            await _repository.DeleteCropAsync(id);
        }
    }
}
