using OpenQA.Selenium;

namespace SauceDemo.Tests.Pages;

public class InventoryPage : BasePage
{
    private static readonly By CartBadge = By.CssSelector("[data-test='shopping-cart-badge']");

    public InventoryPage(IWebDriver driver) : base(driver) { }

    public void AddToCart(string product) =>
        Click(By.CssSelector($"[data-test='add-to-cart-{product}']"));

    public string GetCartBadgeCount() => GetText(CartBadge);

    public bool IsCartBadgeVisible() => IsPresent(CartBadge);
}
