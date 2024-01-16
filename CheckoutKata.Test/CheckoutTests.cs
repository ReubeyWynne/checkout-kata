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
        productRepo.GetProductBySku("A").Returns(new Product { Sku = "A", UnitPrice = 50 });
        productRepo.GetProductBySku("B").Returns(new Product { Sku = "B", UnitPrice = 30 });
        productRepo.GetProductBySku("C").Returns(new Product { Sku = "C", UnitPrice = 20 });
        productRepo.GetProductBySku("D").Returns(new Product { Sku = "D", UnitPrice = 15 });
        productRepo.GetProductBySku("E").Returns(new Product { Sku = "E", UnitPrice = 20 });

        var promoRepo = Substitute.For<IPromotionRepo>();
        promoRepo.GetPromotionBySku("A").Returns(new MultiBuyPromotion { Threshold = 3, Sku = "A", PromoPrice = 130 });
        promoRepo.GetPromotionBySku("B").Returns(new MultiBuyPromotion { Threshold = 2, Sku = "B", PromoPrice = 45 });
        promoRepo.GetPromotionBySku("C").Returns((IPromotion?)null);
        promoRepo.GetPromotionBySku("D").Returns((IPromotion?)null);
        promoRepo.GetPromotionBySku("E").Returns(new BulkPricingPromotion { BulkPricingTiers = new Dictionary<int, int> { { 5, 10 }, { 10, 5 } }, Sku = "E" });


        _checkout = new ConcreteCheckout(productRepo, promoRepo);

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
    {
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
    [InlineData((string[])(["A", "A", "A", "A"]), 180)]
    [InlineData((string[])(["B", "B", "B"]), 75)]
    public void Checkout_GetTotalPrice_MinimalPromotionPlusOne_ReturnsCorrectPrice(string[] items, int expectedPrice)
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
    [InlineData((string[])(["A", "A", "A", "B", "B", "C"]), 195)]
    [InlineData((string[])(["A", "A", "A", "B", "B", "C", "D"]), 210)]
    public void Checkout_GetTotalPrice_MultiplePromotionsWithOtherItems_ReturnsCorrectPrice(string[] items, int expectedPrice)
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
    [InlineData((string[])(["E", "E", "E", "E", "E"]), 50)]
    [InlineData((string[])(["E", "E", "E", "E", "E", "E"]), 60)]
    [InlineData((string[])(["E", "E", "E", "E", "E", "E", "E", "E", "E", "E"]), 50)]
    [InlineData((string[])(["E", "E", "E", "E", "E", "E", "E", "E", "E", "E", "E"]), 55)]
    public void Checkout_GetTotalPrice_BulkPricingPromotion_ReturnsCorrectPrice(string[] items, int expectedPrice)
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

    [Fact]
    public void Checkout_GetTotalPrice_BulkPricePromotion_QuanityBelowThreshold_ReturnsCorrectPrice()
    {
        // Act
        _checkout.Scan("E");
        var result = _checkout.GetTotalPrice();

        // Assert
        Assert.Equal(20, result);
    }

    [Theory]
    [InlineData((string[])(["A", "B", "C", "D", "E"]), 135)]
    public void Checkout_GetTotalPrice_MultipleDifferentItems_ReturnsCorrectPrice(string[] items, int expectedPrice)
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

    [Fact]
    public void Checkout_Scan_InvalidProduct_ThrowsException()
    {
        Assert.Throws<ConcreteCheckoutScanInvalidProductException>(() => _checkout.Scan("InvalidProduct"));
    }

    [Theory]
    [InlineData("C", 20)]
    [InlineData("D", 15)]
    public void Checkout_GetTotalPrice_ProductWithNoPromotion_ReturnsCorrectPrice(string item, int expectedPrice)
    {
        // Act
        _checkout.Scan(item);

        var result = _checkout.GetTotalPrice();

        // Assert
        Assert.Equal(expectedPrice, result);
    }
    [Theory]
    [InlineData("A", 50)]
    [InlineData("B", 30)]
    [InlineData("E", 20)]
    public void Checkout_GetTotalPrice_ProductWithPromotionBelowThreshold_ReturnsCorrectPrice(string item, int expectedPrice)
    {
        // Act
        _checkout.Scan(item);

        var result = _checkout.GetTotalPrice();

        // Assert
        Assert.Equal(expectedPrice, result);
    }

}

