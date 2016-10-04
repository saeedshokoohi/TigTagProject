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

 
    public class PageSettingRepository : 
        GenericRepository<DataModelContext, PageSetting>, IPageSettingRepository  {

        public override PageSetting GetSingle(Guid Id) {

            var query = Context.PageSettings.FirstOrDefault(x => x.Id ==Id );
            return query;
        }



        public ResultDto validatePageSetting(PageSetting commentReply)
        {
            ResultDto retResult = new ResultDto();
            retResult.isDone = true;
         
            if (retResult.isDone)
                retResult.statusCode = enm_STATUS_CODE.DONE_SUCCESSFULLY;
            return retResult;

        }

        public void DeleteByPageid(Guid pageid)
        {
           var psl=  Context.PageSettings.Where(ps => ps.PageId == pageid);
            Context.PageSettings.RemoveRange(psl);
            Save();
        }
    }
}