using Core.Model;
using Model.Dto.Album;
using Model.Dto.Files;
using Model.Dto.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bsn.DataServices.Albums.Interfaces
{
    public interface IAlbumGetServicies
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<AlbumDto> Get(string id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableParams"></param>
        /// <param name="search"></param>
        /// <param name="all"></param>
        /// <returns></returns>
        Task<DataTable<AlbumDto>> GetTable(TableParams tableParams, string? search = null,  bool? all = false);
    }
}
