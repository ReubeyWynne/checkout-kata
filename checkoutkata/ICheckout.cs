namespace checkoutkata;

public interface ICheckout
{
    void Scan(string sku);
    int GetTotalPrice();
}

