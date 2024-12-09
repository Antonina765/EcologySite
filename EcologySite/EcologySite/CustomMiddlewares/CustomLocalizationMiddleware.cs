using Enums.Users;
using Ecology.Data.Repositories;
using System.Globalization;
using EcologySite.Services;
using Ecology.Data.Interface.Models;
using Ecology.Data.Repositories;
using System.Globalization;
using System.Text.RegularExpressions;


namespace EcologySite.CustomMiddlewares
{
    public class CustomLocalizationMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomLocalizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var authService = context.RequestServices.GetRequiredService<AuthService>();
            var userRepositryReal = context.RequestServices.GetRequiredService<IUserRepositryReal>();

            if (authService.IsAuthenticated())
            {
                var user = userRepositryReal.Get(authService.GetUserId()!.Value)!;
                SwitchLanguage(user.Language);
                await _next.Invoke(context);
                return;
            }

            var langFromCookie = context.Request.Cookies["lang"];
            if (langFromCookie != null)
            {
                var lang = Enum.Parse<Language>(langFromCookie);
                SwitchLanguage(lang);
                await _next.Invoke(context);
                return;
            }
            
            if (context.Request.Headers.ContainsKey("accept-language"))
            {
                var langFromHeader = context.Request.Headers["accept-language"].FirstOrDefault();
                if (langFromHeader is not null)
                {
                    var localStrCode = langFromHeader.Substring(0, 5);
                    var culture = new CultureInfo(localStrCode);
                    SwitchLanguage(culture);
                    await _next.Invoke(context);
                    return;
                }
            }

            await _next.Invoke(context);
        }

        private void SwitchLanguage(Language language)
        {
            CultureInfo culture;

            switch (language)
            {
                case Language.Ru:
                    culture = new CultureInfo("ru-RU");
                    break;
                case Language.En:
                    culture = new CultureInfo("en-US");
                    break;
                default:
                    throw new Exception("Unknown languge");
            }

            SwitchLanguage(culture);
        }

        private void SwitchLanguage(CultureInfo culture)
        {
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }
    }
}
