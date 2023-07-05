using Core.Repository;
using Model.Entity.DBs.Dbo;

namespace Infra.DataAccess.Interfaces
{
    public interface IUserRepository : IRepository<User, Guid>
    {
      
    }
}
