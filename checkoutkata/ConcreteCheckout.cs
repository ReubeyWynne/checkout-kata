namespace checkoutkata;

public class ConcreteCheckout : ICheckout
{
    private readonly Dictionary<string, Product> _products = new()
    {
        ["A"] = new Product { Sku = "A", UnitPrice = 50 },
        ["B"] = new Product { Sku = "B", UnitPrice = 30 },
        ["C"] = new Product { Sku = "C", UnitPrice = 20 },
        ["D"] = new Product { Sku = "D", UnitPrice = 15 },
    };
    public Dictionary<string, int> _items = [];
    public int GetTotalPrice()
    {
        var total = 0;
        foreach (var item in _items)
        {
            var product = _products[item.Key];
            total += product.UnitPrice * item.Value;
        }
        return total;
    }

    public void Scan(string sku)
    {
        if(!_products.ContainsKey(sku)) throw new ArgumentException("Invalid SKU");
        if (!_items.ContainsKey(sku)) _items[sku] =  0;
        _items[sku] += 1;
    }
}
