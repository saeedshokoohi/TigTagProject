using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TigTag.DTO.ModelDTO
{
    public class FollowConditionDto
        :BaseDto
    {
        public System.Guid FollowId { get; set; }
        public System.Guid MenuId { get; set; }

    }
}
