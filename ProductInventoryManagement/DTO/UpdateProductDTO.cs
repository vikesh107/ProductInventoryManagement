namespace ProductInventoryManagement.DTO
{
    public class UpdateProductDTO
    {
        public string SKU { get; set; }  // Unique identifier for products
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public List<Guid> CategoryIDs { get; set; }
    }
}
