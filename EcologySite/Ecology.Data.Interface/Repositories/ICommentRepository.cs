using Ecology.Data.Interface.Models;

namespace Ecology.Data.Interface.Repositories;

public interface ICommentRepository<T> : IBaseRepository<T>
    where T : ICommentData
{
}
