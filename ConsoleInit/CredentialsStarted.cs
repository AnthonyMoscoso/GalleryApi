using Bsn.DataServices.Users.Interfaces;
using Bsn.EncrypterService;
using Core.Utilities;
using Infra.DataAccess.DBs.Interfaces;
using Model.Dto.Auth;
using Model.Dto.User;
using Model.Entity.DBs.Auth;

namespace StartedConfig
{
    public class CredentialsStarted
    {
        private readonly ICredentialsRepository _credentialsRepository;
        private readonly IEncrypterService _encrypterService;
        private readonly Guid defaulSystemId = new("{YourGuidId}");
        public CredentialsStarted(ICredentialsRepository credentialsRepository,IEncrypterService encrypterService)
        {
            _credentialsRepository = credentialsRepository;
            _encrypterService = encrypterService;
        }

        public async Task CreateDefault()
        {

            Credentials credentials = new()
            {
                
                IdUser = defaulSystemId,
                Username = _encrypterService.Encrypt("system"),
                Password = _encrypterService.Encrypt("system"),
                IdUserCreater = defaulSystemId.ToString(),
                Created = DateTime.UtcNow


            };
            await _credentialsRepository.Insert(credentials);

     
        }
    }
}
