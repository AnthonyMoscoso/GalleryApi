using Core.Model;
using Model.Dto.Files;
using Model.Dto.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bsn.DataServices.ImageFiles.Interfaces
{
    public interface IImageFileGetDataServices
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
         Task<ImageFileDto> Get(string id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableParams"></param>
        /// <param name="search"></param>
        /// <param name="idAlbum"></param>
        /// <param name="all"></param>
        /// <returns></returns>
        Task<DataTable<ImageFileDto>> GetTable(TableParams tableParams, string? search = null, string? idAlbum = null, bool? all = false);
    }
}
