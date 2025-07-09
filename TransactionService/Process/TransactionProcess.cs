using TransactionService.Repositories;

namespace TransactionService.Process
{
    public class TransactionProcess : ITransactionProcess
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionProcess(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<bool> CreateTransactionAsync(Transaction transaction)
        {
            return await _transactionRepository.CreateTransactionAsync(transaction);
        }

        public async Task<List<Transaction>> GetTransactionsByFarmerAndDateRangeAsync(int farmerId, DateTime startDate, DateTime endDate)
        {
            return await _transactionRepository.GetTransactionsByFarmerAndDateRangeAsync(farmerId, startDate, endDate);
        }

        public async Task<List<Transaction>> GetTransactionsByDealerAndDateRangeAsync(int dealerId, DateTime startDate, DateTime endDate)
        {
            return await _transactionRepository.GetTransactionsByDealerAndDateRangeAsync(dealerId, startDate, endDate);
        }
        public async Task<Transaction?> GetTransactionByIdAsync(int transactionId)
        {
            return await _transactionRepository.GetTransactionByIdAsync(transactionId);
        }

    }
}
