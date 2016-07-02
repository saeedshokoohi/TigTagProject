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


    public class PageAdminRepository :
        GenericRepository<DataModelContext, PageAdmin>, IPageAdminRepository
    {

        public override PageAdmin GetSingle(Guid Id)
        {

            var query = Context.PageAdmins.FirstOrDefault(x => x.Id == Id);
            return query;
        }

        public ResultDto validatePageAdmin(PageAdmin prt)
        {
            ResultDto retResult = new ResultDto();
            retResult.isDone = true;
            checkPageId(prt, retResult);
           
            if (retResult.isDone)
                retResult.statusCode = enm_STATUS_CODE.DONE_SUCCESSFULLY;
            return retResult;

        }

        private void checkPageId(PageAdmin prt, ResultDto retResult)
        {
            if (Context.Pages.Count(p => p.Id == prt.AdminProfileId && p.PageType == profileTypeCode) == 0)
                retResult.addValidationMessages("Admin profile id is not valid!!");
            if (Context.Pages.Count(p => p.Id == prt.PageId) == 0)
                retResult.addValidationMessages("Page Id is not valid!!");

        }
        public List<PageAdminDto> getPageAdmins(Guid pageId)
        {
            return Mapper<PageAdmin, PageAdminDto>.convertListToDto(Context.PageAdmins.Where(pa => pa.PageId == pageId).Distinct().ToList());
        }
        public ResultDto removePageAdmin(Guid adminProfileId,Guid pageId )
        {
            try
            {
               var delList= Context.PageAdmins.Where(pa => pa.AdminProfileId == adminProfileId && pa.PageId == pageId).ToList();
                 DeleteList(delList);
                return ResultDto.successResult("","Admin Page Delete Successfully");
            }
            catch(Exception ex)
            {
                return ResultDto.exceptionResult(ex);
            }

        }
    }
}