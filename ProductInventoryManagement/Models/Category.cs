namespace ProductInventoryManagement.Models
{
    public class Category
    {
        public Guid CategoryID { get; set; }
        public string Name { get; set; }

        // Navigation Properties
        public ICollection<ProductCategory> ProductCategories { get; set; }
    }
}
