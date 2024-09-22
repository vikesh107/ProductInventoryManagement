namespace ProductInventoryManagement.Models
{
    public class Product
    {
        public Guid ProductID { get; set; }
        public string SKU { get; set; }  // Unique identifier for products
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }

        // Navigation Properties
        public ICollection<Inventory> Inventories { get; set; }
        public ICollection<ProductCategory> ProductCategories { get; set; }
    }
}
