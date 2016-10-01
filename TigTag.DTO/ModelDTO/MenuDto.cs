using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TigTag.DTO.ModelDTO
{
    public class MenuDto:BaseDto
    {
       

        public string MenuTitle { get; set; }
        public long Score { get; set; }
        public string Category { get; set; }
        public System.Guid PageId { get; set; }
        public System.DateTime CreateDate { get; set; }
        public int usedCount { get; set; }
    }
}
