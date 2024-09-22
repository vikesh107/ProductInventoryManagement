public class PriceAdjustmentDTO
{
    public List<Guid> ProductIDs { get; set; }   // List of Product IDs for bulk adjustment
    public decimal? Percentage { get; set; }      // Percentage decrease (e.g., 10% discount)
    public decimal? FixedAmount { get; set; }     // Fixed amount decrease (e.g., $5 off)
}
