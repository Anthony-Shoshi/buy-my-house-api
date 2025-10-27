using Azure;
using Azure.Data.Tables;
using BuyMyHouse.Domain.Entities;
using BuyMyHouse.Domain.Repositories;

namespace BuyMyHouse.Infrastructure.Storage;

public class TableService : ITableService
{
    private readonly TableClient _tableClient;

    public TableService(string connectionString)
    {
        _tableClient = new TableClient(connectionString, "IncomeRecords");
        _tableClient.CreateIfNotExists();
    }

    public async Task AddIncomeRecordAsync(string userId, decimal income)
    {
        var entity = new TableEntity(userId, Guid.NewGuid().ToString())
        {
            { "AnnualIncome", income },
            { "RecordedAt", DateTime.UtcNow }
        };
        await _tableClient.AddEntityAsync(entity);
    }

    public async Task<IEnumerable<IncomeRecordTableEntity>> GetIncomeRecordsAsync(string userId)
    {
        var query = _tableClient.QueryAsync<TableEntity>(e => e.PartitionKey == userId);
        var list = new List<IncomeRecordTableEntity>();

        await foreach (var entity in query)
        {
            list.Add(new IncomeRecordTableEntity
            {
                PartitionKey = entity.PartitionKey,
                RowKey = entity.RowKey,
                AnnualIncome = (decimal)entity["AnnualIncome"],
                RecordedAt = (DateTime)entity["RecordedAt"]
            });
        }

        return list;
    }
}