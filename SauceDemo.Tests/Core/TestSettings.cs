namespace SauceDemo.Tests.Core;

public static class TestSettings
{
    public static string BaseUrl { get; } =
        Environment.GetEnvironmentVariable("BASE_URL") ?? "https://www.saucedemo.com/";

    public static bool Headless { get; } =
        bool.TryParse(Environment.GetEnvironmentVariable("HEADLESS"), out var headless) && headless;

    public static TimeSpan DefaultTimeout { get; } = TimeSpan.FromSeconds(10);
}