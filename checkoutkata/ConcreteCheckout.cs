namespace checkoutkata;

public class ConcreteCheckout(IProductRepo ProductRepo) : ICheckout
{
    
    public Dictionary<Product, int> _basket = [];
    public int GetTotalPrice()
    {
        var total = 0;
        foreach (var lineItem in _basket)
        {
            var product = lineItem.Key; 
            if (product.Promotion == null || lineItem.Value < product.Promotion.Quantity)
            {
                total += product.UnitPrice * lineItem.Value;
                continue;
            }
            var promotion = product.Promotion;
            var quantity = lineItem.Value;
            var price = promotion.Price;
            var numberOfPromotions = quantity / promotion.Quantity;
            var remainder = quantity % promotion.Quantity;
            total += numberOfPromotions * price + remainder * product.UnitPrice;
        }
        return total;
    }

    public void Scan(string sku)
    {
        var product = ProductRepo.GetProductBySku(sku) ?? throw new ConcreteCheckoutScanInvalidProductException("Product not found");
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
