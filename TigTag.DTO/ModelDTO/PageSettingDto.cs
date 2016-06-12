using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TigTag.DTO.ModelDTO
{
   public class PageSettingDto :BaseDto
    {
        public System.Guid PageId { get; set; }
        public Nullable<bool> IsPublic { get; set; }
    }
}
