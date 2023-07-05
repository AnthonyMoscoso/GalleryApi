using AutoMapper;
using Bsn.Authentification.Interfaces;
using Bsn.EncrypterService;
using Bsn.Utilities.Constants;
using Core.Utilities;

using Core.Utilities.Ensures;
using Core.Utilities.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Model.Dto.Auth;
using Microsoft.Azure.Functions.Worker.Http;
using Core.Utilities.Exceptions;
using Model.Dto.User;
using System.IdentityModel.Tokens.Jwt;
using Infra.DataAccess.DBs.Interfaces;
using Model.Entity.DBs.Auth;
using Infra.DataAccess.Interfaces;
using Model.Entity.DBs.Dbo;

namespace Bsn.Authentification
{
    public class AuthentificationService : IAuthentificationService
    {

        private readonly ICredentialsRepository _userCredentialRepository;
        private readonly IEncrypterService _encrypterService;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IValidator<LoginRequest> _validator;
        private readonly ISessionRepository _sessionRepository;

        public AuthentificationService(ICredentialsRepository userCredentialRepository,
            IEncrypterService encrypterService,
            ITokenService tokenService,
            IMapper mapper,
            IUserRepository userRepository,
            ISessionRepository sessionRepository,
            IValidator<LoginRequest> validator)
        {
            _userCredentialRepository = userCredentialRepository;
            _encrypterService = encrypterService;
            _tokenService = tokenService;
            _mapper = mapper;
            _userRepository = userRepository;
            _validator = validator;
           
            _sessionRepository = sessionRepository;
        }

       

        public async Task<LoginResult> Login(LoginRequest loginRequest)
        {
            Ensure.That(loginRequest, nameof(loginRequest)).IsNotNull();
            _validator.ValidateAndThrow(loginRequest);
            string email = Base64.Decode(loginRequest.Identifier);
            string pass = Base64.Decode(loginRequest.Credential);
            Credentials? userCredential = await _userCredentialRepository.Get(w => w.Username.ToLower().Equals(_encrypterService.Encrypt(email)));
            UnauthorizedException.ThrowIfTrue(userCredential == null,ErrorMessage.IncorrectCredentials);
            string password = _encrypterService.Encrypt(pass);
            UnauthorizedException.ThrowIfFalse(password.ToLower().Equals( userCredential!.Password.ToLower()), ErrorMessage.IncorrectCredentials);
            User? user = await _userRepository.Get(userCredential.IdUser);
            NotFoundException.ThrowIfTrue(user == null,ErrorMessage.UserNotFound);
            UserInfo userInfo = _mapper.Map<UserInfo>(user);
            string token = await _tokenService.GenerateToken(userInfo);
            JwtSecurityToken jwtSecurityToken = new(token);
            DateTime expiration = jwtSecurityToken.ValidTo;
            string? idUser = jwtSecurityToken.GetClaimValue<string>(Constant.ClaimsIdUser);
            bool validGuid = Guid.TryParse(idUser, out var idUserGuid);
            BadRequestException.ThrowIfFalse(validGuid, ErrorMessage.WrongGuidFormat);
            Session? session = await _sessionRepository.Get(w=> w.IdUser == idUserGuid);
            if (session!= null){
                await _sessionRepository.Delete(session.Id);
            }
            validGuid = Guid.TryParse(jwtSecurityToken.Id, out var idGuid);
            BadRequestException.ThrowIfFalse(validGuid, ErrorMessage.WrongGuidFormat);
            session = new Session() { Token = token, Id = idGuid, ExpirationDate = expiration, IdUser = idUserGuid! };
            await _sessionRepository.Insert(session);
            return new() { Token = token, };
        }

        public async Task<bool> LogOut(string token)
        {
            JwtSecurityToken jwtSecurityToken = new(token);
            bool validGuid = Guid.TryParse(jwtSecurityToken.Id, out var idGuid);
            BadRequestException.ThrowIfFalse(validGuid, ErrorMessage.WrongGuidFormat);
            Session? session = await _sessionRepository.Get(idGuid);
            UnauthorizedException.ThrowIfTrue(session == null);
            await _sessionRepository.Delete(idGuid);
            return true;
        }

       
    }
}
