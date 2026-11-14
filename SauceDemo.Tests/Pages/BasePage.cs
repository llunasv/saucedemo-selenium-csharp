using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SauceDemo.Tests.Core;

namespace SauceDemo.Tests.Pages;

public abstract class BasePage
{
    protected readonly IWebDriver Driver;
    protected readonly WebDriverWait Wait;

    protected BasePage(IWebDriver driver)
    {
        Driver = driver;
        Wait = new WebDriverWait(driver, TestSettings.DefaultTimeout); //10 secs
    }

    protected IWebElement Find(By locator) =>
        Wait.Until(d =>
        {
            var element = d.FindElement(locator);
            return element.Displayed ? element : null;
        });

    protected void Click(By locator) => Find(locator).Click();

    protected void Type(By locator, string text)
    {
        var element = Find(locator);
        element.Clear();
        element.SendKeys(text);
    }

    protected string GetText(By locator) => Find(locator).Text;

    protected IReadOnlyList<string> GetTexts(By locator) =>
        Wait.Until(d =>
        {
            var elements = d.FindElements(locator);
            return elements.Count > 0 ? elements.Select(e => e.Text).ToList() : null;
        });

    protected bool IsPresent(By locator) => Driver.FindElements(locator).Count > 0;
}