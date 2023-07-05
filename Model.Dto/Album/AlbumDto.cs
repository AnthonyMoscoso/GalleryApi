using Model.Dto.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dto.Album
{
    public class AlbumDto
    {
        public AlbumDto() 
        {
            Images = new HashSet<ImageFileDto>();
        }
        public string IdAlbum { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public DateTimeOffset Created { get; set; }
        public string UrlCover { get;set; } = string.Empty;
        public int TotalItems { get; set; } = 0;
        public ICollection<ImageFileDto> Images { get; set; }
    }
}
