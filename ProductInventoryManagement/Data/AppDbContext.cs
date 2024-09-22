using Microsoft.EntityFrameworkCore;
using ProductInventoryManagement.Models;

namespace ProductInventoryManagement.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<InventoryTransaction> InventoryTransactions { get; set; }  
        public DbSet<InventoryAudit> InventoryAudits { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure many-to-many relationship between Product and Category
            modelBuilder.Entity<ProductCategory>()
                .HasKey(pc => new { pc.ProductID, pc.CategoryID });

            modelBuilder.Entity<ProductCategory>()
                .HasOne(pc => pc.Product)
                .WithMany(p => p.ProductCategories)
                .HasForeignKey(pc => pc.ProductID);

            modelBuilder.Entity<ProductCategory>()
                .HasOne(pc => pc.Category)
                .WithMany(c => c.ProductCategories)
                .HasForeignKey(pc => pc.CategoryID);

            modelBuilder.Entity<Product>()
               .Property(p => p.Price)
               .HasColumnType("decimal(18,2)"); // Specify precision and scale

            // Ensure SKU uniqueness
            modelBuilder.Entity<Product>()
                .HasIndex(p => p.SKU)
                .IsUnique();

            modelBuilder.Entity<InventoryTransaction>(entity =>
            {
                entity.HasKey(e => e.TransactionID);
                entity.Property(e => e.QuantityChanged).IsRequired();
                entity.Property(e => e.Timestamp).IsRequired();
                entity.Property(e => e.Reason).HasMaxLength(200);
                entity.Property(e => e.User).HasMaxLength(100);
            });

            // Configure InventoryAudit
            modelBuilder.Entity<InventoryAudit>(entity =>
            {
                entity.HasKey(e => e.AuditID);
                entity.Property(e => e.AdjustedQuantity).IsRequired();
                entity.Property(e => e.AdjustmentDate).IsRequired();
                entity.Property(e => e.Reason).HasMaxLength(200);
                entity.Property(e => e.User).HasMaxLength(100);
            });
        }
    }
}
