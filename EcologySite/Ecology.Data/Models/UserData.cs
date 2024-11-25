using Enums.Users;
using Ecology.Data.Fake.Models;
using Ecology.Data.Interface.Models;
using EcologyData = Ecology.Data.Models.Ecology.EcologyData;

namespace Ecology.Data.Models;

public class UserData : BaseModel, IUser
{
    public string Login { get; set; }
    public string Password { get; set; }
    //public decimal Coins { get; set; }
    public string AvatarUrl { get; set; }
    
    public Language Language {  get; set; }
    
    public Role Role {  get; set; }
    
    public IEnumerable<EcologyData>? Ecologies { get; set; }
    public IEnumerable<CommentData>? Comments { get; set; }
}