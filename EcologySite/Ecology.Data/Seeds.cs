using Enums.Users;
using Ecology.Data.Models;
using Ecology.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Everything.Data;
public class Seed
{ 
    public void Fill(IServiceProvider service)
    { 
        using var di = service.CreateScope();

        UserFill(di);
    }

    private void UserFill(IServiceScope di)
    {
        var userRepositry = di.ServiceProvider.GetRequiredService<IUserRepositryReal>();
        if (userRepositry.IsAdminExist())
        {
            return;
        }

        userRepositry.Register("admin", "admin", Role.Admin);
    } 
}