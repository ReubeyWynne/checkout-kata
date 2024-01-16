namespace CheckoutKata.Test;

using checkoutkata;
using NSubstitute;

public class CheckoutTests
{
    private ICheckout _checkout;
    public CheckoutTests()
    {
        // Arrange
        var productRepo = Substitute.For<IProductRepo>();
        productRepo.GetProductBySku("A").Returns(new Product { Sku = "A", UnitPrice = 50, Promotion = new Promotion { Quantity = 3, Price = 130 } });
        productRepo.GetProductBySku("B").Returns(new Product { Sku = "B", UnitPrice = 30, Promotion = new Promotion { Quantity = 2, Price = 45 } });
        productRepo.GetProductBySku("C").Returns(new Product { Sku = "C", UnitPrice = 20 });
        productRepo.GetProductBySku("D").Returns(new Product { Sku = "D", UnitPrice = 15 });
        _checkout = new ConcreteCheckout(productRepo);
    }
    [Fact]
    public void Checkout_GetTotalPrice_NoItems_ReturnsZero()
    {

        // Act
        var result = _checkout.GetTotalPrice();

        // Assert
        Assert.Equal(0, result);
    }

    [Theory]
    [InlineData("A", 50)]
    [InlineData("B", 30)]
    [InlineData("C", 20)]
    [InlineData("D", 15)]
    public void Checkout_GetTotalPrice_OneItem_ReturnsCorrectPrice(string item, int expectedPrice)
    {;

        // Act
        _checkout.Scan(item);
        var result = _checkout.GetTotalPrice();

        // Assert
        Assert.Equal(expectedPrice, result);
    }

    [Theory]
    [InlineData((string[])(["A", "B"]), 80)]
    [InlineData((string[])(["A", "B", "C"]), 100)]
    [InlineData((string[])(["A", "B", "C", "D"]), 115)]
    public void Checkout_GetTotalPrice_MultipleItems_ReturnsCorrectPrice(string[] items, int expectedPrice)
    {

        // Act
        foreach (var item in items)
        {
            _checkout.Scan(item);
        }
        var result = _checkout.GetTotalPrice();

        // Assert
        Assert.Equal(expectedPrice, result);
    }

    [Theory]
    [InlineData((string[])(["A", "A", "A"]), 130)]
    [InlineData((string[])(["B", "B"]), 45)]
    public void Checkout_GetTotalPrice_MinimalPromotions_ReturnsCorrectPrice(string[] items, int expectedPrice)
    {;

        // Act
        foreach (var item in items)
        {
            _checkout.Scan(item);
        }
        var result = _checkout.GetTotalPrice();

        // Assert
        Assert.Equal(expectedPrice, result);
    }

    [Theory]
    [InlineData((string[])(["A", "A", "A", "A"]), 180)]
    [InlineData((string[])(["B", "B", "B"]), 75)]
    public void Checkout_GetTotalPrice_MinimalPromotionPlusOne_ReturnsCorrectPrice(string[] items, int expectedPrice)
    {;

        // Act
        foreach (var item in items)
        {
            _checkout.Scan(item);
        }
        var result = _checkout.GetTotalPrice();

        // Assert
        Assert.Equal(expectedPrice, result);
    }

    [Theory]
    [InlineData((string[])(["A", "A", "A", "B", "B", "C"]), 195)]
    [InlineData((string[])(["A", "A", "A", "B", "B", "C", "D"]), 210)]
    public void Checkout_GetTotalPrice_MultiplePromotionsWithOtherItems_ReturnsCorrectPrice(string[] items, int expectedPrice)
    {;

        // Act
        foreach (var item in items)
        {
            _checkout.Scan(item);
        }
        var result = _checkout.GetTotalPrice();

        // Assert
        Assert.Equal(expectedPrice, result);
    }

}