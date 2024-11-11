using Ecology.Data.Interface.Models;
using Ecology.Data.Models.Ecology;

namespace Ecology.Data.Models;

public class UserData : BaseModel, IUser
{
    public string Login { get; set; }
    public decimal Coins { get; set; }
    public string AvatarUrl { get; set; }
    //public IEnumerable<EcologyData>? Ecologies { get; set; }
}