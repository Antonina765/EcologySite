using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecology.Data.Interface.Models;

namespace Ecology.Data.Models.Ecology;

public class EcologyData : BaseModel, IEcologyData
{
    private string _id;
    public int Id { get; set; }
    public string ImageSrc { get; set; }
    public string Text { get; set; }
    //public UserData User { get; set; }
    //public virtual List<CommentData> Comments { get; set; } = new List<CommentData>();
    public ICollection<CommentData> Comments { get; set; }
}
