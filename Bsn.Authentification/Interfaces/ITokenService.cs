using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker.Http;
using Model.Dto.Auth;

namespace Bsn.Authentification.Interfaces
{
    public interface ITokenService
    {
        Task<string> GenerateToken(UserInfo user);
        Task<UserInfo> GetUserFromToken(HttpRequest req);
        Task<UserInfo> GetUserFromToken(HttpRequestData req);
        Task<bool> ValidateToken(string authToken);

        
    }
}
