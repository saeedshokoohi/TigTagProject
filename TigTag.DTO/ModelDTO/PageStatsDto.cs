using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TigTag.DTO.ModelDTO
{
    public class PageStatsDto
    {
        public int postCount { get; set; }
        public int pagesCount { get; set; }
        public int followersCount { get; set; }
        public int followingsCount { get; set; }
        public int participantsCount { get; set; }
        //public int newEventLogsCount { get; set; }
    }
}
