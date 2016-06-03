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
    public class CommentReplyController : BaseController<CommentReply,CommentReplyDto>
    {
 
        CommentReplyRepository CommentReplyRepo = new CommentReplyRepository();

        public override IGenericRepository<CommentReply> getRepository()
        {
            return CommentReplyRepo;
        }
        public ResultDto addCommentReply(CommentReplyDto commentReply)
        {
            if (commentReply == null) return ResultDto.failedResult("Invalid Raw Payload data, it must be an json object  ");
            else
            {
                ResultDto returnResult = new ResultDto();
                CommentReply pageCommentModel = Mapper<CommentReply, CommentReplyDto>.convertToModel(commentReply);
                pageCommentModel.Id = Guid.NewGuid();
                pageCommentModel.CreateDate = DateTime.Now;
                returnResult = CommentReplyRepo.validateCommentReply(pageCommentModel);
                if (returnResult.isDone)
                {
                    try
                    {
                        CommentReplyRepo.Add(pageCommentModel);
                        CommentReplyRepo.Save();
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

        public List<CommentReplyDto> getCommentReplysByPageCommentId(Guid pageCommentId)
        {
            return CommentReplyRepo.getCommentReplysByCommentId(pageCommentId);
        }
    }
}