using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TigTag.DataModel.model;
using TigTag.DTO.ModelDTO.Base;
using TigTag.DTO.ModelDTO;
using TigTag.Repository.ModelRepository;

using TiTag.Repository;
using TigTag.Repository;

namespace TigTag.WebApi.Controllers
{
    public class EventsLogController : BaseController<EventsLog,EventsLogDto>
    {
 
        EventsLogRepository EventsLogRepo = new EventsLogRepository();

        public override IGenericRepository<EventsLog> getRepository()
        {
            return EventsLogRepo;
        }
      
        public List<EventsLogDto> getProfileEvents(Guid pageId)
        {
            return EventsLogRepo.geProfileEvents(pageId);
        }

     
    }
}