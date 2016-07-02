using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TigTag.DataModel.model;
using TigTag.DTO.ModelDTO;
using TigTag.DTO.ModelDTO.Base;
using TigTag.Repository.IModelRepository;
using TiTag.Repository.Base;


namespace TigTag.Repository.ModelRepository {

 
    public class EventsLogRepository : 
        GenericRepository<DataModelContext, EventsLog>, IEventsLogRepository  {

        public override EventsLog GetSingle(Guid Id) {

            var query = Context.EventsLogs.FirstOrDefault(x => x.Id ==Id );
            return query;
        }

        public ResultDto validateEventsLog(EventsLog EventsLog)
        {
            ResultDto retResult = new ResultDto();
            retResult.isDone = true;
            checkPageCommentId(EventsLog, retResult);
            if (retResult.isDone)
                retResult.statusCode = enm_STATUS_CODE.DONE_SUCCESSFULLY;
            return retResult;

        }

        private void checkPageCommentId(EventsLog EventsLog, ResultDto retResult)
        {
           
        }

       
    }
}