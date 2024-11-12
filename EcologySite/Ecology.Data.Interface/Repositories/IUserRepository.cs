using Ecology.Data.Interface.Models;

namespace Ecology.Data.Interface.Repositories;

public interface IUserRepositry<T> : IBaseRepository<T>
    where T : IUser
{
}