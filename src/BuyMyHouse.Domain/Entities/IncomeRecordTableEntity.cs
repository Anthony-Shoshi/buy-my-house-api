namespace BuyMyHouse.Domain.Entities;

public class IncomeRecordTableEntity
{
    public string PartitionKey { get; set; } = null!;
    public string RowKey { get; set; } = null!;
    public decimal AnnualIncome { get; set; }
    public DateTime RecordedAt { get; set; }
}
