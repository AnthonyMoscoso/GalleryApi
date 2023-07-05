using Bsn.Utilities.Constants;
using Core.Utilities.Ensures;
using Core.Utilities.Exceptions;
using Model.Dto.Album;
using Model.Dto.Files;
using Model.Entity.DBs.Dbo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Bsn.DataServices.AlbumImages.Interfaces
{
    public interface IAlbumImageServices
    {
        Task AddImageToAlbum(string idAlbum, AlbumImageInputDto item);

        Task RemoveImageInalbum(string idAlbum,string idImage);
    }
}
