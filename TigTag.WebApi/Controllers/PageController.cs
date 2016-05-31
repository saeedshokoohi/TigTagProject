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
    public class PageController : BaseController<Page,PageDto>
    {
        UserRepository userRepo = new UserRepository();
        PageRepository pageRepo = new PageRepository();
        PageMenuRepository pageMenuRepo = new PageMenuRepository();
        MenuRepository menuRepo = new MenuRepository();
        public override IGenericRepository<Page> getRepository()
        {
            return pageRepo;
        }
        /// <summary>
        /// create a master profile
        /// if another page with IsMasterPage not available
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public ResultDto CreateMasterProfile([FromBody]PageDto page)
        {
            if (page == null) return ResultDto.failedResult("Invalid Raw Payload data, it must be an json object like : {UserId:'',PageTitle:'testPage',Description:'page description ' , URL:'testUrl'} ");
            else
            {
                page.PageType = false;
                page.IsMasterPage = true;
                var masterPage=pageRepo.getMasterPage();
                if (masterPage == null)
                    return CreatePage(page);
                else
                    return ResultDto.failedResult("there is one Master Profile, only one masterProfile is valid!!!");
            }
        }
        /// <summary>
        /// return the page where IsMasterPage attribute is true
        /// </summary>
        /// <returns></returns>
        public PageDto GetMasterProfile()
        {
            var masterPage = pageRepo.getMasterPage();
            return Mapper<Page, PageDto>.convertToDto(masterPage); 
        }
        /// <summary>
        /// return the page which is related to given userid
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public PageDto GetPageByUserId(Guid userid)
        {
            Page page = pageRepo.getPageByUser(userid);
            return Mapper<Page, PageDto>.convertToDto(page);
        }
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
        /// <summary>
        /// return the pages and number of new posts for given user
        /// based on his following conditions
        /// </summary>
        /// <param name="followerPageId"></param>
        /// <returns></returns>
        public List<PageDto> getNewPostCountByFollowerPageId(Guid followerPageId)
        {
            return pageRepo.getNewPostCountByFollowerPageId(followerPageId);
        }
        
        public List<FollowMenuDto> getNewPostCountOnFollowingMenuByFollowerPageId(Guid followerPageId)
        {
            return pageRepo.getNewPostCountOnFollowingMenuByFollowerPageId(followerPageId);
        }

    }
}