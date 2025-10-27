using BuyMyHouse.Domain.Entities;

namespace BuyMyHouse.Domain.Repositories;

public interface ITableService
{
    Task AddIncomeRecordAsync(string userId, decimal income);
    Task<IEnumerable<IncomeRecordTableEntity>> GetIncomeRecordsAsync(string userId);
}