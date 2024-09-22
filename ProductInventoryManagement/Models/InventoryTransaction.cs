public class InventoryTransaction
{
    public Guid TransactionID { get; set; }
    public Guid InventoryID { get; set; }
    public int QuantityChanged { get; set; }
    public DateTime Timestamp { get; set; }
    public string Reason { get; set; }
    public string User { get; set; }
}
