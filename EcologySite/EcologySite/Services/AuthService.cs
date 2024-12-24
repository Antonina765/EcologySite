using System.Data;
using EcologySite.Localizations;
using Enums.Users;

namespace EcologySite.Services;

[AutoRegisterFlag]
public class AuthService
{
    private IHttpContextAccessor _httpContextAccessor;

    public const string AUTH_TYPE_KEY = "Smile";
    public const string CLAIM_TYPE_ID = "Id";
    public const string CLAIM_TYPE_NAME = "Name";
    public const string CLAIM_TYPE_ROLE = "Role";

    public AuthService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public bool IsAuthenticated()
    {
        return GetUserId() is not null;
    }

    public string GetName()
    {
        //return GetClaimValue(CLAIM_TYPE_NAME) ?? "Гость";
        return GetClaimValue(CLAIM_TYPE_NAME) ?? Home.Home_Guest;
    }

    public int? GetUserId()
    {
        var isStr = GetClaimValue(CLAIM_TYPE_ID);
        if (isStr == null)
        {
            return null;
        }

        return int.Parse(isStr);
    }

    public Role GetRole()
    {
        var roleStr = GetClaimValue(CLAIM_TYPE_ROLE);
        if (roleStr is null)
        {
            throw new Exception("Guest cant has a role");
        }
        var roleInt = int.Parse(roleStr);
        var role = (Role)roleInt;
        return role;
    }

    public bool IsAdmin()
    {
        return IsAuthenticated() && GetRole().HasFlag(Role.Admin);
    }

    private string? GetClaimValue(string type)
    {
        return _httpContextAccessor
            .HttpContext!
            .User
            .Claims
            .FirstOrDefault(x => x.Type == type)
            ?.Value;
    } 
}