namespace checkoutkata;

public class ConcreteCheckout(IProductRepo _productRepo, IPromotionRepo _promotionRepo) : ICheckout
{

    public Dictionary<Product, int> _basket = [];
    public int GetTotalPrice()
    {
        var total = 0;
        foreach (var lineItem in _basket)
        {
            var product = lineItem.Key;
            var promo = _promotionRepo.GetPromotionBySku(product.Sku);

            total += (promo != null)
                ? promo.GetLineItemTotal(lineItem.Value, product.UnitPrice)
                : product.UnitPrice * lineItem.Value;
        }
        return total;
    }

    public void Scan(string sku)
    {
        var product = _productRepo.GetProductBySku(sku) ?? throw new ConcreteCheckoutScanInvalidProductException("Product not found");
        if (!_basket.ContainsKey(product)) _basket[product] = 0;
        _basket[product] += 1;
    }
}

public class ConcreteCheckoutScanInvalidProductException : Exception
{
    public ConcreteCheckoutScanInvalidProductException(string message) : base(message)
    {
    }
}
