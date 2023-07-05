using AutoMapper;
using Bsn.AzureServices.BlobStorage.Interfaces;
using Bsn.AzureServices.Enums;
using Bsn.DataServices.Users.Interfaces;
using Bsn.EncrypterService;
using Bsn.Utilities.Constants;
using Core.Model.ValueObjects;
using Core.Utilities;
using Core.Utilities.Ensures;
using Core.Utilities.Enums;
using Core.Utilities.Exceptions;
using Core.Utilities.Extensions;
using FluentValidation;
using FluentValidation.Results;
using Infra.DataAccess.DBs.Interfaces;
using Infra.DataAccess.Interfaces;
using Model.Dto.Auth;
using Model.Dto.User;
using Model.Entity.DBs.Auth;
using Model.Entity.DBs.Dbo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Bsn.DataServices.Users
{
    public class UserServicies : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly ICredentialsRepository _credentialsRepository;
        private readonly IMapper _mapper;
        private readonly IAzureBlobStorageManager _azureStoreManager;
        private readonly IEncrypterService _encrypter;
        private readonly IValidator<UserInputDto> _validator;

        public UserServicies(IUserRepository repository,
            IMapper mapper,
            ICredentialsRepository credentialsRepository,
            IAzureBlobStorageManager azureStoreManager,
            IEncrypterService encrypter,

            IValidator<UserInputDto> validator)

        {
            _repository = repository;
            _mapper = mapper;
            _credentialsRepository = credentialsRepository;
            _azureStoreManager = azureStoreManager;
            _encrypter = encrypter;
            _validator = validator;

        }
        public async Task<UserDto> Get(string idUser)
        {
            bool validGuid = Guid.TryParse(idUser, out var idGuid);
            BadRequestException.ThrowIfFalse(validGuid, ErrorMessage.WrongGuidFormat);
            User? user = await _repository.Get(idGuid);
            NotFoundException.ThrowIfTrue(user == null);
            UserDto userDto = _mapper.Map<UserDto>(user);
            if (!string.IsNullOrWhiteSpace(user!.UrlImage))
            {
                userDto.UrlImage = await _azureStoreManager.GetSasUrl(BlobContainers.user_profile, user.UrlImage);
            }
            userDto.Username = _encrypter.Decrypt(userDto.Username);
            return userDto;
        }
        public async Task<UserDto> Insert(UserInfo userInfo,UserInputDto user)
        {

            Ensure.That(user, nameof(user)).IsNotNull();
            Ensure.That(userInfo, nameof(userInfo)).IsNotNull();

            User info = await _repository.Get(userInfo.IdUser) ?? throw new NotFoundException(ErrorMessage.UserNotFound);
            ValidationResult? validateResult = await _validator.ValidateAsync(user);
            BadRequestException.ThrowIfFalse(validateResult.IsValid, JsonSerializer.Serialize(validateResult.Errors));
            Ensure.That(user.Password, nameof(user.Password)).NotNullOrEmpty(ErrorMessage.InvalidUserPasswordFormat);
            User? user_search = await _repository.Get(w => w.Name.ToLower().Equals(user.Name));
            BadRequestException.ThrowIfTrue(user_search != null, ErrorMessage.UserWithSameNameFound);
            User userToInsert = _mapper.Map<User>(user);
            BadRequestException.ThrowIfFalse(Base64.TryDecode(user.Password), ErrorMessage.InvalidUserPasswordFormat);
            BadRequestException.ThrowIfFalse(Base64.TryDecode(user.Username), ErrorMessage.InvalidUsernameFormat);
            string password = _encrypter.Encrypt(Base64.Decode(user.Password));
            string user_name = _encrypter.Encrypt(Base64.Decode(user.Username));
            userToInsert.Name = user.Name;
            Credentials credentials = new()
            {
                Username = user_name,
                Password = password,
                
            };
          
            userToInsert.Credentials = credentials;
            
            userToInsert = await _repository.Insert(userToInsert);
            string? fileBase64 = user.Base64;

            if (!string.IsNullOrWhiteSpace(fileBase64) && fileBase64.IsBase64String())
            {
                string filename = $"{nameof(User).ToLower()}_{userToInsert.IdUser}.{FileFormat.PNG}";
                bool success = await _azureStoreManager.Upload(fileBase64, filename, BlobContainers.user_profile);
                userToInsert.UrlImage = success ? filename : string.Empty;
                await _repository.Update(userToInsert, userToInsert.IdUser);
            }

            UserDto dto = await Get(userToInsert.IdUser.ToString());
            return dto;
        }
    }
}
