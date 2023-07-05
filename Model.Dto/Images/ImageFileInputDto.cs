using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dto.Images
{
    public class ImageFileInputDto
    { 
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? Base64 { get; set; }
        
    }
}
