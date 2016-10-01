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
using TigTag.DTO.ModelDTO.RequestDto;
using TigTag.Common.Enumeration;
using System.Web.OData;
using TigTag.Common.util;

namespace TigTag.WebApi.Controllers
{
    public class PageController : BaseController<Page, PageDto>
    {
        UserRepository userRepo = new UserRepository();
        PageRepository pageRepo = new PageRepository();
        PageMenuRepository pageMenuRepo = new PageMenuRepository();
        MenuRepository menuRepo = new MenuRepository();
        PageSettingRepository PageSettingRepo = new PageSettingRepository();
        EventsLogRepository eventRepo = new EventsLogRepository();
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
            if (page == null) return ResultDto.failedResult("Invalid Raw Payload data, it must be a json object like : {UserId:'',PageTitle:'testPage',Description:'page description ' , URL:'testUrl'} ");
            else
            {
                page.PageType = enmPageTypes.PROFILE.GetHashCode();
                page.IsMasterPage = true;
                var masterPage = pageRepo.getMasterPage();
                if (masterPage == null)
                    return CreateGeneralPage(page);
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
        [EnableQueryAttribute]
        public IQueryable<PageDto> queryByMenuList(String menuList)
        {
            Guid[] menuidlist = JsonUtil.convertToGuidArray(menuList);
            var retValues = pageRepo.queryByMenuList(menuidlist);
            addFollowingInfo(retValues);
            return retValues;
        }

        private void addFollowingInfo(IQueryable<PageDto> pages)
        {
            foreach (var p in pages)
            {
                Guid currentUser = getCurrentProfileId();
                 p.IsFollowingByCurrentUser=pageRepo.isFollowingByCurentUser(currentUser, p);
                if(p.IsFollowingByCurrentUser)
                p.newPostCount = pageRepo.getNewPostCountByFollowerPageIdAndFollowingPageId(currentUser,p.Id);

            }
        }
        private void addFollowingInfo(PageDto p)
        {
           
                Guid currentUser = getCurrentProfileId();
                p.IsFollowingByCurrentUser = pageRepo.isFollowingByCurentUser(currentUser, p);
                if (p.IsFollowingByCurrentUser)
                    p.newPostCount = pageRepo.getNewPostCountByFollowerPageIdAndFollowingPageId(currentUser, p.Id);

            
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
                page.PageType = enmPageTypes.PROFILE.GetHashCode();
                return CreateGeneralPage(page);
            }
        }
        /// <summary>
        /// create profile for user
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public ResultDto CreatePage([FromBody]PageDto page)
        {
            if (page == null) return ResultDto.failedResult("Invalid Raw Payload data, it must be an json object like : {UserId:'',PageTitle:'testPage',Description:'page description ' , URL:'testUrl'} ");
            else
            {
                page.PageType = enmPageTypes.PAGE.GetHashCode();
                ResultDto result= CreateGeneralPage(page);
             
                return result;
            }
        }
        /// <summary>
        /// create profile for user
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public ResultDto CreateTeam([FromBody]PageDto page)
        {
            if (page == null) return ResultDto.failedResult("Invalid Raw Payload data, it must be an json object like : {UserId:'',PageTitle:'testPage',Description:'page description ' , URL:'testUrl'} ");
            else
            {
                page.PageType = enmPageTypes.TEAM.GetHashCode();
                return CreateGeneralPage(page);
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
                page.PageType = enmPageTypes.POST.GetHashCode();
                if (String.IsNullOrEmpty(page.URL)) page.URL = pageRepo.generatePostId(page);
                return CreateGeneralPage(page);
            }
        }
        /// <summary>
        /// this service create a new page for user
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        private ResultDto CreateGeneralPage(PageDto page)
        {

            if (page == null) return ResultDto.failedResult("Invalid Raw Payload data, it must be an json object like : {UserId:'',PageTitle:'testPage',Description:'page description ' , URL:'testUrl'} ");
            ResultDto returnResult = new ResultDto();
            Page pageModel = Mapper<Page, PageDto>.convertToModel(page);
            pageModel.Id = Guid.NewGuid();
            pageModel.CreateDate = DateTime.Now;
            enmPageTypes pageType = (enmPageTypes)page.PageType;
            switch (pageType)
            {
                case enmPageTypes.PROFILE:
              
                    returnResult = pageRepo.validatePage(pageModel);
                    break;
                case enmPageTypes.PAGE:
                case enmPageTypes.TEAM:
                case enmPageTypes.POST:
                    returnResult = pageRepo.validatePost(pageModel);
                    break;
                default:
                    break;
            }



            if (returnResult.isDone)
            {
                try
                {
                    pageRepo.setPageColor(pageModel);
                    pageRepo.Add(pageModel);
                    pageRepo.Save();
                    //adding menu list to page
                    
                   
                        if (page.Menulist != null)
                            foreach (var menuid in page.Menulist)
                            {
                            try
                            {
                                //var menu= menuRepo.GetSingle(menuid);
                                PageMenu pageMenu = new PageMenu();
                                ResultDto tempResult = new ResultDto();

                                pageMenu.Id = Guid.NewGuid();
                                pageMenu.MenuId = menuid;
                                pageMenu.PageId = pageModel.Id;
                                menuRepo.checkUnquness(pageMenu, tempResult);
                                if (tempResult.validationMessages != null && tempResult.validationMessages.Count() == 0)
                                {
                                    pageMenuRepo.Add(pageMenu);
                                    menuRepo.increaseScore(pageMenu.MenuId);
                                    pageMenuRepo.Save();
                                }
                            }
                            catch { }

                            }
                 
                    try
                    {
                        if (page.newMenuTitleList != null)
                        {
                            foreach (var menuTitle in page.newMenuTitleList)
                            {
                                Guid menuid = menuRepo.addOrGetMenuByTitle(menuTitle, pageModel.Id);
                                PageMenu pageMenu = new PageMenu();
                                ResultDto tempResult = new ResultDto();

                                pageMenu.Id = Guid.NewGuid();
                                pageMenu.MenuId = menuid;
                                pageMenu.PageId = pageModel.Id;
                                menuRepo.checkUnquness(pageMenu, tempResult);
                                if (tempResult.validationMessages != null && tempResult.validationMessages.Count() == 0)
                                {
                                    pageMenuRepo.Add(pageMenu);
                                    menuRepo.increaseScore(pageMenu.MenuId);
                                    pageMenuRepo.Save();
                                }

                            }
                        }
                    }
                    catch { }
                    
                    //adding page setting
                    if (pageModel.PageType != enmPageTypes.POST.GetHashCode())
                    {
                        PageSetting pageSetting = new PageSetting();
                        pageSetting.Id = Guid.NewGuid();
                        if (page.IsPublic == null) page.IsPublic = true;
                        pageSetting.IsPublic = page.IsPublic;
                        pageSetting.PageId = pageModel.Id;
                        PageSettingRepo.Add(pageSetting);
                        PageSettingRepo.Save();
                    }
                    if (page.ProfileId == Guid.Empty) page.ProfileId = getCurrentProfileId();
                    switch (pageType)
                    {
                        case enmPageTypes.PROFILE:
                            eventRepo.AddProfileEvent(page.ProfileId, pageModel);
                            break;
                        case enmPageTypes.PAGE:
                            eventRepo.AddPageEvent(page.ProfileId,pageModel);
                            break;
                        case enmPageTypes.TEAM:
                            eventRepo.AddTeamEvent(page.ProfileId, pageModel);
                            break;
                        case enmPageTypes.POST:
                            eventRepo.AddPostEvent(page.ProfileId, pageModel);
                            break;
                        default:
                            break;
                    }


                    returnResult.isDone = true;
                    returnResult.message = "new page/post created successfully";
                    returnResult.returnId = pageModel.Id.ToString();
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
        /// <summary>
        /// return the pages which user is following and also filter them by given menuidList 
        /// if menuidList is null or empty no filtering will be applied
        /// </summary>
        /// <param name="followerPageId"></param>
        /// <param name="menuList"></param>
        /// <returns></returns>
        [HttpGet]
        [EnableQueryAttribute]
        public IQueryable<PageDto> queryNewPostCountByFollowerPageIdAndmenuList(String followerPageId, String menuList)
        {
            Guid[] menuidlist = JsonUtil.convertToGuidArray(menuList);
            Guid followerid = Guid.NewGuid();
            try
            {
                 followerid = Guid.Parse(followerPageId);
            }
            catch { return null; }
            return pageRepo.getNewPostCountByFollowerPageId(followerid, menuidlist);
        }
        /// <summary>
        /// return the followingPackageMenu List and the count of new posts for current user
        /// </summary>
        /// <param name="followerPageId"></param>
        /// <returns></returns>
        public List<FollowMenuDto> getNewPostCountOnFollowingMenuByFollowerPageId(Guid followerPageId)
        {
            return pageRepo.getNewPostCountOnFollowingMenuByFollowerPageId(followerPageId);
        }
        public PageDto getPageStats(string pageId)
        {
            try
            {
                Guid Id = Guid.Parse(pageId);
                PageDto retPage= getById(Id);
                retPage.PageStats= pageRepo.getPageStats(retPage);
                addFollowingInfo(retPage);
                addMenuInfo(retPage);
                addPageDetail(retPage);
                addContactInfoDetail(retPage);
                addTicketList(retPage);
                return retPage;
                  }
            catch(Exception ex) {
                return null;

            }
        }

        private void addTicketList(PageDto retPage)
        {
            PageRepository pageRepo = new PageRepository();
            retPage.TicketList = pageRepo.getTicketList(retPage.Id);
        }

        private void addContactInfoDetail(PageDto retPage)
        {
            PageRepository pageRepo = new PageRepository();
            retPage.ContactInfoList=pageRepo.getContactInfoList(retPage.Id);
        }

        private void addPageDetail(PageDto retPage)
        {
            PageRepository pageRepo = new PageRepository();
            
            retPage.ParentPageDto = pageRepo.getParentPage(retPage.Id);
            retPage.CreatedByUserDto = pageRepo.getPageCReatorUser(retPage.Id);
        }

        private void addMenuInfo(PageDto retPage)
        {
            retPage.Menulist = pageRepo.getMenuList(retPage.Id);
            retPage.PublicMenuDtoList = pageRepo.getPublicMenuDtoList(retPage.Id);
            retPage.CustomMenuDtoList = pageRepo.getCustomMenuDtoList(retPage.Id);
        }

        public List<PageDto> findPostsByFollowerAndFollowingPageId([FromBody]BaseRequestDto request)
        {

            Guid followingPageId = BaseRequestDto.extractParameterValueAsGuid(request, "followingPageId");
            Guid followerPageId = BaseRequestDto.extractParameterValueAsGuid(request, "followerPageId");
            pageRepo.checkFollowingPageAsVisited(followerPageId, followingPageId);
            return pageRepo.getNewPostByFollowerAndFollowingPageId(followerPageId, followingPageId, request);
        }
        public List<PageDto> findPostsByFollowerAndFollowingMenuId([FromBody]BaseRequestDto request)
        {

            Guid followingPageId = BaseRequestDto.extractParameterValueAsGuid(request, "followingMenuId");
            Guid followerPageId = BaseRequestDto.extractParameterValueAsGuid(request, "followerPageId");
            pageRepo.checkFollowingMenuAsVisited(followerPageId, followingPageId);
            return pageRepo.getNewPostByFollowerAndFollowingMenuId(followerPageId, followingPageId, request);
        }
        public ResultDto checkFollowingPageAsVisited([FromBody]BaseRequestDto request)
        {

            Guid followingPageId = BaseRequestDto.extractParameterValueAsGuid(request, "followingPageId");
            Guid followerPageId = BaseRequestDto.extractParameterValueAsGuid(request, "followerPageId");
            return pageRepo.checkFollowingPageAsVisited(followerPageId, followingPageId);

        }

        public ResultDto checkFollowingMenuAsVisited([FromBody]BaseRequestDto request)
        {
            Guid followingMenuId = BaseRequestDto.extractParameterValueAsGuid(request, "followingMenuId");
            Guid followerPageId = BaseRequestDto.extractParameterValueAsGuid(request, "followerPageId");
            return pageRepo.checkFollowingMenuAsVisited(followerPageId, followingMenuId);

        }

        public PageDto getParentPage(Guid pageid)
        {
            return pageRepo.getParentPage(pageid);
        }

        public List<PageDto> getPostByPageId(Guid pageId)
        {
            return pageRepo.getPostByPageId(pageId, 100);
        }
        public List<PageDto> getPageByProfileId(Guid pageId)
        {
            return pageRepo.getPageByProfileId(pageId, 100);
        }
        public List<PageDto> getTeamByPageId(Guid pageId)
        {
            return pageRepo.getTeamByPageId(pageId, 100);
        }
        public List<PageDto> getParticipantsByPageId(Guid pageId)
        {
            return pageRepo.getParticipantsByPageId(pageId, 100);
        }
        [HttpGet]
        [EnableQueryAttribute]
        public IQueryable<PageDto> queryParticipantsByPageIdAndmenuList(String pageId, String menuList)
        {
            Guid[] menuidlist = JsonUtil.convertToGuidArray(menuList);
            Guid pageuId = Guid.NewGuid();
            try
            {
                pageuId = Guid.Parse(pageId);
            }
            catch { return null; }
            return pageRepo.getParticipantsByPageId(pageuId, menuidlist);
        }
        public PageSettingDto getPageSetting(Guid pageId)
        {
            return pageRepo.getPageSetting(pageId);
        }
        public List<PageDto> filterPostsByMenuList(PageDto filter)
        {
           return pageRepo.filterPostsByMenuList(filter);
          
        }
    }
}