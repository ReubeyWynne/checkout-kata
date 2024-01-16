namespace checkoutkata;

public interface IProductRepo
{
    public Product? GetProductBySku(string sku);
}
