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

    [Test]
    public void OpenCart_AfterAddingTwoItems_ShowsBothItems()
    {
        var cartPage = LoginAsStandardUser()
            .AddToCart(Products.Backpack)
            .AddToCart(Products.BikeLight)
            .OpenCart();

        Assert.That(cartPage.GetItemNames(), Is.EqualTo(new[] { Products.Backpack.Name, Products.BikeLight.Name }));
    }

    [Test]
    public void RemoveFromCart_OnInventoryPage_HidesBadge()
    {
        var inventoryPage = LoginAsStandardUser()
            .AddToCart(Products.Backpack)
            .RemoveFromCart(Products.Backpack);

        Assert.That(inventoryPage.IsCartBadgeVisible(), Is.False);
    }

    [Test]
    public void RemoveFromCart_OnCartPage_KeepsOtherItems()
    {
        var cartPage = LoginAsStandardUser()
            .AddToCart(Products.Backpack)
            .AddToCart(Products.BikeLight)
            .OpenCart()
            .RemoveFromCart(Products.Backpack);

        Assert.That(cartPage.GetItemNames(), Is.EqualTo(new[] { Products.BikeLight.Name }));
    }
}
