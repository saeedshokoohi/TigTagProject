using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TigTag.DTO.ModelDTO
{
    public class ContactInfoDto:BaseDto
    {
        public System.Guid PageId { get; set; }
        public int ContactType { get; set; }
        public string Title { get; set; }
        public string Value { get; set; }
    }
}
