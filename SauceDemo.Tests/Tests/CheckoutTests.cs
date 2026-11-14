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

    private CheckoutInformationPage OpenCheckoutWithTwoProducts() =>
        LoginAsStandardUser()
            .AddToCart(Products.Backpack)
            .AddToCart(Products.BikeLight)
            .OpenCart()
            .Checkout();

    [TestCaseSource(nameof(MissingFieldCases))]
    public void Checkout_WithMissingField_ShowsRequiredFieldError(Customer customer, string expectedError)
    {
        var checkoutInformationPage = OpenCheckoutWithTwoProducts()
            .FillInformation(customer)
            .ContinueExpectingError();

        Assert.That(checkoutInformationPage.GetErrorMessage(), Is.EqualTo(expectedError));
    }

    [Test]
    public void Checkout_Overview_ItemTotalEqualsSumOfItemPrices()
    {
        var checkoutOverviewPage = OpenCheckoutWithTwoProducts()
            .FillInformation(new CustomerBuilder().Build())
            .Continue();

        Assert.That(checkoutOverviewPage.GetItemTotal(), Is.EqualTo(checkoutOverviewPage.GetItemPrices().Sum()));
    }
}
