using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TigTag.DTO.ModelDTO
{
    public class FollowMenuDto
        :BaseDto
    {
        public string title { get; set; }
        public System.DateTime date { get; set; }
        public string lastVisitDate { get; set; }
        public System.Guid FollowerUserId { get; set; }
        public Guid[] menuIdList { get; set; }
        public int newPostCount { get; set; }
    }
}
