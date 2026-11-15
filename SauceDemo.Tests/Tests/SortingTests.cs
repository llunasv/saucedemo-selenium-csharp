using SauceDemo.Tests.Core;
using SauceDemo.Tests.Data;

namespace SauceDemo.Tests.Tests;

public class SortingTests : BaseTest
{
    [Test]
    public void SortByName_AToZ_ProductsAreOrderedAlphabetically()
    {
        // A→Z is the default order, so sort Z→A first – otherwise this passes even if sorting is broken
        var inventoryPage = LoginAsStandardUser()
            .SortBy(SortOptions.NameZToA)
            .SortBy(SortOptions.NameAToZ);

        Assert.That(inventoryPage.GetNames(), Is.Ordered.Ascending);
    }

    [Test]
    public void SortByName_ZToA_ProductsAreOrderedReverseAlphabetically()
    {
        var inventoryPage = LoginAsStandardUser()
            .SortBy(SortOptions.NameZToA);

        Assert.That(inventoryPage.GetNames(), Is.Ordered.Descending);
    }

    [Test]
    public void SortByPrice_LowToHigh_ProductsAreOrderedAscending()
    {
        var inventoryPage = LoginAsStandardUser()
            .SortBy(SortOptions.PriceLowToHigh);

        Assert.That(inventoryPage.GetPrices(), Is.Ordered.Ascending);
    }

    [Test]
    public void SortByPrice_HighToLow_ProductsAreOrderedDescending()
    {
        var inventoryPage = LoginAsStandardUser()
            .SortBy(SortOptions.PriceHighToLow);

        Assert.That(inventoryPage.GetPrices(), Is.Ordered.Descending);
    }
}
