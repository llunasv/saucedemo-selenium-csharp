namespace SauceDemo.Tests
{
    public class SmokeTest : BaseTest
    {
        [Test]
        public void LoginPage_Opens() => Assert.That(Driver.Title, Is.EqualTo("Swag Labs"));
    }
}
