using OpenQA.Selenium;

namespace SauceDemo.Tests.Pages;

public class CheckoutOverviewPage : BasePage
{
    private static readonly By ItemPrices = By.CssSelector("[data-test='inventory-item-price']");
    private static readonly By ItemTotalLabel = By.CssSelector("[data-test='subtotal-label']");

    public CheckoutOverviewPage(IWebDriver driver) : base(driver, ItemTotalLabel) { }

    public IReadOnlyList<decimal> GetItemPrices() =>
        GetTexts(ItemPrices).Select(ParsePrice).ToList();

    public decimal GetItemTotal() => ParsePrice(GetText(ItemTotalLabel).Split('$')[1]);
}
