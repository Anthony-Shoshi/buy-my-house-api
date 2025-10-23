using Azure;
using Azure.Data.Tables;

namespace BuyMyHouse.Infrastructure.Storage;

public class TableService
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

    public async Task<IEnumerable<TableEntity>> GetIncomeRecordsAsync(string userId)
    {
        var query = _tableClient.QueryAsync<TableEntity>(e => e.PartitionKey == userId);
        var list = new List<TableEntity>();
        await foreach (var entity in query)
            list.Add(entity);
        return list;
    }
}