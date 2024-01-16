namespace CheckoutKata.Test;

using checkoutkata;

public class UnitTest1
{
    [Fact]
    public void Checkout_GetTotalPrice_NoItems_ReturnsZero()
    {
        // Arrange
        var checkout = new ConcreteCheckout();

        // Act
        var result = checkout.GetTotalPrice();

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
        // Arrange
        var checkout = new ConcreteCheckout();

        // Act
        checkout.Scan(item);
        var result = checkout.GetTotalPrice();

        // Assert
        Assert.Equal(expectedPrice, result);
    }

    [Theory]
    [InlineData((string[])(["A", "B"]), 80)]
    [InlineData((string[])(["A", "B", "C"]), 100)]
    [InlineData((string[])(["A", "B", "C", "D"]), 115)]
    public void Checkout_GetTotalPrice_MultipleItems_ReturnsCorrectPrice(string[] items, int expectedPrice)
    {
        // Arrange
        var checkout = new ConcreteCheckout();

        // Act
        foreach (var item in items)
        {
            checkout.Scan(item);
        }
        var result = checkout.GetTotalPrice();

        // Assert
        Assert.Equal(expectedPrice, result);
    }
}