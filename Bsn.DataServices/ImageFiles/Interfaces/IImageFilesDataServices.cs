using Core.Model;
using Model.Dto.Album;
using Model.Dto.Auth;
using Model.Dto.Files;
using Model.Dto.Images;
using Model.Dto.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bsn.DataServices.ImageFiles.Interfaces
{
    public interface IImageFilesDataServices
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public Task<ImageFileDto> Insert(UserInfo userInfo, ImageFileInputDto item);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="items"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<ImageFileDto> Update(UserInfo userInfo, ImageFileInputDto items, string id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task Delete(string id);

    }
}
