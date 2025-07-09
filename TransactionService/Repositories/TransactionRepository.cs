using Microsoft.EntityFrameworkCore;

namespace TransactionService.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly TransactionDBContext _context;

        public TransactionRepository(TransactionDBContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateTransactionAsync(Transaction transaction)
        {
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Transaction>> GetTransactionsByFarmerAndDateRangeAsync(int farmerId, DateTime startDate, DateTime endDate)
        {
            return await _context.Transactions
                .Where(t => t.FarmerId == farmerId && t.TransactionDate >= startDate && t.TransactionDate <= endDate)
                .ToListAsync();
        }

        public async Task<List<Transaction>> GetTransactionsByDealerAndDateRangeAsync(int dealerId, DateTime startDate, DateTime endDate)
        {
            return await _context.Transactions
                .Where(t => t.DealerId == dealerId && t.TransactionDate >= startDate && t.TransactionDate <= endDate)
                .ToListAsync();
        }

        public async Task<Transaction?> GetTransactionByIdAsync(int transactionId)
        {
            return await _context.Transactions.FindAsync(transactionId);
        }

    }
}
