public class InventoryAudit
{
    public Guid AuditID { get; set; }
    public Guid InventoryID { get; set; }
    public int AdjustedQuantity { get; set; }
    public DateTime AdjustmentDate { get; set; }
    public string Reason { get; set; }
    public string User { get; set; }
}
