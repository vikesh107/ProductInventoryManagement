namespace ProductInventoryManagement.DTOs
{
    public class InventoryDTO
    {
        public Guid InventoryID { get; set; }
        public Guid ProductID { get; set; }
        public int Quantity { get; set; }
        public string WarehouseLocation { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
