namespace checkoutkata;

public class MultiBuyPromotion : IPromotion
{
    public required int Threshold { get; init; }
    public required string Sku { get; init; }
    public required int PromoPrice { get; init; }
    public int GetLineItemTotal(int quantity, int basePrice)
    {
        var numberOfPromotions = quantity / Threshold;
        var remainder = quantity % Threshold;
        return numberOfPromotions * PromoPrice + remainder * basePrice;
    }
}
