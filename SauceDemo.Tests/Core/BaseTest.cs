using OpenQA.Selenium;
using SauceDemo.Tests.Data;
using SauceDemo.Tests.Pages;

namespace SauceDemo.Tests.Core;

public abstract class BaseTest
{
    protected IWebDriver Driver = null!;

    [SetUp]
    public void Setup()
    {
        Driver = DriverFactory.Create();
        Driver.Navigate().GoToUrl(TestSettings.BaseUrl);
    }

    [TearDown]
    public void TearDown()
    {
        Driver.Quit();
        Driver.Dispose();
    }

    protected InventoryPage LoginAsStandardUser() =>
        new LoginPage(Driver).Login(Users.Standard, Users.Password);
}
