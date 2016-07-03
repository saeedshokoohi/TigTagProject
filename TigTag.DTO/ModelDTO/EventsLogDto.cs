using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TigTag.Common.Enumeration;

namespace TigTag.DTO.ModelDTO
{
    public class EventsLogDto
        :BaseDto
    {
        public System.DateTime CreateDate { get; set; }
        public Nullable<System.Guid> ActorPageId { get; set; }
        public Nullable<System.Guid> TargetPageId { get; set; }
        public Nullable<int> ActionCode { get; set; }
        public Nullable<System.Guid> OwnerPageId { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string Description { get; set; }
        public string ActionCodeStr {
            get { try { return ((enmEventsActionType)ActionCode).ToString(); } catch { return ""; } }
        }
        public Nullable<System.Guid> CustomRelatedId { get; set; }
    }
}
