namespace SauceDemo.Tests.Data;

public record Product(string Id, string Name);

public static class Products
{
    public static readonly Product Backpack = new("sauce-labs-backpack", "Sauce Labs Backpack");
    public static readonly Product BikeLight = new("sauce-labs-bike-light", "Sauce Labs Bike Light");
}
