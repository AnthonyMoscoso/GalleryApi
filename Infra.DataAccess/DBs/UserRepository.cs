using Core.Repository;
using Infra.DataAccess.Interfaces;
using Model.Context.Contexts;
using Model.Entity.DBs.Dbo;

namespace Infra.DataAccess
{
    public class UserRepository : EFRepository<User, Guid>, IUserRepository
    {
        private new readonly SaalDigitalContext _context;
        public UserRepository(SaalDigitalContext context) : base(context)
        {
            _context= context;
        }

      
    }
}

