namespace ProductInventoryManagement.DTO
{
    public class ProductsDTO
    {
        public Guid ProductID { get; set; }
        public string SKU { get; set; }  // Unique identifier for products
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public IEnumerable<string> CategoryName { get; set; }
    }
}
