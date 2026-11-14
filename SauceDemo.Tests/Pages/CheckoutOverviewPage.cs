using System.Globalization;
using OpenQA.Selenium;

namespace SauceDemo.Tests.Pages;

public class CheckoutOverviewPage : BasePage
{
    private static readonly By ItemPrices = By.CssSelector("[data-test='inventory-item-price']");
    private static readonly By ItemTotalLabel = By.CssSelector("[data-test='subtotal-label']");

    public CheckoutOverviewPage(IWebDriver driver) : base(driver) { }

    public IReadOnlyList<decimal> GetItemPrices() =>
        GetTexts(ItemPrices).Select(ParsePrice).ToList();

    public decimal GetItemTotal() => ParsePrice(GetText(ItemTotalLabel).Split('$')[1]);

    // "$29.99" -> 29.99; InvariantCulture so "." is the decimal separator on any PC locale
    private static decimal ParsePrice(string text) =>
        decimal.Parse(text.TrimStart('$'), CultureInfo.InvariantCulture);
}
