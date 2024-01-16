namespace checkoutkata;

public class ConcreteCheckout : ICheckout
{
    private readonly Dictionary<string, Product> _products = new()
    {
        ["A"] = new Product { Sku = "A", UnitPrice = 50, Promotion = new Promotion { Quantity = 3, Price = 130 } },
        ["B"] = new Product { Sku = "B", UnitPrice = 30, Promotion = new Promotion { Quantity = 2, Price = 45 } },
        ["C"] = new Product { Sku = "C", UnitPrice = 20 },
        ["D"] = new Product { Sku = "D", UnitPrice = 15 },
    };
    public Dictionary<string, int> _basket = [];
    public int GetTotalPrice()
    {
        var total = 0;
        foreach (var lineItem in _basket)
        {
            var product = _products[lineItem.Key];
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
        if (!_products.ContainsKey(sku)) throw new ArgumentException("Invalid SKU");
        if (!_basket.ContainsKey(sku)) _basket[sku] = 0;
        _basket[sku] += 1;
    }
}
