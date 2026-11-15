using OpenQA.Selenium;
using SauceDemo.Tests.Data;

namespace SauceDemo.Tests.Pages;

public class CartPage : BasePage
{
    private static readonly By CheckoutButton = By.CssSelector("[data-test='checkout']");
    private static readonly By CartList = By.CssSelector("[data-test='cart-list']");
    private static readonly By ItemNames = By.CssSelector("[data-test='inventory-item-name']");

    public CartPage(IWebDriver driver) : base(driver, CartList) { }

    public CheckoutInformationPage Checkout()
    {
        Click(CheckoutButton);
        return new CheckoutInformationPage(Driver);
    }

    public CartPage RemoveFromCart(Product product)
    {
        var removeButton = By.CssSelector($"[data-test='remove-{product.Id}']");
        Click(removeButton);
        WaitUntilGone(removeButton);
        return this;
    }

    public IReadOnlyList<string> GetItemNames() => GetTexts(ItemNames);
}
