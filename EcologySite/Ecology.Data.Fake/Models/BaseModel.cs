using Ecology.Data.Interface.Models;
namespace Ecology.Data.Fake.Models;

public abstract class BaseModel : IBaseModel
{
    public int Id { get; set; }
}