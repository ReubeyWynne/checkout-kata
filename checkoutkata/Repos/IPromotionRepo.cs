namespace checkoutkata;

public interface IPromotionRepo
{
    public IPromotion? GetPromotionBySku(string sku);
}
