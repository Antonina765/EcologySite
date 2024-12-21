using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using EcologySite.E2E.Pages;

namespace EcologySite.E2E
{
    public class LoginTest
    {
        public IWebDriver _webDriver;

        [OneTimeSetUp]
        public void SetUp()
        {
            _webDriver = new ChromeDriver();
        }

        [Test]
        public void LoginAsAdmin()
        {
            _webDriver.Login("admin", "admin");

            var logoutLink = _webDriver.FindElement(LayoutSelectors.LogoutLink);
            logoutLink.Click(); 

            Assert.Pass();
        }

        [Test]
        public void LoginAsUser()
        {
            _webDriver.Login("user", "user"); 

            var logoutLink = _webDriver.FindElement(LayoutSelectors.LogoutLink);
            logoutLink.Click();

            Assert.Pass();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _webDriver.Close();
        }
    }
}
