using OpenQA.Selenium;

namespace EcologySite.E2E.Pages
{
    public class LayoutSelectors
    {
        public static readonly By LoginLink = By.ClassName("login-link");
        public static readonly By LogoutLink = By.ClassName("logout-link");
        public static readonly By RegistrationLink = By.ClassName("registration-link");

        public static readonly By UserName = By.CssSelector("#UserName");
        public static readonly By Password = By.CssSelector("#Password");

        public static readonly By SubmitButton = By.CssSelector("button[type=submit]");
    }
}
