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

 
    public class PageCommentRepository : 
        GenericRepository<DataModelContext, PageComment>, IPageCommentRepository  {

        public override PageComment GetSingle(Guid Id) {

            var query = Context.PageComments.FirstOrDefault(x => x.Id ==Id );
            return query;
        }

        public ResultDto validateMenu(PageComment pageCommentModel)
        {
            ResultDto retResult = new ResultDto();
            retResult.isDone = true;
            checkPageId(pageCommentModel, retResult);
            if (retResult.isDone)
                retResult.statusCode = enm_STATUS_CODE.DONE_SUCCESSFULLY;
            return retResult;
        }

        private void checkPageId(PageComment pageCommentModel, ResultDto retResult)
        {
            var c= Context.Pages.Count(p => p.Id == pageCommentModel.PageId);
            if(c==0 || pageCommentModel.PageId==null)
            {
                retResult.addValidationMessages("pageId is not valid or is null!");
            }
        }

        public List<PageCommentDto> getPageCommentsByPageId(Guid pageId)
        {
            List<PageCommentDto> retList = new List<PageCommentDto>();
         var comments=   Context.PageComments.Where(pc => pc.PageId == pageId).OrderByDescending(c => c.CreateDate).ToList();
            foreach (var c in comments)
            {
                PageCommentDto dto = Mapper<PageComment, PageCommentDto>.convertToDto(c);
                int rc=Context.CommentReplys.Count(r => r.PageCommentId == c.Id);
                if (rc > 0)
                {
                    CommentReplyDto lastCommentReply = getLastCommentReply(c.Id);
                    dto.lastCommentReply = lastCommentReply;
                }
                dto.repliesCount = rc;
                retList.Add(dto);

            }
            return retList;


        }

        private CommentReplyDto getLastCommentReply(Guid id)
        {
            try
            {
                var lastReply = Context.CommentReplys.Where(cr => cr.PageCommentId == id).OrderByDescending(cr => cr.CreateDate).FirstOrDefault();
                return Mapper<CommentReply, CommentReplyDto>.convertToDto(lastReply);
            }
            catch
            {
                return null;
            }

        }
    }
}