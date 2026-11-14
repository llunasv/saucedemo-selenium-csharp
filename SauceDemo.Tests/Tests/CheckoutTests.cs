using SauceDemo.Tests.Core;
using SauceDemo.Tests.Data;
using SauceDemo.Tests.Pages;

namespace SauceDemo.Tests.Tests;

public class CheckoutTests : BaseTest
{
    private static IEnumerable<TestCaseData> MissingFieldCases()
    {
        yield return new TestCaseData(new CustomerBuilder().WithoutFirstName().Build(), "Error: First Name is required")
            .SetArgDisplayNames("missing first name");
        yield return new TestCaseData(new CustomerBuilder().WithoutLastName().Build(), "Error: Last Name is required")
            .SetArgDisplayNames("missing last name");
        yield return new TestCaseData(new CustomerBuilder().WithoutPostalCode().Build(), "Error: Postal Code is required")
            .SetArgDisplayNames("missing postal code");
    }

    private CheckoutInformationPage OpenCheckoutWithTwoProducts()
    {
        new LoginPage(Driver).Login(Users.Standard, Users.Password);
        var inventoryPage = new InventoryPage(Driver);
        inventoryPage.AddToCart(Products.Backpack);
        inventoryPage.AddToCart(Products.BikeLight);
        inventoryPage.OpenCart();
        new CartPage(Driver).Checkout();
        return new CheckoutInformationPage(Driver);
    }

    [TestCaseSource(nameof(MissingFieldCases))]
    public void Checkout_WithMissingField_ShowsRequiredFieldError(Customer customer, string expectedError)
    {
        var checkoutInformationPage = OpenCheckoutWithTwoProducts();

        checkoutInformationPage.FillInformation(customer);
        checkoutInformationPage.Continue();

        Assert.That(checkoutInformationPage.GetErrorMessage(), Is.EqualTo(expectedError));
    }

    [Test]
    public void Checkout_Overview_ItemTotalEqualsSumOfItemPrices()
    {
        var checkoutInformationPage = OpenCheckoutWithTwoProducts();

        checkoutInformationPage.FillInformation(new CustomerBuilder().Build());
        checkoutInformationPage.Continue();

        var checkoutOverviewPage = new CheckoutOverviewPage(Driver);
        Assert.That(checkoutOverviewPage.GetItemTotal(), Is.EqualTo(checkoutOverviewPage.GetItemPrices().Sum()));
    }
}
