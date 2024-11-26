using Enums.Users;
using Ecology.Data.Interface.Repositories;
using Ecology.Data.Models;

namespace Ecology.Data.Repositories;

public interface IUserRepositryReal : IUserRepositry<UserData>
{
    string GetAvatarUrl(int userId);
    bool IsAdminExist();
    UserData? Login(string login, string password);
    void Register(string login, string password, Role role = Role.User);
    void UpdateAvatarUrl(int userId, string avatarUrl);
    void UpdateLocal(int userId, Language language);
    void UpdateRole(int userId, Role role);
}

public class UserRepository : BaseRepository<UserData>, IUserRepositryReal
{
    public UserRepository(WebDbContext webDbContext) : base(webDbContext)
    {
    }

    public override void Add(UserData data)
    {
        throw new NotImplementedException("User method Register to create a new User");
    }
    
    public string GetAvatarUrl(int userId)
    {
        return _dbSet.First(x => x.Id == userId).AvatarUrl;
    }

    public bool IsAdminExist()
    {
        return _dbSet.Any(x => x.Role.HasFlag(Role.Admin));
    }
    /*public bool IsAdminExist()
    {
        using (var context = new ApplicationDbContext())
        {
            var query = from u in context.Users
                where u.Role == "Admin" // Убедитесь, что колонка `Role` существует
                //where u.UserRole == "Admin" // Используйте существующую колонку
                select u;

            return query.Any();
        }
    }*/


    public UserData? Login(string login, string password)
    {
        var brokenPassword = BrokePassword(password);

        return _dbSet.FirstOrDefault(x => x.Login == login && x.Password == brokenPassword);
    }

    public void Register(string login, string password, Role role = Role.User)
    {
        var user = new UserData
        {
            Login = login,
            Password = BrokePassword(password),
            //Age = age,
            //Coins = 100,
            AvatarUrl = "/images/avatar/default.png",
            Role = role,
            Language = Language.En
        };

        _dbSet.Add(user);
        _webDbContext.SaveChanges();
    }
    
    public void UpdateAvatarUrl(int userId, string avatarUrl)
    {
        var user = _dbSet.First(x => x.Id == userId);
        user.AvatarUrl = avatarUrl;
        _webDbContext.SaveChanges();
    }

    public void UpdateRole(int userId, Role role)
    {
        var user = _dbSet.First(x => x.Id == userId);
        user.Role = role;
        _webDbContext.SaveChanges();
    }
    
    public void UpdateLocal(int userId, Language language)
    {
        var user = _dbSet.First(x => x.Id == userId);

        user.Language = language;

        _webDbContext.SaveChanges();
    }

    private string BrokePassword(string originalPassword)
    {
        // jaaaack
        // jacke
        // jack
        var brokenPassword = originalPassword.Replace("a", "");

        // jck
        return brokenPassword;
    }
}