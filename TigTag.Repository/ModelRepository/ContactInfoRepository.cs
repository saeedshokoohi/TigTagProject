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

    public class ContactInfoRepository :
        GenericRepository<DataModelContext, ContactInfo>, IContactInfoRepository
    {
        PageRepository pageRepo = new PageRepository();

        public ContactInfo GetSingle(Guid Id)
        {

            var query = Context.ContactInfos.FirstOrDefault(x => x.Id == Id);
            return query;
        }

        public ResultDto validateContactInfo(ContactInfo contactInfoModel)
        {
            ResultDto retResult = new ResultDto();
            retResult.isDone = true;
            checkPageId(contactInfoModel, retResult);
            if (retResult.isDone)
                retResult.statusCode = enm_STATUS_CODE.DONE_SUCCESSFULLY;
            return retResult;

        }
        private void checkPageId(ContactInfo ContactInfo, ResultDto retResult)
        {
            var c = Context.Pages.Count(p => p.Id == ContactInfo.PageId);
            if (c == 0 || ContactInfo.PageId == null)
            {
                retResult.addValidationMessages("pageId is not valid or is null!");
            }


        }
    }
       
}