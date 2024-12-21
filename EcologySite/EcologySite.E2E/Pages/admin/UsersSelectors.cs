using OpenQA.Selenium;

namespace EcologySite.E2E.Pages.admin
{
    public class UsersSelectors
    {
        public static By GenerateByForUser(string username)
        {
            return By.CssSelector($".delete-user[data-username={username}]");
        }
    }
}
