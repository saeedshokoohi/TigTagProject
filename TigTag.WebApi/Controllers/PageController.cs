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

namespace TigTag.WebApi.Controllers
{
    public class PageController : BaseController<User>
    {
        UserRepository userRepo = new UserRepository();
        PageRepository pageRepo = new PageRepository();
        PageMenuRepository pageMenuRepo = new PageMenuRepository();
        MenuRepository menuRepo = new MenuRepository();
        /// <summary>
        /// create profile for user
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public ResultDto CreateProfile([FromBody]PageDto page)
        {
            if (page == null) return ResultDto.failedResult("Invalid Raw Payload data, it must be an json object like : {UserId:'',PageTitle:'testPage',Description:'page description ' , URL:'testUrl'} ");
            else
            {
                page.PageType = false;
                return CreatePage(page);
            }
        }
        /// <summary>
        /// create a post for given page
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public ResultDto CreatePost([FromBody]PageDto page)
        {
            if (page == null) return ResultDto.failedResult("Invalid Raw Payload data, it must be an json object like : {UserId:'',PageTitle:'testPage',Description:'page description ' , URL:'testUrl'} ");
            else
            {
                page.PageType = true;
                if (String.IsNullOrEmpty(page.URL)) page.URL =pageRepo.generatePostId(page);
                return CreatePage(page);
            }
        }
        /// <summary>
        /// this service create a new page for user
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        private ResultDto CreatePage(PageDto page)
        {
           
            if (page == null)return ResultDto.failedResult("Invalid Raw Payload data, it must be an json object like : {UserId:'',PageTitle:'testPage',Description:'page description ' , URL:'testUrl'} ");
            ResultDto returnResult = new ResultDto();
            Page pageModel= Mapper<Page, PageDto>.convertToModel(page);
            pageModel.Id = Guid.NewGuid();
            pageModel.CreateDate = DateTime.Now;
           if(page.PageType)
                returnResult = pageRepo.validatePost(pageModel);
           else
            returnResult = pageRepo.validatePage(pageModel);
            if (returnResult.isDone)
            {
                try
                {
                    pageRepo.Add(pageModel);
                    pageRepo.Save();
                    //adding menu list to page
                    if (page.Menulist!=null)
                    foreach (var menuid in page.Menulist)
                    {
                        //var menu= menuRepo.GetSingle(menuid);
                        PageMenu pageMenu = new PageMenu();
                        pageMenu.Id = Guid.NewGuid();
                        pageMenu.MenuId = menuid;
                        pageMenu.PageId = pageModel.Id;
                        pageMenuRepo.Add(pageMenu);
                        pageMenuRepo.Save();

                    }
                  
                    returnResult.isDone = true;
                    returnResult.message = "new page/post created successfully";
                    returnResult.returnId = pageModel.Id.ToString();
                }
                catch (Exception ex)
                {
                    returnResult= ResultDto.exceptionResult(ex);
                   

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