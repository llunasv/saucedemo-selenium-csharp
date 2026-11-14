using OpenQA.Selenium;
using SauceDemo.Tests.Data;

namespace SauceDemo.Tests.Pages;

public class CheckoutInformationPage : BasePage
{
    private static readonly By FirstNameInput = By.CssSelector("[data-test='firstName']");
    private static readonly By LastNameInput = By.CssSelector("[data-test='lastName']");
    private static readonly By PostalCodeInput = By.CssSelector("[data-test='postalCode']");
    private static readonly By ContinueButton = By.CssSelector("[data-test='continue']");
    private static readonly By ErrorMessage = By.CssSelector("[data-test='error']");

    public CheckoutInformationPage(IWebDriver driver) : base(driver) { }

    public void FillInformation(Customer customer)
    {
        Type(FirstNameInput, customer.FirstName);
        Type(LastNameInput, customer.LastName);
        Type(PostalCodeInput, customer.PostalCode);
    }

    public void Continue() => Click(ContinueButton);

    public string GetErrorMessage() => GetText(ErrorMessage);
}
