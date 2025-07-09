
using Microsoft.EntityFrameworkCore;

namespace CropService.Repositories
{
    public class CropRepository : ICropRepository
    {
        private readonly CropDBContext _context;
        public CropRepository(CropDBContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Crop>> GetAllCropsAsync()
        {
            return await _context.Crops.ToListAsync();
        }
        public async Task<Crop?> GetCropByIdAsync(int cropId)
        {
            return await _context.Crops.FindAsync(cropId);
        }
        public async Task<Crop> GetCropByNameAsync(string cropName)
        {
            return await _context.Crops
                .Where(c => c.Name.ToLower() == cropName.ToLower())
                .FirstOrDefaultAsync();
        }
        public async Task CreateCropAsync(Crop crop)
        {
            await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT CropDB.dbo.Crops ON");
            await _context.Crops.AddAsync(crop);
            await _context.SaveChangesAsync();
            await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT CropDB.dbo.Crops OFF");
        }
        public async Task UpdateCropAsync(Crop crop)
        {
            _context.Crops.Update(crop);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteCropAsync(int cropId)
        {
            var crop = await _context.Crops.FindAsync(cropId);
            if (crop != null)
            {
                _context.Crops.Remove(crop);
                await _context.SaveChangesAsync();
            }
        }
    }
}
