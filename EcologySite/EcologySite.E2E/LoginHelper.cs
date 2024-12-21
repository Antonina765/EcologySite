using OpenQA.Selenium;
using EcologySite.E2E.Pages;

namespace EcologySite.E2E
{
    public static class LoginHelper
    {
        public static void Login(this IWebDriver webDriver, string userName, string password)
        {
            webDriver.Url = CommonConstants.BASE_URL;

            var loginLink = webDriver.FindElement(LayoutSelectors.LoginLink);
            loginLink.Click();

            var userNameInput = webDriver.FindElement(LayoutSelectors.UserName);
            userNameInput.SendKeys(userName);

            var passwordInput = webDriver.FindElement(LayoutSelectors.Password);
            passwordInput.SendKeys(password);

            var sybmitButton = webDriver.FindElement(LayoutSelectors.SubmitButton);
            sybmitButton.Click();
        }

        public static void Logout(this IWebDriver webDriver)
        {
            var logoutLink = webDriver.FindElement(LayoutSelectors.LogoutLink);
            logoutLink.Click();
        }

        public static void LoginAsAdmin(this IWebDriver webDriver)
        {
            webDriver.Login("admin", "admin");
        }
    }
}
