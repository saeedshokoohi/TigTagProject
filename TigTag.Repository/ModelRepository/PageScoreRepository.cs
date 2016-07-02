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

 
    public class PageScoreRepository : 
        GenericRepository<DataModelContext, PageScore>, IPageScoreRepository  {

        public override PageScore GetSingle(Guid Id) {

            var query = Context.PageScores.FirstOrDefault(x => x.Id ==Id );
            return query;
        }

        public ResultDto validatePageScore(PageScore prt)
        {
            ResultDto retResult = new ResultDto();
            retResult.isDone = true;
            checkPageId(prt, retResult);
          
            if (retResult.isDone)
                retResult.statusCode = enm_STATUS_CODE.DONE_SUCCESSFULLY;
            return retResult;

        }

        private void checkPageId(PageScore prt, ResultDto retResult)
        {
            if (Context.Pages.Count(p => p.Id == prt.ProfileId && p.PageType == profileTypeCode) == 0)
                retResult.addValidationMessages(" profile id is not valid!!");
            if (Context.Pages.Count(p => p.Id == prt.PageToScore) == 0)
                retResult.addValidationMessages("PageToScore is not valid!!");
            if(prt.Score>5 || prt.Score<1)
                retResult.addValidationMessages("Score must be a number between 1 to 5!!");

        }

        public TotalScoreDto getTotalPostScore(Guid postId)
        {
            TotalScoreDto retScore = new TotalScoreDto();
            var q = Context.PageScores.Where(ps => ps.PageToScore == postId);
            retScore.AverageScore= q.Average(p => p.Score);
            retScore.ScoresCount = q.Count();
            return retScore;


        }
        public TotalScoreDto getTotalPageScore(Guid pageid)
        {
            TotalScoreDto retScore = new TotalScoreDto();
             var postList=Context.Pages.Where(p => p.PageId == pageid && p.PageType==postTypeCode).ToList();
            Double? tempSum = 0;
            Double? tempCount = 0;
            foreach (var post in postList)
            {
                var ave = Context.PageScores.Where(ps => ps.PageToScore == post.Id).Average(p => p.Score);
                tempSum =tempSum+ave;
                tempCount=tempCount+1;
            }

            var q = Context.PageScores.Where(ps => ps.Page.PageId==pageid);
            retScore.AverageScore = tempSum / tempCount;
            retScore.ScoresCount = tempCount;
            return retScore;


        }
    }
}