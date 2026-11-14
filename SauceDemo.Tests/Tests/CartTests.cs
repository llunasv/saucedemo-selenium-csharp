using SauceDemo.Tests.Core;
using SauceDemo.Tests.Data;
using SauceDemo.Tests.Pages;

namespace SauceDemo.Tests.Tests;

public class CartTests : BaseTest
{
    private InventoryPage LoginAsStandardUser()
    {
        new LoginPage(Driver).Login(Users.Standard, Users.Password);
        return new InventoryPage(Driver);
    }

    [Test]
    public void Cart_WhenEmpty_ShowsNoBadge()
    {
        var inventoryPage = LoginAsStandardUser();

        Assert.That(inventoryPage.IsCartBadgeVisible(), Is.False);
    }

    [Test]
    public void AddToCart_OneItem_ShowsBadgeCountOne()
    {
        var inventoryPage = LoginAsStandardUser();

        inventoryPage.AddToCart(Products.Backpack);

        Assert.That(inventoryPage.GetCartBadgeCount(), Is.EqualTo("1"));
    }

    [Test]
    public void AddToCart_TwoItems_ShowsBadgeCountTwo()
    {
        var inventoryPage = LoginAsStandardUser();

        inventoryPage.AddToCart(Products.Backpack);
        inventoryPage.AddToCart(Products.BikeLight);

        Assert.That(inventoryPage.GetCartBadgeCount(), Is.EqualTo("2"));
    }
}
