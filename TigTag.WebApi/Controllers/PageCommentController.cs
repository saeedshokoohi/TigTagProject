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
    public class PageCommentController : BaseController<PageComment,PageCommentDto>
    {
 
        PageCommentRepository PageCommentRepo = new PageCommentRepository();

        public override IGenericRepository<PageComment> getRepository()
        {
            return PageCommentRepo;
        }

        public ResultDto addPageComment(PageCommentDto pageComment)
        {
            if (pageComment == null) return ResultDto.failedResult("Invalid Raw Payload data, it must be an json object  ");
            else
            {
                ResultDto returnResult = new ResultDto();
                PageComment pageCommentModel = Mapper<PageComment, PageCommentDto>.convertToModel(pageComment);
                pageCommentModel.Id = Guid.NewGuid();
                pageCommentModel.CreateDate = DateTime.Now;
                returnResult = PageCommentRepo.validateMenu(pageCommentModel);
                if (returnResult.isDone)
                {
                    try
                    {
                        PageCommentRepo.Add(pageCommentModel);
                        PageCommentRepo.Save();
                        returnResult.isDone = true;
                        returnResult.message = "new Page Comment created successfully";
                        returnResult.returnId = pageCommentModel.Id.ToString();
                        eventLogRepo.AddCommentEvent(pageComment.ProfileId, pageCommentModel);
                    }
                    catch (Exception ex)
                    {
                        returnResult = ResultDto.exceptionResult(ex);


                    }

                }

                return returnResult;
            }
        }
              public List<PageCommentDto> getPageCommentsByPageId(Guid pageId)
        {
            return PageCommentRepo.getPageCommentsByPageId(pageId);
        }
    }

      
}