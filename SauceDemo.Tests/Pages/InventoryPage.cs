using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SauceDemo.Tests.Data;

namespace SauceDemo.Tests.Pages;

public class InventoryPage : BasePage
{
    private static readonly By CartBadge = By.CssSelector("[data-test='shopping-cart-badge']");
    private static readonly By CartLink = By.CssSelector("[data-test='shopping-cart-link']");
    private static readonly By InventoryList = By.CssSelector("[data-test='inventory-list']");
    private static readonly By ItemNames = By.CssSelector("[data-test='inventory-item-name']");
    private static readonly By ItemPrices = By.CssSelector("[data-test='inventory-item-price']");
    private static readonly By SortDropdown = By.CssSelector("[data-test='product-sort-container']");

    public InventoryPage(IWebDriver driver) : base(driver, InventoryList) { }

    public CartPage OpenCart()
    {
        Click(CartLink);
        return new CartPage(Driver);
    }

    public InventoryPage AddToCart(Product product)
    {
        Click(By.CssSelector($"[data-test='add-to-cart-{product.Id}']"));
        return this;
    }

    public InventoryPage RemoveFromCart(Product product)
    {
        var removeButton = By.CssSelector($"[data-test='remove-{product.Id}']");
        Click(removeButton);
        WaitUntilGone(removeButton);
        return this;
    }

    public InventoryPage SortBy(string option)
    {
        new SelectElement(Find(SortDropdown)).SelectByValue(option);
        return this;
    }

    public IReadOnlyList<string> GetNames() => GetTexts(ItemNames);

    public IReadOnlyList<decimal> GetPrices() =>
        GetTexts(ItemPrices).Select(ParsePrice).ToList();

    public string GetCartBadgeCount() => GetText(CartBadge);

    public bool IsCartBadgeVisible() => IsPresent(CartBadge);
}
