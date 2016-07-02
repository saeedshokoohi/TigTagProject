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
    public class PageScoreController : BaseController<PageScore,PageScoreDto>
    {
 
        PageScoreRepository PageScoreRepo = new PageScoreRepository();

        public override IGenericRepository<PageScore> getRepository()
        {
            return PageScoreRepo;
        }
        /// <summary>
        /// adding an score to a post
        /// </summary>
        /// <param name="PageScoreModelDto"></param>
        /// <returns></returns>
        public ResultDto addPageScore(PageScoreDto PageScoreModelDto)
        {
            if (PageScoreModelDto == null) return ResultDto.failedResult("Invalid Raw Payload data, it must be an json object  ");
            else
            {
                ResultDto returnResult = new ResultDto();
                PageScore paticipantMOdel = Mapper<PageScore, PageScoreDto>.convertToModel(PageScoreModelDto);
                paticipantMOdel.Id = Guid.NewGuid();
                paticipantMOdel.CreateDate = DateTime.Now;
                     returnResult = PageScoreRepo.validatePageScore(paticipantMOdel);
                if (returnResult.isDone)
                {
                    try
                    {
                        PageScoreRepo.Add(paticipantMOdel);
                        PageScoreRepo.Save();
                        returnResult.isDone = true;
                        returnResult.message = "new PageScore created successfully";
                        returnResult.returnId = paticipantMOdel.Id.ToString();
                    }
                    catch (Exception ex)
                    {
                        returnResult = ResultDto.exceptionResult(ex);
                    }
                }
                return returnResult;
            }
        }
        /// <summary>
        /// get Total Score for Average score of given page
        /// </summary>
        /// <param name="pageid"></param>
        /// <returns></returns>
        public TotalScoreDto getTotalPageScore(Guid pageid)
        {
            return PageScoreRepo.getTotalPageScore(pageid);
        }
        /// <summary>
        /// get Average Scores for a post
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        public TotalScoreDto getTotalPostScore(Guid postId)
        {
            return PageScoreRepo.getTotalPostScore(postId);
        }
    }
}