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
    public IEnumerable<CommentData>? Comments { get; set; }
}
