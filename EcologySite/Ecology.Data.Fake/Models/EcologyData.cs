using Ecology.Data.Interface.Models;
namespace Ecology.Data.Fake.Models;

public class EcologyData : BaseModel, IEcologyData
{
    public string ImageSrc { get; set; }
    public List<string> Text { get; set; }
}
