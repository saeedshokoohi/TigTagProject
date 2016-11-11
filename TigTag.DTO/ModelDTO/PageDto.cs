using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TigTag.DTO.ModelDTO
{
   public class PageDto :BaseDto
    {
      

        public string PageTitle { get; set; }
        public string Description { get; set; }
        public System.DateTime CreateDate { get; set; }
        public int PageType { get; set; }
        public System.Guid UserId { get; set; }
        public Nullable<System.Guid> PageId { get; set; }
        public Nullable<System.Guid> ImageId { get; set; }
        public string URL { get; set; }
        public Nullable<bool> IsMasterPage { get; set; }
        public Nullable<bool> IsPublic { get; set; }
        public string Color { get; set; }
        public Guid[] Menulist { get; set; }
        public int newPostCount { get; set; }
        public bool IsFollowingByCurrentUser { get; set; }
        public PageStatsDto PageStats { get; set; }
        public List<MenuDto> PublicMenuDtoList { get; set; }
        public List<MenuDto> CustomMenuDtoList { get; set; }
        public List<string> newMenuTitleList { get; set; }
        public PageDto ParentPageDto { get; set; }
        public PageDto CreatedByUserDto { get; set; }
        public List<ContactInfoDto> ContactInfoList { get; set; }
        public List<TicketDto> TicketList { get; set; }
        public int newEventsCount { get; set; }
    }
}
