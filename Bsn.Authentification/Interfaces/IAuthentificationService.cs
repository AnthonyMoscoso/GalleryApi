using Microsoft.AspNetCore.Http;
using Model.Dto.Auth;
using Microsoft.Azure.Functions.Worker.Http;
namespace Bsn.Authentification.Interfaces
{
    public interface IAuthentificationService
    {
        Task<LoginResult> Login(LoginRequest loginRequest);
        Task<bool> LogOut(string token);
      
    }
}
