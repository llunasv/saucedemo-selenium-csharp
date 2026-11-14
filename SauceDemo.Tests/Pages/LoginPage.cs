using OpenQA.Selenium;

namespace SauceDemo.Tests.Pages;

public class LoginPage : BasePage
{
    private static readonly By UsernameInput = By.CssSelector("[data-test='username']");
    private static readonly By PasswordInput = By.CssSelector("[data-test='password']");
    private static readonly By LoginButton = By.CssSelector("[data-test='login-button']");
    private static readonly By ErrorMessage = By.CssSelector("[data-test='error']");

    public LoginPage(IWebDriver driver) : base(driver, LoginButton) { }

    public InventoryPage Login(string username, string password)
    {
        SubmitCredentials(username, password);
        return new InventoryPage(Driver);
    }

    public LoginPage LoginExpectingError(string username, string password)
    {
        SubmitCredentials(username, password);
        return this;
    }

    private void SubmitCredentials(string username, string password)
    {
        Type(UsernameInput, username);
        Type(PasswordInput, password);
        Click(LoginButton);
    }

    public string GetErrorMessage() => GetText(ErrorMessage);
}