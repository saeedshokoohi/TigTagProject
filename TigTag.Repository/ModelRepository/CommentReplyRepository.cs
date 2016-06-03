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

 
    public class CommentReplyRepository : 
        GenericRepository<DataModelContext, CommentReply>, ICommentReplyRepository  {

        public override CommentReply GetSingle(Guid Id) {

            var query = Context.CommentReplys.FirstOrDefault(x => x.Id ==Id );
            return query;
        }

        public ResultDto validateCommentReply(CommentReply commentReply)
        {
            ResultDto retResult = new ResultDto();
            retResult.isDone = true;
            checkPageCommentId(commentReply, retResult);
            if (retResult.isDone)
                retResult.statusCode = enm_STATUS_CODE.DONE_SUCCESSFULLY;
            return retResult;

        }

        public List<CommentReplyDto> getCommentReplysByCommentId(Guid pageCommentId)
        {
            return Mapper<CommentReply,CommentReplyDto>.convertListToDto( Context.CommentReplys.Where(cr => cr.PageCommentId == pageCommentId).OrderByDescending(c=>c.CreateDate ).ToList());
        }
        private void checkPageCommentId(CommentReply commentReply, ResultDto retResult)
        {
            var c = Context.PageComments.Count(p => p.Id == commentReply.PageCommentId);
            if (c == 0 || commentReply.PageCommentId == null)
            {
                retResult.addValidationMessages("pageCommentId is not valid or is null!");
            }
        }

       
    }
}