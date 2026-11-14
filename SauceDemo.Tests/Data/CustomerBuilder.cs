namespace SauceDemo.Tests.Data;

// Starts with a valid customer; each test changes only the field it cares about.
public class CustomerBuilder
{
    private string _firstName = "John";
    private string _lastName = "Doe";
    private string _postalCode = "12345";

    public CustomerBuilder WithoutFirstName()
    {
        _firstName = "";
        return this;
    }

    public CustomerBuilder WithoutLastName()
    {
        _lastName = "";
        return this;
    }

    public CustomerBuilder WithoutPostalCode()
    {
        _postalCode = "";
        return this;
    }

    public Customer Build() => new(_firstName, _lastName, _postalCode);
}
