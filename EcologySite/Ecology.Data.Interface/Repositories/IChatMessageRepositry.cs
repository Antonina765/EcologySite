using Ecology.Data.Interface.Models;

namespace Ecology.Data.Interface.Repositories
{
    public interface IChatMessageRepositry<T> : IBaseRepository<T>
        where T : IChatMessageData
    {
    }
}
