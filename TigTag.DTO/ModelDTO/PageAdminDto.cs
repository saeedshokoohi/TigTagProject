using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TigTag.DTO.ModelDTO
{
    public class PageAdminDto:BaseDto
    {
        public System.Guid AdminProfileId { get; set; }
        public System.Guid PageId { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
    }
}
