using SauceDemo.Tests.Core;
using SauceDemo.Tests.Data;

namespace SauceDemo.Tests.Tests;

public class CartTests : BaseTest
{
    [Test]
    public void Cart_WhenEmpty_ShowsNoBadge()
    {
        var inventoryPage = LoginAsStandardUser();

        Assert.That(inventoryPage.IsCartBadgeVisible(), Is.False);
    }

    [Test]
    public void AddToCart_OneItem_ShowsBadgeCountOne()
    {
        var inventoryPage = LoginAsStandardUser()
            .AddToCart(Products.Backpack);

        Assert.That(inventoryPage.GetCartBadgeCount(), Is.EqualTo("1"));
    }

    [Test]
    public void AddToCart_TwoItems_ShowsBadgeCountTwo()
    {
        var inventoryPage = LoginAsStandardUser()
            .AddToCart(Products.Backpack)
            .AddToCart(Products.BikeLight);

        Assert.That(inventoryPage.GetCartBadgeCount(), Is.EqualTo("2"));
    }
}
