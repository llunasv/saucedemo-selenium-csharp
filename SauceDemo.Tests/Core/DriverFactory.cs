using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Chromium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;

namespace SauceDemo.Tests.Core;

public static class DriverFactory
{
    public static IWebDriver Create() => TestSettings.Browser switch
    {
        "chrome" => new ChromeDriver(ConfigureChromium(new ChromeOptions())),
        "edge" => new EdgeDriver(ConfigureChromium(CreateEdgeOptions())),
        "firefox" => new FirefoxDriver(CreateFirefoxOptions()),
        _ => throw new ArgumentException(
            $"Unsupported BROWSER '{TestSettings.Browser}'. Use chrome, firefox or edge.")
    };

    private static T ConfigureChromium<T>(T options) where T : ChromiumOptions
    {
        options.AddArgument("--window-size=1920,1080");
        if (TestSettings.Headless) options.AddArgument("--headless=new");

        // Chrome flags "secret_sauce" as a leaked password and opens a
        // "Change your password" popup that swallows clicks after login.
        options.AddUserProfilePreference("credentials_enable_service", false);
        options.AddUserProfilePreference("profile.password_manager_enabled", false);
        options.AddUserProfilePreference("profile.password_manager_leak_detection", false);
        return options;
    }

    private static EdgeOptions CreateEdgeOptions()
    {
        var options = new EdgeOptions();
        // On the Linux CI runner Edge exits right after launch ("Chrome instance exited")
        // while Chrome starts fine: Edge can't set up its sandbox there
        if (OperatingSystem.IsLinux()) options.AddArgument("--no-sandbox");
        return options;
    }

    private static FirefoxOptions CreateFirefoxOptions()
    {
        var options = new FirefoxOptions();
        options.AddArguments("--width=1920", "--height=1080");
        if (TestSettings.Headless) options.AddArgument("-headless");
        return options;
    }
}
