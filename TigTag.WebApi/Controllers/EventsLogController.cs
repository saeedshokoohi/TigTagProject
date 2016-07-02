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
        public ResultDto addEventsLog(EventsLogDto EventsLogModelDto)
        {
            if (EventsLogModelDto == null) return ResultDto.failedResult("Invalid Raw Payload data, it must be an json object  ");
            else
            {
                ResultDto returnResult = new ResultDto();
                EventsLog paticipantMOdel = Mapper<EventsLog, EventsLogDto>.convertToModel(EventsLogModelDto);
                paticipantMOdel.Id = Guid.NewGuid();
                paticipantMOdel.CreateDate = DateTime.Now;
                     returnResult = EventsLogRepo.validateEventsLog(paticipantMOdel);
                if (returnResult.isDone)
                {
                    try
                    {
                        EventsLogRepo.Add(paticipantMOdel);
                        EventsLogRepo.Save();
                        returnResult.isDone = true;
                        returnResult.message = "new EventsLog created successfully";
                        returnResult.returnId = paticipantMOdel.Id.ToString();
                    }
                    catch (Exception ex)
                    {
                        returnResult = ResultDto.exceptionResult(ex);


                    }

                }
                return returnResult;
            }
        }

     
    }
}