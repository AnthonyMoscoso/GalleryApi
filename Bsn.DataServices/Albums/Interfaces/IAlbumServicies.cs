using Model.Dto.Album;
using Model.Dto.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bsn.DataServices.Albums.Interfaces
{
    public interface IAlbumServicies
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<AlbumDto> Insert(UserInfo userInfo,AlbumInputDto item);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="item"></param>
        /// <param name="idAlbum"></param>
        /// <returns></returns>
        Task<AlbumDto> Update(UserInfo userInfo, AlbumInputDto item, string idAlbum);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Delete(string id);
    }
}
