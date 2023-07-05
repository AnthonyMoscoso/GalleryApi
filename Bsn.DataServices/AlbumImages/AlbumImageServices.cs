using AutoMapper;
using Bsn.DataServices.AlbumImages.Interfaces;
using Bsn.Utilities.Constants;
using Core.Utilities.Ensures;
using Core.Utilities.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using Infra.DataAccess.DBs.Interfaces;
using Model.Dto.Album;
using Model.Dto.Files;
using Model.Entity.DBs.Dbo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Bsn.DataServices.AlbumImages
{
    public class AlbumImageServices : IAlbumImageServices
    {
        private readonly IAlbumRepository _albumrepository;
        private readonly IImageFileRepository _imageFileRepository;
        private readonly IAlbumImageRepository _albumImageRepository;
        private readonly IValidator<AlbumImageInputDto> _validator;
        private readonly IMapper _mapper;
        public AlbumImageServices(
            IAlbumImageRepository albumImageRepository ,
            IAlbumRepository albumRepository,
            IImageFileRepository imageFileRepository,
            IValidator<AlbumImageInputDto> validator,
            IMapper mapper) 
        { 
            _albumImageRepository = albumImageRepository;
            _albumrepository = albumRepository;
            _imageFileRepository = imageFileRepository;
            _validator = validator;
            _mapper = mapper;
        }
        public async Task AddImageToAlbum(string idAlbum, AlbumImageInputDto item)
        {
            Ensure.That(item, nameof(item)).IsNotNull();
            ValidationResult? validateResult = await _validator.ValidateAsync(item);
            BadRequestException.ThrowIfFalse(validateResult.IsValid, JsonSerializer.Serialize(validateResult.Errors));
            bool validGuid = Guid.TryParse(idAlbum, out var idGuid);
            BadRequestException.ThrowIfFalse(validGuid, ErrorMessage.WrongGuidFormat);
            Album? album = await _albumrepository.Get(idGuid) ?? throw new NotFoundException(ErrorMessage.AlbumNotFound);
            IList<AlbumImage> albumImages = new List<AlbumImage>();
            foreach (string idImage in item.IdImages)
            {
                validGuid = Guid.TryParse(idImage, out var idGuidImage);
                BadRequestException.ThrowIfFalse(validGuid, ErrorMessage.WrongGuidFormat);
                ImageFile? imageFile = await _imageFileRepository.Get(idGuidImage) ?? throw new NotFoundException(ErrorMessage.ImageFileNotFound);
                AlbumImage? albumImage = await _albumImageRepository.Get(w => w.IdImage == idGuidImage && w.IdAlbum == idGuid);
                BadRequestException.ThrowIfTrue(albumImage != null, ErrorMessage.ImageIsAlreadyInAlbum);
                albumImages.Add(new() { IdAlbum = idGuid,IdImage = idGuidImage});
            }

            await _albumImageRepository.InsertRange(albumImages);
        }

        
        public async Task RemoveImageInalbum(string idAlbum, string idImage)
        {
            Ensure.That(idAlbum, nameof(idAlbum)).NotNullOrEmpty();
            Ensure.That(idImage, nameof(idImage)).NotNullOrEmpty();
            bool validGuid = Guid.TryParse(idAlbum, out var guidAlbum);
            BadRequestException.ThrowIfFalse(validGuid, ErrorMessage.WrongGuidFormat);
            Album? album = await _albumrepository.Get(guidAlbum) ?? throw new NotFoundException(ErrorMessage.AlbumNotFound);
            validGuid = Guid.TryParse(idImage, out var idGuid);
            BadRequestException.ThrowIfFalse(validGuid, ErrorMessage.WrongGuidFormat);
            ImageFile? imageFile = await _imageFileRepository.Get(idGuid) ?? throw new NotFoundException(ErrorMessage.ImageFileNotFound);
            AlbumImage? albumImage = await _albumImageRepository.Get(w => w.IdImage == idGuid && w.IdAlbum == guidAlbum);
            Ensure.That(albumImage!= null, nameof(albumImage)).IsNotNull();
            await _albumImageRepository.Delete(albumImage!.IdAlbumImage);
        }
    }
}
