namespace Ecology.Data.Interface.Models;

public interface IUser : IBaseModel
{
    public string Login { get; set; }
    public decimal Coins { get; set; }

    public string AvatarUrl { get; set; }
}