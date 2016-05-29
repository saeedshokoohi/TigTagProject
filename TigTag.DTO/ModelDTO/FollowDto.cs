using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TigTag.DTO.ModelDTO
{
    public class FollowDto
        :BaseDto
    {
        public System.Guid FollowerUserId { get; set; }
        public System.Guid FollowingPageId { get; set; }
        public string lastVisitDate { get; set; }
        public System.DateTime CreateDate { get; set; }
    }
}
