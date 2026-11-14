using OpenQA.Selenium;

namespace SauceDemo.Tests.Pages;

public class InventoryPage : BasePage
{
    private static readonly By CartBadge = By.CssSelector("[data-test='shopping-cart-badge']");
    private static readonly By CartLink = By.CssSelector("[data-test='shopping-cart-link']");
    private static readonly By InventoryList = By.CssSelector("[data-test='inventory-list']");

    public InventoryPage(IWebDriver driver) : base(driver, InventoryList) { }

    public void OpenCart() => Click(CartLink);

    public void AddToCart(string product) =>
        Click(By.CssSelector($"[data-test='add-to-cart-{product}']"));

    public string GetCartBadgeCount() => GetText(CartBadge);

    public bool IsCartBadgeVisible() => IsPresent(CartBadge);
}
