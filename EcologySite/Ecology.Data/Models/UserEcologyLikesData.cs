using Ecology.Data.Models.Ecology;

namespace Ecology.Data.Models;

public class UserEcologyLikesData : BaseModel
{
    public int UserId { get; set; }
    public UserData User { get; set; }

    public int EcologyDataId { get; set; }
    public EcologyData EcologyData { get; set; }
}
    