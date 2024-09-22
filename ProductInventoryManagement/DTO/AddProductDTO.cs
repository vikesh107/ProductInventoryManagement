namespace ProductInventoryManagement.DTO
{
    public class AddProductDTO
    {
        public string SKU { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public List<Guid> catgeroyID { get; set; }
    }
}
