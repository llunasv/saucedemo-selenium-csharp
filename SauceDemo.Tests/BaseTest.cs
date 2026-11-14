using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SauceDemo.Tests;

public abstract class BaseTest
{

    protected const string BaseUrl = "https://www.saucedemo.com/";
    protected IWebDriver Driver = null!;


    [SetUp]
    public void Setup()
    {
        var options = new ChromeOptions();
        options.AddArgument("--window-size=1920,1080");
        Driver = new ChromeDriver(options);
        Driver.Navigate().GoToUrl(BaseUrl);
    }

    [TearDown]
    public void TearDown()
    {
        Driver.Quit();
        Driver.Dispose();
    }
}
