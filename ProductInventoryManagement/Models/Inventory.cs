using System;

namespace ProductInventoryManagement.Models
{
    public class Inventory
    {
        public Guid InventoryID { get; set; }
        public Guid ProductID { get; set; }
        public int Quantity { get; set; }
        public string WarehouseLocation { get; set; }
        public DateTime LastUpdated { get; set; }

        // Navigation property
        public virtual Product Product { get; set; }
    }
}
