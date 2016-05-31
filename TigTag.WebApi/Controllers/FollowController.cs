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
    public class FollowController : BaseController<Follow,FollowDto>
    {
 
        FollowRepository followRepo = new FollowRepository();
        MenuRepository menuRepo = new MenuRepository();
        FollowConditionRepository followConditionRepo = new FollowConditionRepository();

        public override IGenericRepository<Follow> getRepository()
        {
            return followRepo;
        }
        /// <summary>
        /// a service to follow a page and add follow condition
        /// </summary>
        /// <param name="follow"></param>
        /// <returns></returns>
        public ResultDto followProfile([FromBody]FollowDto follow)
        {
            if (follow == null) return ResultDto.failedResult("Invalid Raw Payload data, it must be an json object");
            ResultDto returnResult = new ResultDto();
            Follow followModel = Mapper<Follow, FollowDto>.convertToModel(follow);
            followModel.Id = Guid.NewGuid();
            followModel.CreateDate = DateTime.Now;
         
                returnResult = followRepo.validateFollow(followModel);
            if (returnResult.isDone)
            {
                try
                {
                    followRepo.Add(followModel);
                    followRepo.Save();
                    //adding menu list to follow
                    if (follow.menuIdList != null)
                        foreach (var menuid in follow.menuIdList)
                        {
                            var menu= menuRepo.GetSingle(menuid);
                            if (menu != null)
                            {
                                FollowCondition followCondition = new FollowCondition();
                                followCondition.Id = Guid.NewGuid();
                                followCondition.MenuId = menuid;
                                followCondition.FollowId = followModel.Id;
                                followConditionRepo.Add(followCondition);
                                followConditionRepo.Save();
                            }

                        }

                    returnResult.isDone = true;
                    returnResult.message = "new follow created successfully";
                    returnResult.returnId = followModel.Id.ToString();
                }
                catch (Exception ex)
                {
                    returnResult = ResultDto.exceptionResult(ex);


                }
            }
            else
            {
                returnResult.message = "input is not valid. check the validation messages";
            }
            return returnResult;
        }
    }
}