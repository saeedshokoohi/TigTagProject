using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TigTag.DTO.ModelDTO
{
    public class ParticipantDto:BaseDto
    {
        public System.DateTime CreateDate { get; set; }
        public System.Guid ParticipantPageId { get; set; }
        public System.Guid PageId { get; set; }
        public Nullable<int> RequestStatus { get; set; }
    }
}
