using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TigTag.Common.Enumeration;
using TigTag.DataModel.model;
using TigTag.DTO.ModelDTO;
using TigTag.DTO.ModelDTO.Base;
using TigTag.Repository.IModelRepository;
using TiTag.Repository.Base;


namespace TigTag.Repository.ModelRepository {

 
    public class ParticipantRepository : 
        GenericRepository<DataModelContext, Participant>, IParticipantRepository  {

        public override Participant GetSingle(Guid Id) {

            var query = Context.Participants.FirstOrDefault(x => x.Id ==Id );
            return query;
        }

        public ResultDto validateParticipant(Participant prt)
        {
            ResultDto retResult = new ResultDto();
            checkPageId(prt, retResult);
            retResult.isDone = true;
            if (retResult.isDone)
                retResult.statusCode = enm_STATUS_CODE.DONE_SUCCESSFULLY;
            return retResult;

        }

        private void checkPageId(Participant prt, ResultDto retResult)
        {
            
          var c1= Context.Pages.Count(p => p.Id == prt.PageId && p.PageType !=postTypeCode );
          var c2 = Context.Pages.Count(p => p.Id == prt.ParticipantPageId && p.PageType == profileTypeCode);
            if (c1 == 0) retResult.addValidationMessages("PageId is not valid");
            if (c2 == 0) retResult.addValidationMessages("ParticipantPageId is not valid");

        }
    }
}