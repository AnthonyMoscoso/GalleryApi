using Core.Repository;
using Infra.DataAccess.DBs.Interfaces;
using Model.Context.Contexts;
using Model.Entity.DBs.Auth;

namespace Infra.DataAccess
{
    public class CredentialRepository : EFRepository<Credentials, Guid>, ICredentialsRepository
    {
        private new readonly SaalDigitalContext _context;
        public CredentialRepository(SaalDigitalContext context) : base(context)
        {
            _context= context;
        }

       
    }
}
