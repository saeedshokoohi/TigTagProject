using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TigTag.DTO.ModelDTO
{
    public class PageScoreDto:BaseDto
    {
        public System.Guid ProfileId { get; set; }
        public System.Guid PageToScore { get; set; }
        public Nullable<int> Score { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
    }
}
