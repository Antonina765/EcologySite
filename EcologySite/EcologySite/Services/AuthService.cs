namespace EcologySite.Services;

public class AuthService
{
    private IHttpContextAccessor _httpContextAccessor;

    public const string AUTH_TYPE_KEY = "Smile";
    public const string CLAIM_TYPE_ID = "Id";
    public const string CLAIM_TYPE_NAME = "Name";

    public AuthService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public bool IsAuthenticated()
    {
        return GetUserId() is not null;
    }

    public string? GetName()
    {
        return GetClaimValue(CLAIM_TYPE_NAME) ?? "Guest";
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