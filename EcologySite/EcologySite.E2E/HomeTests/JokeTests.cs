using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using EcologySite.E2E.Pages.Home;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace EcologySite.E2E.HomeTests
{
    public class JokeTests
    {
        public IWebDriver _webDriver;
        public WebDriverWait _wait; 

        [OneTimeSetUp]
        public void SetUp()
        {
            _webDriver = new ChromeDriver();
            _wait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(10));
            _webDriver.Url = CommonConstants.BASE_URL;
        }

        [Test]
        public void Test()
        {
            var jokeButton  = _wait.Until(
                ExpectedConditions.ElementToBeClickable(IndexSelectors.JokeButton));

            jokeButton.Click();

            Assert.Pass();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _webDriver.Close();
        }
    }
}
