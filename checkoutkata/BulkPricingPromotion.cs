namespace checkoutkata;

public class BulkPricingPromotion : IPromotion
{
    public required string Sku { get; init; }
    public required Dictionary<int, int> BulkPricingTiers { get; init; }

    public int GetLineItemTotal(int quantity, int basePrice)
    {
        foreach(var tier in BulkPricingTiers.OrderByDescending(t => t.Key))
        {
            if (quantity >= tier.Key)
            {
                return tier.Value * quantity;
            }
        }
        return basePrice * quantity;
    }
}
