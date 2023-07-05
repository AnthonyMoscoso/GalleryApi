using AutoMapper;
using Bsn.AzureServices.BlobStorage.Interfaces;
using Bsn.AzureServices.Enums;
using Bsn.DataServices.ImageFiles.Interfaces;
using Bsn.Utilities.Constants;
using Core.Model;
using Core.Utilities.Ensures;
using Core.Utilities.Enums;
using Core.Utilities.Exceptions;
using Core.Utilities.Extensions;
using FluentValidation;
using FluentValidation.Results;
using Infra.DataAccess.DBs.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Model.Dto.Album;
using Model.Dto.Auth;
using Model.Dto.Files;
using Model.Dto.Images;
using Model.Dto.Table;
using Model.Entity.DBs.Dbo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Bsn.DataServices.ImageFiles
{
    public class ImageFileDataServices : IImageFileGetDataServices, IImageFilesDataServices
    {
        private readonly IImageFileRepository _repository;
        private readonly IAlbumImageRepository _albumImageRepository;
        private readonly IAlbumRepository _albumRepository;
        private IValidator<ImageFileInputDto> _validator;
        
        private readonly IMapper _mapper;
        private readonly IAzureBlobStorageManager _azureBlobStorageManager;
        public ImageFileDataServices(
            IImageFileRepository repository,
            IAlbumImageRepository albumImageRepository,
            IAlbumRepository albumRepository,
            IMapper mapper,
            IAzureBlobStorageManager azureBlobStorageManager,
            IValidator<ImageFileInputDto> validator
           )
        {
            _repository = repository;
            _albumImageRepository = albumImageRepository;
            _albumRepository = albumRepository;
            _mapper = mapper;
            _albumImageRepository = albumImageRepository;   
            _azureBlobStorageManager = azureBlobStorageManager;
            _validator = validator;

        }
        public async Task Delete(string id)
        {
            Ensure.That(id,nameof(id)).NotNullOrEmpty();
            bool validGuid =  Guid.TryParse(id, out var idGuid);
            BadRequestException.ThrowIfFalse(validGuid, ErrorMessage.WrongGuidFormat);
            ImageFile? item = await _repository.Get(idGuid) ?? throw new NotFoundException(ErrorMessage.ImageFileNotFound);
            //Delete the item and its associated image from the Azure Blob Storage
            if (!string.IsNullOrWhiteSpace(item.Url))
            {
                await _azureBlobStorageManager.Delete(BlobContainers.images_container, item.Url);
            }
            if (!item.AlbumImage.IsNullOrEmpty())
            {
                await _albumImageRepository.DeleteRange(item.AlbumImage);
            }
            await _repository.Delete(idGuid);

        }

        public async Task<ImageFileDto> Get(string id)
        {
            Ensure.That(id, nameof(id)).NotNullOrEmpty();
            bool validGuid = Guid.TryParse(id, out var idGuid);
            BadRequestException.ThrowIfFalse(validGuid, ErrorMessage.WrongGuidFormat);
            ImageFile? item = await _repository.Get(idGuid);
            NotFoundException.ThrowIfTrue(item == null, ErrorMessage.ImageFileNotFound);
            if (!string.IsNullOrWhiteSpace(item!.Url))
            {
                item.Url = await _azureBlobStorageManager.GetSasUrl(BlobContainers.images_container, item.Url);
            }
            ImageFileDto itemDto = _mapper.Map<ImageFileDto>(item);
            return itemDto;
        }

        public async Task<DataTable<ImageFileDto>> GetTable(TableParams tableParams, string? search = null, string? idAlbum = null, bool? all = false)
        {
            tableParams ??= new();
            IQueryable<ImageFile> itemsFromDbs = await _repository.GetAll();
            if (!string.IsNullOrWhiteSpace(idAlbum))
            {
                itemsFromDbs = itemsFromDbs.Where(w => w.AlbumImage.Any(w=> w.IdAlbum.Equals(idAlbum)));
            }
            if (!string.IsNullOrWhiteSpace(search))
            {
                itemsFromDbs = itemsFromDbs.Where(w => w.Name.ToLower().Contains(search.ToLower()));
            }
            IEnumerable<ImageFileDto> items = _mapper.Map<IEnumerable<ImageFileDto>>(itemsFromDbs);
            int total = items.Count();
            if (!string.IsNullOrWhiteSpace(tableParams.OrderBy))
            {
                items = items.AsQueryable().OrderBy(tableParams.OrderBy, tableParams.IsAsc);
            }
            if (all.HasValue && !all.Value)
            {
                items = items.Skip(tableParams.Skip).Take(tableParams.Take);
            }
            foreach (ImageFileDto item in items)
            {
                if (!string.IsNullOrWhiteSpace(item!.Url))
                {
                    item.Url = await _azureBlobStorageManager.GetSasUrl(BlobContainers.images_container, item.Url);
                }
            }
            return new DataTable<ImageFileDto>()
            {
                Items = items,
                TotalItems = total,
            };
        }

        public async Task<ImageFileDto> Insert(UserInfo userInfo, ImageFileInputDto item)
        {
            Ensure.That(item, nameof(item)).IsNotNull();
            Ensure.That(userInfo, nameof(userInfo)).IsNotNull();
            Ensure.That(item.Base64,nameof(item.Base64)).NotNullOrEmpty();

            ValidationResult? validateResult = await _validator.ValidateAsync(item);
            BadRequestException.ThrowIfFalse(validateResult.IsValid,JsonSerializer.Serialize(validateResult.Errors));
            validateResult.IsValid.ThrowExcetionIfFalse(new ValidationException(validateResult.Errors));
             ImageFile itemToInsert = _mapper.Map<ImageFile>(item);
            itemToInsert.Created = DateTime.UtcNow;
            itemToInsert.Updated = DateTime.UtcNow;
            itemToInsert.IdUserCreater = userInfo.IdUser.ToString();
            itemToInsert = await _repository.Insert(itemToInsert);
            string? fileBase64 = item.Base64;
            if (!string.IsNullOrWhiteSpace(fileBase64) && fileBase64.IsBase64String())
            {
                string filename = GenerateFileName(itemToInsert);
                bool success = await _azureBlobStorageManager.Upload(fileBase64, filename, BlobContainers.images_container);
                itemToInsert.Url = success ? filename : string.Empty;
                await _repository.Update(itemToInsert, itemToInsert.IdImage);
            }
            return await Get(itemToInsert.IdImage.ToString());
        }

        public async Task<ImageFileDto> Update(UserInfo userInfo, ImageFileInputDto item, string id)
        {
            Ensure.That(userInfo, nameof(userInfo)).IsNotNull();
            Ensure.That(item, nameof(item)).IsNotNull();
            Ensure.That(id, nameof(id)).NotNullOrEmpty();
            bool validGuid = Guid.TryParse(id, out var idGuid);
            BadRequestException.ThrowIfFalse(validGuid, ErrorMessage.WrongGuidFormat);
            ValidationResult? validateResult = await _validator.ValidateAsync(item);
            BadRequestException.ThrowIfFalse(validateResult.IsValid, JsonSerializer.Serialize(validateResult.Errors));
            ImageFile? search = await _repository.Get(w => w.IdImage == idGuid);
            BadRequestException.ThrowIfTrue(search == null, ErrorMessage.ImageFileNotFound);
            ImageFile? itemUpdate = _mapper.Map<ImageFile>(item);
            itemUpdate.IdImage = idGuid;
            itemUpdate.Url = search!.Url;
            itemUpdate.Created = search.Created;
            itemUpdate.Updated = DateTime.UtcNow;
            itemUpdate.IdUserCreater = search.IdUserCreater;
            itemUpdate.IdUserModifier = userInfo.IdUser.ToString();
            string? fileBase64 = item.Base64;
          
            if (!string.IsNullOrWhiteSpace(fileBase64) && fileBase64.IsBase64String())
            {
                string filename = GenerateFileName(itemUpdate);
                bool success = await _azureBlobStorageManager.Upload(fileBase64, filename, BlobContainers.images_container);
                itemUpdate.Url = success ? filename : string.Empty;

            }
            else if (fileBase64 == null && !string.IsNullOrWhiteSpace(itemUpdate.Url))
            {
                string filename = GenerateFileName(itemUpdate);
                bool success = await _azureBlobStorageManager.Delete(BlobContainers.images_container, filename);
                itemUpdate.Url = string.Empty;
            }
            itemUpdate = await _repository.Update(itemUpdate, idGuid);
            return await Get(itemUpdate!.IdImage.ToString());
        }

        private static string GenerateFileName(ImageFile item) => $"imagefile_{item.IdImage}.{FileFormat.PNG}";

       
    }
}
