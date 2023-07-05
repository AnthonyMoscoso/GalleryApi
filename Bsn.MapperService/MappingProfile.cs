using AutoMapper;
using Model.Dto.Album;
using Model.Dto.Auth;
using Model.Dto.Files;
using Model.Dto.Images;
using Model.Entity.DBs.Auth;
using Model.Entity.DBs.Dbo;

namespace Bsn.MapperService
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Credentials, UserInfo>().ForMember(w => w.IdUser, x => x.MapFrom(z => z.IdUser));
            #region User
           
            CreateMap<User, UserInfo>();
            #endregion

            CreateMap<ImageFileInputDto, ImageFile>().ReverseMap();
            CreateMap<ImageFileDto, ImageFile>().ReverseMap();

            CreateMap<AlbumImageInputDto,AlbumImage>().ReverseMap();

            CreateMap<AlbumInputDto, Album>().ReverseMap();
            CreateMap<AlbumDto, Album>().ReverseMap();
        }
    }
}
