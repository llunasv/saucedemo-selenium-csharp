using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SauceDemo.Tests.Core;

public abstract class BaseTest
{

    protected  IWebDriver Driver = null!;

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
}
