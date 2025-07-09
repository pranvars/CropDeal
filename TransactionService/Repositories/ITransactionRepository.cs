namespace TransactionService.Repositories
{
    public interface ITransactionRepository
    {
        Task<bool> CreateTransactionAsync(Transaction transaction);
        Task<List<Transaction>> GetTransactionsByFarmerAndDateRangeAsync(int farmerId, DateTime startDate, DateTime endDate);
        Task<List<Transaction>> GetTransactionsByDealerAndDateRangeAsync(int dealerId, DateTime startDate, DateTime endDate);

        Task<Transaction?> GetTransactionByIdAsync(int transactionId);
    }
}
