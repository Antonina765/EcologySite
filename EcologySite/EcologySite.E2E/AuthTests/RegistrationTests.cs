using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using EcologySite.E2E.Pages;
using EcologySite.E2E.Pages.admin;

namespace EcologySite.E2E.AuthTests
{
    public class RegistrationTests
    {
        public IWebDriver _webDriver;

        [OneTimeSetUp]
        public void SetUp()
        {
            _webDriver = new ChromeDriver();
        }

        [Test]
        [TestCase("SmileTest1", "123")]
        [TestCase("SmileTest2", "321")]
        [TestCase("SmileTest3", "SmileTest3")]
        public void Registration(string username, string password)
        {
            _webDriver.Url = CommonConstants.BASE_URL;

            var loginLink = _webDriver.FindElement(LayoutSelectors.RegistrationLink);
            loginLink.Click();

            var userNameInput = _webDriver.FindElement(LayoutSelectors.UserName);
            userNameInput.SendKeys(username);

            var passwordInput = _webDriver.FindElement(LayoutSelectors.Password);
            passwordInput.SendKeys(password);

            var sybmitButton = _webDriver.FindElement(LayoutSelectors.SubmitButton);
            sybmitButton.Click();

            _webDriver.Login(username, password);
            _webDriver.Logout();

            _webDriver.LoginAsAdmin();
            _webDriver.Url = "https://localhost:7130/Admin/Users";
            var deleteLink = _webDriver.FindElement(UsersSelectors.GenerateByForUser(username));
            deleteLink.Click();
            _webDriver.Logout();

            Assert.Pass();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _webDriver.Close();
        }
    }
}
