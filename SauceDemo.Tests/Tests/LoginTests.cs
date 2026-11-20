using SauceDemo.Tests.Core;
using SauceDemo.Tests.Data;
using SauceDemo.Tests.Pages;

namespace SauceDemo.Tests.Tests;

public class LoginTests : BaseTest
{
    [Test]
    public void Login_WithValidCredentials_OpensInventoryPage()
    {
        new LoginPage(Driver).Login(Users.Standard, Users.Password);

        Assert.That(Driver.Url, Does.Contain("inventory.html"));
    }

    [Test]
    public void Login_WithLockedOutUser_ShowsLockedOutError()
    {
        var loginPage = new LoginPage(Driver).LoginExpectingError(Users.LockedOut, Users.Password);

        Assert.That(loginPage.GetErrorMessage(),
            Is.EqualTo("Epic sadface: Sorry, this user has been locked out."));
    }

    [TestCase(Users.Standard, "wrong_password")]
    [TestCase("no_such_user", Users.Password)]
    public void Login_WithInvalidCredentials_ShowsMismatchError(string username, string password)
    {
        var loginPage = new LoginPage(Driver).LoginExpectingError(username, password);

        Assert.That(loginPage.GetErrorMessage(),
            Is.EqualTo("Epic sadface: Username and password do not match any user in this service"));
    }

    [TestCase(Users.Standard, "", "Epic sadface: Password is required")]
    [TestCase("", Users.Password, "Epic sadface: Username is required")]
    [TestCase("", "", "Epic sadface: Username is required")]
    public void Login_WithMissingCredentials_ShowsRequiredFieldError(string username, string password, string expected)
    {
        var loginPage = new LoginPage(Driver).LoginExpectingError(username, password);

        Assert.That(loginPage.GetErrorMessage(), Is.EqualTo(expected));
    }
}
