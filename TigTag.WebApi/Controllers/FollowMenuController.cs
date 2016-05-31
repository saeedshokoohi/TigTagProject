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
    public class FollowMenuController : BaseController<FollowMenu,FollowMenuDto>
    {
 
        FollowMenuRepository followMenuRepo = new FollowMenuRepository();
        MenuRepository menuRepo = new MenuRepository();
        FollowMenuPackageRepository followMenuPackageRepo = new FollowMenuPackageRepository();

        public override IGenericRepository<FollowMenu> getRepository()
        {
            return followMenuRepo;
        }
        public ResultDto createFollowMenu([FromBody]FollowMenuDto follow)
        {
            if (follow == null) return ResultDto.failedResult("Invalid Raw Payload data, it must be an json object");
            ResultDto returnResult = new ResultDto();
            FollowMenu followModel = Mapper<FollowMenu, FollowMenuDto>.convertToModel(follow);
            followModel.Id = Guid.NewGuid();
            followModel.date = DateTime.Now;

            if (follow.menuIdList == null || follow.menuIdList.Length == 0)
                returnResult = ResultDto.invalidResult("Follow menu must have atleast one menulistitem");
                returnResult = followMenuRepo.validateFollowMenu(followModel);
          
            if (returnResult.isDone)
            {
                try
                {
                   

                    //adding menu list to follow
                    String title = "";
                    if (follow.menuIdList != null)
                        foreach (var menuid in follow.menuIdList)
                        {
                            var menu = menuRepo.GetSingle(menuid);
                            if (menu != null)
                            {
                                FollowMenuPackage followCondition = new FollowMenuPackage();
                                followCondition.Id = Guid.NewGuid();
                                followCondition.MenuId = menuid;
                                followCondition.FollowMenuId = followModel.Id;
                                followMenuPackageRepo.Add(followCondition);
                                title = title + "#"+menu.MenuTitle;
                              
                            }
                            else
                            {
                                returnResult = ResultDto.failedResult("menuid:" + menuid + " is not valid");
                            }

                        }
                    if (returnResult.isDone)
                    {
                        followModel.title = title;
                        followMenuRepo.Add(followModel);
                        followMenuRepo.Save();
                        followMenuPackageRepo.Save();
                        returnResult.isDone = true;
                        returnResult.message = "new followMenu created successfully";
                        returnResult.returnId = followModel.Id.ToString();
                    }
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