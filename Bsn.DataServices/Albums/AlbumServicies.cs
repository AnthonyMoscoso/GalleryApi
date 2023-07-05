using AutoMapper;
using Bsn.AzureServices.BlobStorage;
using Bsn.AzureServices.BlobStorage.Interfaces;
using Bsn.AzureServices.Enums;
using Bsn.DataServices.Albums.Interfaces;
using Bsn.Utilities.Constants;
using Core.Model;
using Core.Utilities.Ensures;
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
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Bsn.DataServices.Albums
{
    public class AlbumServicies : IAlbumServicies, IAlbumGetServicies
    {
   
        private readonly IImageFileRepository _imageFileRepository;
        private readonly IAlbumImageRepository _albumImageRepository;
        private readonly IAlbumRepository _albumRepository;
        private readonly IValidator<AlbumInputDto> _validator;
        private readonly IAzureBlobStorageManager _azureBlobStorageManager;
        private readonly IMapper _mapper;
        public AlbumServicies(
            IImageFileRepository repository,
            IAlbumImageRepository albumImageRepository,
            IAlbumRepository albumRepository,
            IMapper mapper,
            IValidator<AlbumInputDto> validator,
            IAzureBlobStorageManager azureBlobStorageManager
           )
        {
            _imageFileRepository = repository;
            _albumImageRepository = albumImageRepository;
            _albumRepository = albumRepository;
            _mapper = mapper;
            _albumImageRepository = albumImageRepository;
            _validator = validator;
            _azureBlobStorageManager = azureBlobStorageManager;

        }
        public async Task Delete(string id)
        {
            Ensure.That(id, nameof(id)).NotNullOrEmpty();
            bool validGuid = Guid.TryParse(id, out var idGuid);
            BadRequestException.ThrowIfFalse(validGuid, ErrorMessage.WrongGuidFormat);
            Album? item = await _albumRepository.Get(idGuid) ?? throw new NotFoundException(ErrorMessage.AlbumNotFound);
            
            if (!item.AlbumImage.IsNullOrEmpty())
            {
                await _albumImageRepository.DeleteRange(item.AlbumImage);
            }
            await _albumRepository.Delete(idGuid);

        }

        public async Task<AlbumDto> Get(string id)
        {
            Ensure.That(id, nameof(id)).NotNullOrEmpty();
            bool validGuid = Guid.TryParse(id, out var idGuid);
            BadRequestException.ThrowIfFalse(validGuid, ErrorMessage.WrongGuidFormat);
            Album? item = await _albumRepository.Get(idGuid);
            NotFoundException.ThrowIfTrue(item == null, ErrorMessage.AlbumNotFound);
            AlbumDto itemDto = _mapper.Map<AlbumDto>(item);
            var images  = await _imageFileRepository.GetAll(w => w.AlbumImage.Any(w => w.IdAlbum == idGuid));
            itemDto.Images = _mapper.Map<ICollection<ImageFileDto>>(images);
            itemDto.TotalItems = images.Count();
            if (images != null) 
            {
                itemDto.UrlCover = images.OrderBy(w=> w.Updated).FirstOrDefault() != null ? images.First().Url : string.Empty;
                if (!string.IsNullOrWhiteSpace(itemDto.UrlCover))
                {
                    itemDto.UrlCover = await _azureBlobStorageManager.GetSasUrl(BlobContainers.images_container, itemDto.UrlCover);
                }

                foreach (var image in itemDto.Images)
                {
                    image.Url = await _azureBlobStorageManager.GetSasUrl(BlobContainers.images_container, image.Url);
                }
            }
            return itemDto;
        }

        public async Task<DataTable<AlbumDto>> GetTable(TableParams tableParams, string? search = null, bool? all = false)
        {
            tableParams ??= new();
            IQueryable<Album> itemsFromDbs = await _albumRepository.GetAll();
            
            if (!string.IsNullOrWhiteSpace(search))
            {
                itemsFromDbs = itemsFromDbs.Where(w => w.Name.ToLower().Contains(search.ToLower()));
            }
            IEnumerable<AlbumDto> items = _mapper.Map<IEnumerable<AlbumDto>>(itemsFromDbs);
            int total = items.Count();
            if (!string.IsNullOrWhiteSpace(tableParams.OrderBy))
            {
                items = items.AsQueryable().OrderBy(tableParams.OrderBy, tableParams.IsAsc);
            }
            if (all.HasValue && !all.Value)
            {
                items = items.Skip(tableParams.Skip).Take(tableParams.Take);
            }

            foreach (AlbumDto album in items)
            {
                bool validGuid = Guid.TryParse(album.IdAlbum, out var idGuid);
                BadRequestException.ThrowIfFalse(validGuid, ErrorMessage.WrongGuidFormat);
                var images = await _imageFileRepository.GetAll(w => w.AlbumImage.Any(w => w.IdAlbum == idGuid));
                album.TotalItems = images.Count();
                album.UrlCover = images.OrderBy(w => w.Updated).FirstOrDefault() != null ? images.First().Url : string.Empty;
                if (!string.IsNullOrWhiteSpace(album.UrlCover))
                {
                    album.UrlCover = await _azureBlobStorageManager.GetSasUrl(BlobContainers.images_container, album.UrlCover);
                }
         
            }
            return new DataTable<AlbumDto>()
            {
                Items = items,
                TotalItems = total,
            };
        }

        public async Task<AlbumDto> Insert(UserInfo userInfo,AlbumInputDto item)
        {
            Ensure.That(item, nameof(item)).IsNotNull();
            Ensure.That(userInfo, nameof(userInfo)).IsNotNull();


            ValidationResult? validateResult = await _validator.ValidateAsync(item);
            BadRequestException.ThrowIfFalse(validateResult.IsValid, JsonSerializer.Serialize(validateResult.Errors));
            validateResult.IsValid.ThrowExcetionIfFalse(new ValidationException(validateResult.Errors));
            Album itemToInsert = _mapper.Map<Album>(item);
            itemToInsert.Created = DateTime.UtcNow;
            itemToInsert.Updated = DateTime.UtcNow;
            itemToInsert.IdUserCreater = userInfo.IdUser.ToString();
            itemToInsert = await _albumRepository.Insert(itemToInsert);
           
            return await Get(itemToInsert.IdAlbum.ToString());
        }

        public async     Task<AlbumDto> Update(UserInfo userInfo, AlbumInputDto item, string idAlbum)
        {
            Ensure.That(userInfo, nameof(userInfo)).IsNotNull();
            Ensure.That(item, nameof(item)).IsNotNull();
            Ensure.That(idAlbum, nameof(idAlbum)).NotNullOrEmpty();
            bool validGuid = Guid.TryParse(idAlbum, out var idGuid);
            BadRequestException.ThrowIfFalse(validGuid, ErrorMessage.WrongGuidFormat);
            ValidationResult? validateResult = await _validator.ValidateAsync(item);
            BadRequestException.ThrowIfFalse(validateResult.IsValid, JsonSerializer.Serialize(validateResult.Errors));
            Album? search = await _albumRepository.Get(w => w.IdAlbum == idGuid);
            BadRequestException.ThrowIfTrue(search == null, ErrorMessage.ImageFileNotFound);
            Album? itemUpdate = _mapper.Map<Album>(item);
            itemUpdate.IdAlbum = idGuid;
            itemUpdate.Created = search!.Created;
            itemUpdate.Updated = DateTime.UtcNow;
            itemUpdate.IdUserCreater = search.IdUserCreater;
            itemUpdate.IdUserModifier = userInfo.IdUser.ToString();
            
            itemUpdate = await _albumRepository.Update(itemUpdate, idGuid);
            return await Get(itemUpdate!.IdAlbum.ToString());
        }
    }
}
