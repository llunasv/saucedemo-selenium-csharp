using OpenQA.Selenium;

namespace SauceDemo.Tests.Pages;

public class CartPage : BasePage
{
    private static readonly By CheckoutButton = By.CssSelector("[data-test='checkout']");
    private static readonly By CartList = By.CssSelector("[data-test='cart-list']");

    public CartPage(IWebDriver driver) : base(driver, CartList) { }

    public CheckoutInformationPage Checkout()
    {
        Click(CheckoutButton);
        return new CheckoutInformationPage(Driver);
    }
}
