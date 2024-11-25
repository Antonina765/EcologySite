using Enums.Users;
using Ecology.Data.Repositories;
using System.Globalization;
using EcologySite.Services;

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
                CultureInfo culture;
                switch (user.Language)
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

                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;
            }

            await _next.Invoke(context);
        }
    }
}
