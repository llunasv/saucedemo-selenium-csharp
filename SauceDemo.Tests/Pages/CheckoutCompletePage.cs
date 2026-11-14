using OpenQA.Selenium;

namespace SauceDemo.Tests.Pages;

public class CheckoutCompletePage : BasePage
{
    private static readonly By BackHomeButton = By.CssSelector("[data-test='back-to-products']");
    private static readonly By CompleteHeader = By.CssSelector("[data-test='complete-header']");

    public CheckoutCompletePage(IWebDriver driver) : base(driver, CompleteHeader) { }

    public InventoryPage BackHome()
    {
        Click(BackHomeButton);
        return new InventoryPage(Driver);
    }

    public string GetHeader() => GetText(CompleteHeader);
}
