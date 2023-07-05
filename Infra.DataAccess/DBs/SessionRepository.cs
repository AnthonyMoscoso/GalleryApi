using Core.Repository;
using Infra.DataAccess.DBs.Interfaces;
using Model.Context.Contexts;
using Model.Entity.DBs.Auth;

namespace Infra.DataAccess
{
    public class SessionRepository : EFRepository<Session, Guid>, ISessionRepository
    {
        private new readonly SaalDigitalContext _context;
        public SessionRepository(SaalDigitalContext context) : base(context)
        {
            _context = context;
        }
    }
}
