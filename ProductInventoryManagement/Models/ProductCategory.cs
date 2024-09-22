namespace ProductInventoryManagement.Models
{
    public class ProductCategory
    {
        public Guid ProductCategoryId { get; set; }
        public Guid ProductID { get; set; }
        public Product Product { get; set; }

        public Guid CategoryID { get; set; }
        public Category Category { get; set; }
    }
}
