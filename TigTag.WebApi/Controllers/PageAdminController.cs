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
    public class PageAdminController : BaseController<PageAdmin,PageAdminDto>
    {
 
        PageAdminRepository PageAdminRepo = new PageAdminRepository();

        public override IGenericRepository<PageAdmin> getRepository()
        {
            return PageAdminRepo;
        }
        public ResultDto addPageAdmin(PageAdminDto PageAdminModelDto)
        {
            if (PageAdminModelDto == null) return ResultDto.failedResult("Invalid Raw Payload data, it must be an in json object format  ");
            else
            {
                ResultDto returnResult = new ResultDto();
                PageAdmin pageAdminModel = Mapper<PageAdmin, PageAdminDto>.convertToModel(PageAdminModelDto);
                pageAdminModel.Id = Guid.NewGuid();
                pageAdminModel.CreateDate = DateTime.Now;
                     returnResult = PageAdminRepo.validatePageAdmin(pageAdminModel);
                if (returnResult.isDone)
                {
                    try
                    {
                        removePageAdmin(PageAdminModelDto);
                        PageAdminRepo.Add(pageAdminModel);
                        PageAdminRepo.Save();
                        returnResult.isDone = true;
                        returnResult.message = "new PageAdmin created successfully";
                        returnResult.returnId = pageAdminModel.Id.ToString();
                        eventLogRepo.addPageAdminEvent(PageAdminModelDto.ProfileId, pageAdminModel);
                    }
                    catch (Exception ex)
                    {
                        returnResult = ResultDto.exceptionResult(ex);


                    }

                }
                return returnResult;
            }
        }

        public List<PageAdminDto> getPageAdmins(Guid pageId)
        {
          return  PageAdminRepo.getPageAdmins(pageId);
        }
        public ResultDto removePageAdmin(PageAdminDto pageAdminDto)
        {
            return PageAdminRepo.removePageAdmin(pageAdminDto.AdminProfileId, pageAdminDto.PageId);
        }
    }
}