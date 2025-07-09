using Microsoft.EntityFrameworkCore;

namespace FarmerService.Repositories
{
    public class FarmerRepository : IFarmerRepository
    {
        private readonly FarmerDBContext _context;

        public FarmerRepository(FarmerDBContext context)
        {
            _context = context;
        }

        public async Task<Farmer> GetFarmerByIdAsync(int farmerId)
        {
            return await _context.Farmers.FindAsync(farmerId);
        }

        public async Task<bool> UpdateFarmerAsync(int farmerId, Farmer farmer)
        {
            var existingFarmer = await _context.Farmers.FindAsync(farmerId);
            if (existingFarmer == null) return false;

            existingFarmer.UserId = farmer.UserId;
            existingFarmer.Location = farmer.Location;
            existingFarmer.AccountNumber = farmer.AccountNumber;
            existingFarmer.BankIfsccode = farmer.BankIfsccode;

            await _context.SaveChangesAsync();
            return true;
        }

        //public async Task<bool> UpdateFarmersStatusAsync(bool isActive)
        //{
        //    //_context.Farmers.UpdateRange(farmers);
        //    //await _context.SaveChangesAsync();
        //    //return true;

        //    /*foreach(var item in farmers)
        //    {
        //        var existingFarmer = await _context.Farmers.FindAsync(item.FarmerId);
        //        if (existingFarmer == null) return false;

        //        existingFarmer.IsActive = item.IsActive;
        //        await _context.SaveChangesAsync();  
        //    }*/

        //    // Fetch all farmers who are currently active
        //    var farmers = await _context.Farmers.Where(f => f.IsActive == !isActive).ToListAsync();

        //    if (farmers == null || farmers.Count == 0)
        //        return false; // No farmers to update

        //    // Update the status
        //    foreach (var farmer in farmers)
        //    {
        //        farmer.IsActive = isActive;
        //    }

        //    // Save changes
        //    await _context.SaveChangesAsync();
        //    return true;

        //}
        public async Task<bool> UpdateFarmersStatusAsync(List<int> farmerIds, bool isActive)
        {
            var farmers = await _context.Farmers.Where(f => farmerIds.Contains(f.FarmerId)).ToListAsync();
            if (farmers == null || farmers.Count == 0)
                return false;

            foreach (var farmer in farmers)
            {
                farmer.IsActive = isActive;
            }
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<List<Farmer>> GetAllFarmersAsync()
        {
            return await _context.Farmers.ToListAsync();
        }
        public async Task<Farmer> AddFarmer(Farmer farmer)
        {
            await _context.Farmers.AddAsync(new Farmer
            {
                UserId = farmer.UserId,
                Location = farmer.Location,
                AccountNumber = farmer.AccountNumber,
                BankIfsccode = farmer.BankIfsccode,
                IsActive = true
            });
            await _context.SaveChangesAsync();
            return farmer;
        }
        public async Task<int?> GetFarmerIdByUserIdAsync(int userId)
        {
            var farmer = await _context.Farmers.FirstOrDefaultAsync(f => f.UserId == userId);
            return farmer?.FarmerId;
        }
    }
}
