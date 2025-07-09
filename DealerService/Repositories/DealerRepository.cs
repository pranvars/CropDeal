using Microsoft.EntityFrameworkCore;

namespace DealerService.Repositories
{
    public class DealerRepository : IDealerRepository
    {
        private readonly DealerDBContext _context;

        public DealerRepository(DealerDBContext context)
        {
            _context = context;
        }

        public async Task<Dealer> GetDealerByIdAsync(int dealerId)
        {
            return await _context.Dealers.FindAsync(dealerId);
        }

        public async Task<bool> UpdateDealerAsync(int dealerId, Dealer dealer)
        {
            var existingDealer = await _context.Dealers.FindAsync(dealerId);
            if (existingDealer == null) return false;

            existingDealer.UserId = dealer.UserId;
            existingDealer.Location = dealer.Location;
            existingDealer.AccountNumber = dealer.AccountNumber;
            existingDealer.BankIfsccode = dealer.BankIfsccode;

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateDealersStatusAsync(List<int> dealerIds, bool isActive)
        {
            var dealers = await _context.Dealers.Where(d => dealerIds.Contains(d.DealerId)).ToListAsync();
            if (dealers == null || dealers.Count == 0)
                return false;

            foreach (var dealer in dealers)
            {
                dealer.IsActive = isActive;
            }
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<List<Dealer>> GetAllDealersAsync()
        {
            return await _context.Dealers.ToListAsync();
        }
        public async Task<Dealer> AddDealer(Dealer dealer)
        {
            await _context.Dealers.AddAsync(new Dealer
            {
                UserId = dealer.UserId,
                Location = dealer.Location,
                AccountNumber = dealer.AccountNumber,
                BankIfsccode = dealer.BankIfsccode,
                IsActive = true
            });
            await _context.SaveChangesAsync();
            return dealer;
        }
        public async Task<int?> GetDealerIdByUserIdAsync(int userId)
        {
            var dealer = await _context.Dealers.FirstOrDefaultAsync(d => d.UserId == userId);
            return dealer?.DealerId;
        }
    }
}
