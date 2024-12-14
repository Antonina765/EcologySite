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
        private readonly ILogger<CustomLocalizationMiddleware> _logger;

        public CustomLocalizationMiddleware(RequestDelegate next, ILogger<CustomLocalizationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var authService = context.RequestServices.GetRequiredService<AuthService>();
            var userRepositryReal = context.RequestServices.GetRequiredService<IUserRepositryReal>();

            if (authService.IsAuthenticated())
            {
                var user = userRepositryReal.Get(authService.GetUserId()!.Value)!;
                try
                {
                    SwitchLanguage(user.Language);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error switching language for authenticated user");
                    // Fallback to default language if needed
                }
                await _next.Invoke(context);
                return;
            }

            var langFromCookie = context.Request.Cookies["lang"];
            if (langFromCookie != null)
            {
                try
                {
                    var lang = Enum.Parse<Language>(langFromCookie);
                    SwitchLanguage(lang);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error switching language from cookie");
                    // Fallback to default language if needed
                }
                await _next.Invoke(context);
                return;
            }

            if (context.Request.Headers.ContainsKey("accept-language"))
            {
                var langFromHeader = context.Request.Headers["accept-language"].FirstOrDefault();
                if (langFromHeader != null && langFromHeader.Length >= 5)
                {
                    var localStrCode = langFromHeader.Substring(0, 5);
                    var culture = new CultureInfo(localStrCode);
                    SwitchLanguage(culture);
                    await _next.Invoke(context);
                    return;
                }
                else 
                { 
                    // Fallback to default language if needed
                    SwitchLanguage(new CultureInfo("en-US"));
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
                    throw new Exception("Unknown language");
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
