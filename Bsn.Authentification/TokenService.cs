using AutoMapper;
using Bsn.Authentification.Extensions;
using Bsn.Authentification.Interfaces;
using Bsn.Authentification.Models;
using Bsn.Utilities.Configuration.Interfaces;
using Bsn.Utilities.Constants;

using Core.Utilities.Ensures;
using Core.Utilities.Exceptions;
using Core.Utilities.Extensions;
using Infra.DataAccess.DBs.Interfaces;
using Infra.DataAccess.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Model.Dto.Auth;
using Model.Entity.DBs.Auth;
using Model.Entity.DBs.Dbo;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Bsn.Authentification
{
    public class TokenService : ITokenService
    {

        private readonly IUserRepository _userRepository;
        private readonly ISessionRepository _sessionRepository;
        private readonly IMapper _mapper;
        private readonly IKeys _keys;
        private readonly int token_hour_life = 2;
        private readonly int token_minute_life = 0;
        private readonly int token_second_life = 0;
        public TokenService(IUserRepository userRepository, IMapper mapper, IKeys keys, ISessionRepository sessionRepository)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _keys = keys;
            _sessionRepository = sessionRepository;
        }
        public async Task<string> GenerateToken(UserInfo userInfo)
        {

            User? user = await _userRepository.Get(userInfo.IdUser);
            Ensure.That(user,nameof(user)).IsNotNull(ErrorMessage.UserNotFound);
            TokenData tokenData = GetTokenSecurityData();
            SigningCredentials _signingCredentials = tokenData.GetSigningCredentials();
            JwtHeader _Header = new(_signingCredentials);
            Claim[] _Claims = GetClaims(userInfo);
            JwtPayload _Payload = GetPayLoad(tokenData, _Claims);
            JwtSecurityToken _Token = new(_Header, _Payload);
            return _Token.GetWriteToken();
        }

        private JwtPayload GetPayLoad(TokenData token, Claim[] claims )
        {
            return new(
                    issuer: token.Issuer,
                    audience: token.Audience,
                    claims: claims,
                    notBefore: DateTime.UtcNow,
                    expires: GetTokenLife()

                );
        }
        private static Claim[] GetClaims(UserInfo userInfo)
        {

            return   new[] {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(Constant.ClaimsIdUser, userInfo.IdUser.ToString()),
                new Claim(Constant.ClaimsUserName, userInfo.Name)
            };
        }
        public TokenData GetTokenSecurityData()
        {
            string? secret = _keys.JWT.Secret;
            string? issuer = _keys.JWT.Issuer;
            string? audience = _keys.JWT.Audience;

            Ensure.That(secret, nameof(secret)).NotNullOrEmpty();
            Ensure.That(issuer, nameof(issuer)).NotNullOrEmpty();
            Ensure.That(audience, nameof(audience)).NotNullOrEmpty();
            return new TokenData
            {
                Secret = secret!,
                Issuer = issuer!,
                Audience = audience!
            }; 
        }
        public async Task<bool> ValidateToken(string authToken)
        {
            SecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            TokenValidationParameters validationParameters = GetValidationParameters();
            bool validTokenParameter = tokenHandler.TryValidateToken(authToken, validationParameters, out _);
            if (!validTokenParameter)
            {
                return false;
            }
            JwtSecurityToken jwtSecurityToken = new(authToken);
            bool validGuid = Guid.TryParse(jwtSecurityToken.Id, out var idGuid);
            BadRequestException.ThrowIfFalse(validGuid, ErrorMessage.WrongGuidFormat);
            Session? session = await  _sessionRepository.Get(idGuid);
            if (session == null)
            {
                return false;
            }
            if( session.ExpirationDate < DateTime.UtcNow)
            {
                await _sessionRepository.Delete(idGuid);
                return false;
            }

            return true;

        }

        private TokenValidationParameters GetValidationParameters()
        {
            TokenData tokenData = GetTokenSecurityData();
            return new TokenValidationParameters()
            {
                ValidateLifetime = true,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidIssuer = tokenData.Issuer,
                ValidAudience = tokenData.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenData.Secret))
            };
        }

        public async Task<UserInfo> GetUserFromToken(HttpRequest req)
        {
            string? bearerToken = req.GetValue<string>(Constant.authorization);
            UnauthorizedException.ThrowIfTrue(string.IsNullOrWhiteSpace(bearerToken), ErrorMessage.EmptyToken);
            string? token = bearerToken!.StartsWith($"{Constant.Bearer} ", StringComparison.InvariantCultureIgnoreCase) ? bearerToken[7..] : bearerToken;
            UnauthorizedException.ThrowIfTrue(string.IsNullOrWhiteSpace(token), ErrorMessage.InvalidToken);
            bool isValidToken = await  ValidateToken(token);
            UnauthorizedException.ThrowIfFalse(isValidToken, ErrorMessage.InvalidToken);
            JwtSecurityTokenHandler tokenHandler = new();
            JwtSecurityToken securityToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
            string? userIdValue = securityToken.GetClaimValue<string>(Constant.ClaimsIdUser.ToLower());
            Ensure.That(userIdValue, nameof(userIdValue)).NotNullOrEmpty(ErrorMessage.ClaimValueNotFound);
            string? userNamerValue = securityToken.GetClaimValue<string>(Constant.ClaimsUserName.ToLower());
            UnauthorizedException.ThrowIfTrue(string.IsNullOrWhiteSpace(userNamerValue), ErrorMessage.ClaimValueNotFound);
            User? user = await _userRepository.Get(w => w.IdUser.ToString() == userIdValue);
            UnauthorizedException.ThrowIfTrue(user == null, ErrorMessage.UserNotFound);
            UserInfo userInfo = _mapper.Map<UserInfo>(user);
            return userInfo;
        }



        private DateTime GetTokenLife()
        {
            return DateTime.UtcNow.AddSeconds(token_second_life).AddMinutes(token_minute_life).AddHours(token_hour_life);
        }

        public async Task<UserInfo> GetUserFromToken(HttpRequestData req)
        {
            string? bearerToken = req.GetHeaderValue<string>(Constant.authorization);
            UnauthorizedException.ThrowIfTrue(string.IsNullOrWhiteSpace(bearerToken),ErrorMessage.EmptyToken);
            string? token = bearerToken!.StartsWith($"{Constant.Bearer} ", StringComparison.InvariantCultureIgnoreCase) ? bearerToken[7..] : bearerToken;
            UnauthorizedException.ThrowIfTrue(string.IsNullOrWhiteSpace(token), ErrorMessage.InvalidToken);
            bool isValidToken = await ValidateToken(token);
            UnauthorizedException.ThrowIfFalse(isValidToken, ErrorMessage.InvalidToken);
            JwtSecurityTokenHandler tokenHandler = new();
            JwtSecurityToken securityToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
            string? userIdValue = securityToken.GetClaimValue<string>(Constant.ClaimsIdUser.ToLower());
            Ensure.That(userIdValue, nameof(userIdValue)).NotNullOrEmpty(ErrorMessage.ClaimValueNotFound);
            string? userNamerValue = securityToken.GetClaimValue<string>(Constant.ClaimsUserName.ToLower());
            UnauthorizedException.ThrowIfTrue(string.IsNullOrWhiteSpace(userNamerValue),ErrorMessage.ClaimValueNotFound);
            bool validGuid = Guid.TryParse(userIdValue, out var idGuid);
            BadRequestException.ThrowIfFalse(validGuid, ErrorMessage.WrongGuidFormat);
            User? user = await _userRepository.Get(w => w.IdUser == idGuid);
            UnauthorizedException.ThrowIfTrue(user== null, ErrorMessage.UserNotFound);
            UserInfo userInfo = _mapper.Map<UserInfo>(user);
            return userInfo;
        }

   
    }
}
