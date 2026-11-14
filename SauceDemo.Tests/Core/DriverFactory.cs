using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SauceDemo.Tests.Core
{
    public static class DriverFactory
    {
        public static IWebDriver Create()
        {
            var options = new ChromeOptions();
            options.AddArgument("--window-size=1920,1080");
            if (TestSettings.Headless) options.AddArgument("--headless=new");

            // Chrome flags "secret_sauce" as a leaked password and opens a
            // "Change your password" popup that swallows clicks after login.
            options.AddUserProfilePreference("credentials_enable_service", false);
            options.AddUserProfilePreference("profile.password_manager_enabled", false);
            options.AddUserProfilePreference("profile.password_manager_leak_detection", false);

            return new ChromeDriver(options);
        }
    }
}
