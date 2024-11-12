using Ecology.Data.Interface.Models;

namespace Ecology.Data.Models;

public class UserData : BaseModel, IUser
{
    public string Login { get; set; }
    public string Password { get; set; }
    //public decimal Coins { get; set; }
    public string AvatarUrl { get; set; }
}