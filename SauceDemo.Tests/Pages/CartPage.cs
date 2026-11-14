using OpenQA.Selenium;

namespace SauceDemo.Tests.Pages;

public class CartPage : BasePage
{
    private static readonly By CheckoutButton = By.CssSelector("[data-test='checkout']");

    public CartPage(IWebDriver driver) : base(driver) { }

    public void Checkout() => Click(CheckoutButton);
}
