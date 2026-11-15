using SauceDemo.Tests.Core;
using SauceDemo.Tests.Pages;

namespace SauceDemo.Tests.Tests;

public class LogoutTests : BaseTest
{
    [Test]
    public void Logout_FromMenu_OpensLoginPage()
    {
        LoginAsStandardUser().Logout();

        Assert.That(Driver.Url, Is.EqualTo(TestSettings.BaseUrl));
    }

    [Test]
    public void OpenInventory_AfterLogout_ShowsLoginRequiredError()
    {
        LoginAsStandardUser().Logout();

        Driver.Navigate().GoToUrl(TestSettings.BaseUrl + "inventory.html");
        var loginPage = new LoginPage(Driver);

        Assert.That(loginPage.GetErrorMessage(),
            Is.EqualTo("Epic sadface: You can only access '/inventory.html' when you are logged in."));
    }
}
