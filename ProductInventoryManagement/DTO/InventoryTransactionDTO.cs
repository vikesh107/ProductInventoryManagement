using System;

namespace ProductInventoryManagement.Models
{
    public class InventoryTransactionDTO
    {
        public Guid InventoryID { get; set; }
        public int QuantityChanged { get; set; }
        public string Reason { get; set; }
        public string UserId { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
