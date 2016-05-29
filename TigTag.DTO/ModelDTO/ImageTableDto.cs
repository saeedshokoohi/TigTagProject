using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TigTag.DTO.ModelDTO
{
    public class ImageTableDto
        :BaseDto
    {
        public string ImageName { get; set; }
        public string ImageType { get; set; }
        public byte[] ImageData { get; set; }
        public byte[] ThumbnailData { get; set; }
    }
}
