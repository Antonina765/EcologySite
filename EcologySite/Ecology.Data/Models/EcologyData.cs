using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecology.Data.Interface.Models;

namespace Ecology.Data.Models.Ecology;

public class EcologyData : BaseModel, IEcologyData
{
    public string ImageSrc { get; set; }
    public string Text { get; set; }
    public int UserId { get; set; }
    public int ForMainPage { get; set; } = 0;
    public UserData User { get; set; }
    public IEnumerable<CommentData>? Comments { get; set; }
    //public virtual List<UserData> UsersWhoLikeIt { get; set; }
    public ICollection<UserEcologyLikesData> UsersWhoLikeIt { get; set; } = new List<UserEcologyLikesData>();
}
