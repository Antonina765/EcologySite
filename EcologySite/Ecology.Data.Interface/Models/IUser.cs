using Enums.Users;
namespace Ecology.Data.Interface.Models;

public interface IUser : IBaseModel
{
    public string Login { get; set; }
    public string Password { get; set; }
    //public decimal Coins { get; set; }

    public string AvatarUrl { get; set; }
    
    public Role Role { get; set; }
}