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
    public class PageSettingController : BaseController<PageSetting,PageSettingDto>
    {
 
        PageSettingRepository PageSettingRepo = new PageSettingRepository();

        public override IGenericRepository<PageSetting> getRepository()
        {
            return PageSettingRepo;
        }
        public ResultDto addPageSetting(PageSettingDto commentReply)
        {
            if (commentReply == null) return ResultDto.failedResult("Invalid Raw Payload data, it must be an json object  ");
            else
            {
                ResultDto returnResult = new ResultDto();
                PageSetting pageCommentModel = Mapper<PageSetting, PageSettingDto>.convertToModel(commentReply);
                pageCommentModel.Id = Guid.NewGuid();
           
                returnResult = PageSettingRepo.validatePageSetting(pageCommentModel);
                if (returnResult.isDone)
                {
                    try
                    {
                        PageSettingRepo.Add(pageCommentModel);
                        PageSettingRepo.Save();
                        returnResult.isDone = true;
                        returnResult.message = "new comment Reply created successfully";
                        returnResult.returnId = pageCommentModel.Id.ToString();
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