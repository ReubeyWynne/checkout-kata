namespace CheckoutKata.Test;

using Xunit;
using checkoutkata;
using System.Collections.Generic;

public class BulkPricingPromotionTests
{
    [Fact]
    public void GetLineItemTotal_ReturnsCorrectTotal_WhenQuantityMatchesTier()
    {
        // Arrange
        var promotion = new BulkPricingPromotion
        {
            Sku = "E",
            BulkPricingTiers = new Dictionary<int, int> { { 5, 10 } }
        };

        // Act
        var total = promotion.GetLineItemTotal(5, 20);

        // Assert
        Assert.Equal(50, total);
    }

    [Fact]
    public void GetLineItemTotal_ReturnsBasePriceTotal_WhenQuantityDoesNotMatchTier()
    {
        // Arrange
        var promotion = new BulkPricingPromotion
        {
            Sku = "E",
            BulkPricingTiers = new Dictionary<int, int> { { 5, 10 } }
        };

        // Act
        var total = promotion.GetLineItemTotal(4, 20);

        // Assert
        Assert.Equal(80, total);
    }

    [Fact]
    public void GetLineItemTotal_ReturnsHighestTierTotal_WhenQuantityMatchesMultipleTiers()
    {
        // Arrange
        var promotion = new BulkPricingPromotion
        {
            Sku = "E",
            BulkPricingTiers = new Dictionary<int, int> { { 5, 10 }, { 10, 8 } }
        };

        // Act
        var total = promotion.GetLineItemTotal(10, 20);

        // Assert
        Assert.Equal(80, total);
    }
}