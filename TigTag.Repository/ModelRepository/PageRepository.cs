using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TigTag.DataModel.model;
using TigTag.DTO.ModelDTO;
using TigTag.DTO.ModelDTO.Base;
using TigTag.Repository.IModelRepository;
using TiTag.Repository.Base;
using TigTag.DTO.ModelDTO.RequestDto;
using TigTag.Common.Enumeration;

namespace TigTag.Repository.ModelRepository {

 
    public class PageRepository : 
        GenericRepository<DataModelContext, Page>, IPageRepository  {
        private readonly string URL_ALREADY_EXISTS = "PAGE_URL_ALREADY_EXISTS";
        private readonly string URL_IS_EMPTY = "URL_IS_EMPTY";
        private readonly string TITLE_IS_EMPTY = "TITLE_IS_EMPTY";
        public Page GetSingle(Guid Id) {

            var query = Context.Pages.FirstOrDefault(x => x.Id ==Id );
            return query;
        }

        public ResultDto validatePage(Page pageModel)
        {
            ResultDto retResult = new ResultDto();
            retResult.isDone = true;
            checkUrl(pageModel, retResult);
            checkTitle(pageModel, retResult);
            checkUserId(pageModel, retResult);
            checkImageId(pageModel, retResult);
            if (retResult.isDone)
                retResult.statusCode = enm_STATUS_CODE.DONE_SUCCESSFULLY;
            return retResult;
        }

        public Page getMasterPage()
        {
            var masterpages= Context.Pages.Where(p => p.IsMasterPage == true).ToList();
            if (masterpages.Count > 0)
                return masterpages[0];
            else
                return null;
        }

        public ResultDto validatePost(Page pageModel)
        {
            ResultDto retResult = new ResultDto();
            retResult.isDone = true;
            checkUrl(pageModel, retResult);
            checkTitle(pageModel, retResult);
            //checkUserId(pageModel, retResult);
            checkPageId(pageModel, retResult);
            checkImageId(pageModel, retResult);
            if (retResult.isDone)
                retResult.statusCode = enm_STATUS_CODE.DONE_SUCCESSFULLY;
            return retResult;
        }

        public Page findByUserName(string name)
        {
           List<Page> pages= Context.Pages.Where(p => p.User.UserName == name && p.PageType==profileTypeCode).ToList();
            if (pages.Count > 0)
                return pages[0];
            else
                return null;
            
        }

        public string generatePostId(PageDto page)
        {
            return "post" + Guid.NewGuid().ToString().Substring(0, 15);
        }

        private void checkPageId(Page page, ResultDto retResult)
        {
            if (page == null || page.PageId == null)
            {
                retResult.isDone = false;
                retResult.statusCode = enm_STATUS_CODE.INPUT_NOT_VALID;
                retResult.addValidationMessages("Page_Id is null or empty while it is required for creating post");
            }
            else
            {
                List<Page> c = Context.Pages.Where(u => u.Id == page.PageId).ToList();
                if (c.Count == 0)
                {
                    retResult.isDone = false;
                    retResult.statusCode = enm_STATUS_CODE.INPUT_NOT_VALID;
                    retResult.addValidationMessages("Page_ID_IS_NOT_VALID");
                }
                else
                {
                    page.UserId = c[0].UserId;
                }
            }
        }

        public bool isFollowingByCurentUser(Guid currentProfileId, PageDto item)
        {
            var q= Context.Follows.Where(f => f.FollowerUserId == currentProfileId && f.FollowingPageId == item.Id);
            return (q.Count() > 0);
        }
        public int getNewPostCountByFollowerPageIdAndFollowingPageId(Guid followerPageId,Guid followingPageId)
        {
            int newPostCount = 0;
           
            List<Guid> tempListOfposts = new List<Guid>();
            
            //for every follow of this page
            var follows = Context.Follows.Where(f => f.FollowerUserId == followerPageId && f.FollowingPageId == followingPageId);
            foreach (var f in follows)
            {
                var posts = Context.Pages.Where(p => p.PageId == f.FollowingPageId && (p.CreateDate > f.lastVisitDate || f.lastVisitDate == null)).Select(p => p);
                foreach (var post in posts)
                {
                    var conditions = Context.FollowConditions.Where(fc => fc.FollowId == f.Id);
                    bool isOk = true;
                    foreach (var condition in conditions)
                    {
                        if (post.PageMenus.Any(pfm => pfm.MenuId == condition.MenuId))
                        {

                        }
                        else
                        {
                            isOk = false;
                        }
                    }
                    if (isOk)
                    {
                        if (!tempListOfposts.Contains(post.Id))
                        {
                            tempListOfposts.Add(post.Id);
                            newPostCount++;
                        }
                    }

                }

            }
            return newPostCount;
        }
        public List<PageDto>  getNewPostCountByFollowerPageId(Guid followerPageId)
        {
            List<PageDto> retList = new List<PageDto>();

            //pages which current user is following
         
          var followingpages=   Context.Follows.Where(f => f.FollowerUserId == followerPageId ).Select(f => f.Page).Distinct().ToList();
            //for every page
            foreach (var fp in followingpages)
            {
                PageDto newPageDto = Mapper<Page, PageDto>.convertToDto(fp);
                retList.Add(newPageDto);
                newPageDto.newPostCount = getNewPostCountByFollowerPageIdAndFollowingPageId(followerPageId, fp.Id);
            }

            return retList;

        }

        private bool postContainsMenu(Page post, Menu menu)
        {
            return Context.PageMenus.All(pm => pm.PageId == post.Id && pm.MenuId == menu.Id);
        }

        public Page getPageByUser(Guid userid)
        {
            var pages= Context.Pages.Where(p => p.UserId == userid && p.PageType == profileTypeCode).ToList();
            if (pages.Count > 0)
            {
                return pages[0];
            }
            else return null;
        }

        private void checkUserId(Page page, ResultDto retResult)
        {
            if (page == null || page.UserId==null)
            {
                retResult.isDone = false;
                retResult.statusCode = enm_STATUS_CODE.INPUT_NOT_VALID;
                retResult.addValidationMessages("user id is null or empty while it is required for creating page");
            }
            else
            {
                var c = Context.Users.Count(u =>  u.Id == page.UserId);
                if (c == 0)
                {
                    retResult.isDone = false;
                    retResult.statusCode = enm_STATUS_CODE.INPUT_NOT_VALID;
                    retResult.addValidationMessages("User_ID_NOT_VALID");
                }
            }
        }

        public List<FollowMenuDto> getNewPostCountOnFollowingMenuByFollowerPageId(Guid followerPageId)
        {
            List<FollowMenuDto> retList = new List<FollowMenuDto>();

            //pages which current user is following

            var followMenus = Context.FollowMenus.Where(f => f.FollowerUserId == followerPageId).Distinct().ToList();
            //for every following menu
            foreach (var fp in followMenus)
            {
                FollowMenuDto newPageDto = Mapper<FollowMenu, FollowMenuDto>.convertToDto(fp);
                List<Guid> tempListOfposts = new List<Guid>();
                retList.Add(newPageDto);
                //for every follow of this page
                    
                    var posts = Context.Pages.Where(p =>p.PageType==postTypeCode && (p.CreateDate > fp.lastVisitDate || fp.lastVisitDate == null)).OrderByDescending(c => c.CreateDate).Select(p => p);
                    foreach (var post in posts)
                    {
                        var packages = Context.FollowMenuPackages.Where(fc => fc.FollowMenuId == fp.Id);
                        bool isOk = true;
                        foreach (var condition in packages)
                        {
                            if (post.PageMenus.Any(pfm => pfm.MenuId == condition.MenuId))
                            {

                            }
                            else
                            {
                                isOk = false;
                            }
                        }
                        if (isOk)
                        {
                            if (!tempListOfposts.Contains(post.Id))
                            {
                                tempListOfposts.Add(post.Id);
                                newPageDto.newPostCount++;
                            }
                        }


                }
            }

            return retList;
        }

        public void setPageColor(Page pageModel)
        {
            try
            {
                if ((pageModel.Color == null || pageModel.Color.Trim().Length == 0) && pageModel.PageId != null)
                    pageModel.Color = GetSingle((Guid)pageModel.PageId).Color;
            }
            catch { }

        }

        public ResultDto checkFollowingMenuAsVisited(Guid followerPageId, Guid followingMenuId)
        {
            try
            {
                Context.FollowMenus.Where(f => f.FollowerUserId == followerPageId && f.Id == followingMenuId).ToList().ForEach(f => f.lastVisitDate = DateTime.Now);
                Context.SaveChanges();
                return ResultDto.successResult("", "last visited date set => " + DateTime.Now);
                
            }
            catch (Exception ex)
            {
                return ResultDto.exceptionResult(ex);
            }
        }

        public ResultDto checkFollowingPageAsVisited(Guid followerPageId, Guid followingPageId)
        {
            
            try
            {
                Context.Follows.Where(f => f.FollowerUserId == followerPageId && f.FollowingPageId == followingPageId).ToList().ForEach(f => f.lastVisitDate = DateTime.Now);
                Context.SaveChanges();
                return ResultDto.successResult("", "last visited set for " + DateTime.Now);

            }
            catch(Exception ex)
            {
                return ResultDto.exceptionResult(ex);
            }
        }

        public List<PageDto> getNewPostByFollowerAndFollowingMenuId(Guid followerPageId, Guid followingMenuId, BaseRequestDto request)
        {
            List<PageDto> retList = new List<PageDto>();

            //pages which current user is following

            var followMenus = Context.FollowMenus.Where(f => f.Id == followingMenuId).Distinct().ToList();
            //for every following menu
            foreach (var fp in followMenus)
            {
                List<Guid> tempListOfposts = new List<Guid>();
                //for every follow of this page
                var posts = Context.Pages.Where(p => p.PageType==postTypeCode && (p.CreateDate > fp.lastVisitDate || fp.lastVisitDate == null)).OrderByDescending(c => c.CreateDate).Select(p => p);
                foreach (var post in posts)
                {
                    var packages = Context.FollowMenuPackages.Where(fc => fc.FollowMenuId == fp.Id);
                    bool isOk = true;
                    foreach (var condition in packages)
                    {
                        if (post.PageMenus.Any(pfm => pfm.MenuId == condition.MenuId))
                        {

                        }
                        else
                        {
                            isOk = false;
                        }
                    }
                    if (isOk)
                    {
                        if (!tempListOfposts.Contains(post.Id))
                        {
                            tempListOfposts.Add(post.Id);
                            retList.Add(Mapper<Page,PageDto>.convertToDto(post));
                            if (retList.Count >= request.maxResult)
                                return retList;
                        }
                    }


                }
            }

            return retList;
        }

        public IQueryable<PageDto> getNewPostCountByFollowerPageId(Guid followerPageId, Guid[] menuidlist)
        {
            List<PageDto> retList = new List<PageDto>();

            //pages which current user is following
            List<Page> followingpages = null;
            if (menuidlist!=null)
             followingpages = Context.Follows.Where(f => f.FollowerUserId == followerPageId && menuidlist.All(mi=>f.Page.PageMenus.Any(pm=>mi==pm.MenuId))).Select(f => f.Page).Distinct().ToList();
            else
                 followingpages = Context.Follows.Where(f => f.FollowerUserId == followerPageId ).Select(f => f.Page).Distinct().ToList();
            //for every page
            foreach (var fp in followingpages)
            {
                PageDto newPageDto = Mapper<Page, PageDto>.convertToDto(fp);
                retList.Add(newPageDto);
                newPageDto.newPostCount = getNewPostCountByFollowerPageIdAndFollowingPageId(followerPageId, fp.Id);
            }

            return retList.AsQueryable();
        }

        public List<MenuDto> getPublicMenuDtoList(Guid pageid)
        {
            List<Menu> pmenus=Context.PageMenus.Where(pm => pm.PageId == pageid && pm.Menu.Page.IsMasterPage == true).Select(pm => pm.Menu).ToList();
            return Mapper<Menu, MenuDto>.convertListToDto(pmenus);
        }

        public List<MenuDto> getCustomMenuDtoList(Guid pageid)
        {
            List<Menu> pmenus = Context.PageMenus.Where(pm => pm.PageId == pageid && pm.Menu.Page.IsMasterPage != true).Select(pm => pm.Menu).ToList();
            return Mapper<Menu, MenuDto>.convertListToDto(pmenus);
        }

        public PageStatsDto getPageStats(PageDto retPage)
        {
            PageStatsDto pageStats = new PageStatsDto();
            pageStats.postCount = getTotalPostCountOfPage(retPage.Id);
            pageStats.pagesCount = getTotalPagesCountOfPage(retPage.Id);
            pageStats.followersCount = getTotalFollowersCountOfPage(retPage.Id);
            pageStats.followingsCount = getTotalFollowingsCountOfPage(retPage.Id);
            pageStats.participantsCount = getTotalParticipantsCountOfPage(retPage.Id);


            return pageStats;

        }


        public UserDto getPageCReatorUser(Guid id)
        {
           var users= Context.Pages.Where(p => p.Id == id).Select(p => p.User).ToList();
            if (users.Count() > 0)
                return Mapper<User, UserDto>.convertToDto(users[0]);
            else
                return null;
        }

        public List<TicketDto> getTicketList(Guid id)
        {
            List<TicketDto> retList = null;
            var ciList = Context.Tickets.Where(ci => ci.PageId == id).ToList();
            return Mapper<Ticket, TicketDto>.convertListToDto(ciList);
        }

        public List<ContactInfoDto> getContactInfoList(Guid id)
        {
            List<ContactInfoDto> retList = null;
            var ciList= Context.ContactInfos.Where(ci => ci.PageId == id).ToList();
            return Mapper<ContactInfo, ContactInfoDto>.convertListToDto(ciList);
        }

        public Guid[] getMenuList(Guid id)
        {
            return Context.PageMenus.Where(pm => pm.PageId == id).Select(pm => pm.MenuId).ToArray();

        }

        private int getTotalParticipantsCountOfPage(Guid id)
        {
            return Context.Participants.Count(p => p.PageId == id );
        }

        private int getTotalFollowingsCountOfPage(Guid id)
        {
            return Context.Follows.Count(f => f.FollowerUserId==id && f.RequestStatus == 1);
        }

        private int getTotalFollowersCountOfPage(Guid id)
        {
            return Context.Follows.Count(f => f.FollowingPageId == id && f.RequestStatus == 1);
        }

        private int getTotalPagesCountOfPage(Guid id)
        {
            return Context.Pages.Count(p => p.PageId == id && p.PageType == pageTypeCode);
        }

        private int getTotalPostCountOfPage(Guid id)
        {
            return Context.Pages.Count(p => p.PageId == id && p.PageType == postTypeCode);
        }

        public PageSettingDto getPageSetting(Guid pageId)
        {
          var ps=Context.PageSettings.Where(p => p.PageId == pageId).ToList();
            if (ps != null && ps.Count > 0)
                return Mapper<PageSetting, PageSettingDto>.convertToDto(ps[0]);
            else
                return new PageSettingDto();
        }

      
        public List<PageDto> getTeamByPageId(Guid pageId, int MaxRetun)
        {
            var pl = Context.Pages.Where(p => p.PageId == pageId && p.PageType ==teamTypeCode).
                OrderByDescending(p => p.CreateDate).Take(MaxRetun).ToList();
          return  ConvertPageListToDtoList( pl);
        }

        public PageDto getParentPage(Guid pageid)
        {
            var parentPageid = Context.Pages.Where(p => p.Id == pageid).Select(p => p.PageId).ToList();
            if (parentPageid != null && parentPageid.Count > 0 && parentPageid[0]!=null)
            {
             
                return Mapper<Page, PageDto>.convertToDto(GetSingle((Guid)parentPageid[0]));
            }
            else return null;
        }

        public List<PageDto> getPageByProfileId(Guid pageId, int MaxRetun)
        {
            var pl = Context.Pages.Where(p => p.PageId == pageId && p.PageType == pageTypeCode).
                OrderByDescending(p => p.CreateDate).Take(MaxRetun).ToList();
            return ConvertPageListToDtoList(pl);
        }
        public List<PageDto> getPostByPageId(Guid pageId, int MaxRetun)
        {
            var pl = Context.Pages.Where(p => p.PageId == pageId && p.PageType == postTypeCode).
                OrderByDescending(p => p.CreateDate).Take(MaxRetun).ToList();
            return ConvertPageListToDtoList(pl);
        }
        public List<PageDto> getParticipantsByPageId(Guid pageId, int MaxRetun)
        {
            var pl = Context.Participants.Where(p => p.PageId == pageId).Select(pr=>pr.Page1).
                OrderByDescending(p => p.CreateDate).Take(MaxRetun).ToList();
            return ConvertPageListToDtoList(pl);
        }
        public List<PageDto> filterPostsByMenuList(PageDto filter)
        {
            if (filter != null && filter.Menulist != null)
            {
                List<Page> pagelist = new List<Page>();
              
                if (filter.PageId!=null)
                {
                    var pl = Context.PageMenus.GroupBy(pm => pm.PageId).Where(gp => gp.Any(p2 => p2.PageId == filter.PageId) && gp.All(p => filter.Menulist.Contains(p.MenuId))).ToList();
                   
                    foreach (var item in pl)
                    {
                        var page= GetSingle(item.Key);
                        pagelist.Add(page);
                    }
                  
                }
                else
                {
                    var pl = Context.PageMenus.GroupBy(pm => pm.PageId).Where(gp => gp.All(p => filter.Menulist.Contains(p.MenuId))).ToList();

                    foreach (var item in pl)
                    {
                        var page = GetSingle(item.Key);
                        pagelist.Add(page);
                    }
                }
                return ConvertPageListToDtoList(pagelist);
            }
                return null;
        }

        public IQueryable<PageDto> getParticipantsByPageId(Guid pageId, Guid[] menuidlist)
        {
            var pl = Context.Participants.Where(p => p.PageId == pageId && menuidlist.All(mi => p.Page1.PageMenus.Any(pm => mi == pm.MenuId))).Select(pr => pr.Page1).Distinct().AsQueryable();
            return  Mapper<Page,PageDto>.convertIquerybleToDto(pl);
        }

        private List<PageDto> ConvertPageListToDtoList(List<Page> pl)
        {
            List<PageDto> retList = new List<PageDto>();
            foreach (var item in pl)
            {
                PageDto pdto = Mapper<Page, PageDto>.convertToDto(item);
                FillPageSetting(pdto);
                retList.Add(pdto);

            }
            return retList;
        }

        private void FillPageSetting(PageDto pdto)
        {
            var ps= getPageSetting(pdto.Id);
            pdto.IsPublic = true;
            if (ps != null && ps.IsPublic != null)
                pdto.IsPublic = ps.IsPublic;


        }
        /// <summary>
        /// query on pages while pages can be filtered by menu list
        /// </summary>
        /// <param name="menuList"></param>
        /// <returns></returns>
         
         public  IQueryable<PageDto> queryByMenuList(Guid[] menuList)
        {
            if(menuList!=null && menuList.Length>0)
            return  Mapper<Page,PageDto>.convertIquerybleToDto(  Context.Pages.Where(p=>menuList.All(mi=>p.PageMenus.Any(fm=>fm.MenuId==mi))).AsQueryable());
            else
                return Mapper<Page, PageDto>.convertIquerybleToDto(Context.Pages.AsQueryable());

        }

        private bool ContainsAllMenuList(Guid[] menuList, ICollection<PageMenu> pageMenus)
        {
            bool itContains = false;
            foreach (var m in menuList)
            {
                foreach (var pm in pageMenus)
                {
                    if(pm.MenuId.Equals(m))
                    {
                        itContains = true;
                        break;
                    }
                }
                if (!itContains) return itContains;
                else itContains = false;
            }
            return true;
        }

        public List<PageDto> getNewPostByFollowerAndFollowingPageId(Guid followerPageId, Guid followingPageId, BaseRequestDto request)
        {
            List<PageDto> retList = new List<PageDto>();

            //pages which current user is following

            var followingpages = Context.Pages.Where(f => f.Id == followingPageId).ToList();
            if(followingpages.Count>0)
            {
                Page fp = followingpages[0];
               
                List<Guid> tempListOfposts = new List<Guid>();
               
                //for every follow of this page
                var follows = Context.Follows.Where(f => f.FollowerUserId == followerPageId && f.FollowingPageId == fp.Id);
                foreach (var f in follows)
                {
                    var posts = Context.Pages.Where(p => p.PageId == f.FollowingPageId && (p.CreateDate > f.lastVisitDate || f.lastVisitDate == null)).OrderByDescending(c => c.CreateDate).Select(p => p);
                    foreach (var post in posts)
                    {
                        var conditions = Context.FollowConditions.Where(fc => fc.FollowId == f.Id);
                        bool isOk = true;
                        foreach (var condition in conditions)
                        {
                            if (post.PageMenus.Any(pfm => pfm.MenuId == condition.MenuId))
                            {

                            }
                            else
                            {
                                isOk = false;
                            }
                        }
                        if (isOk)
                        {
                            if (!tempListOfposts.Contains(post.Id))
                            {
                                tempListOfposts.Add(post.Id);
                                retList.Add(Mapper<Page, PageDto>.convertToDto(post));
                                if (retList.Count >= request.maxResult)
                                    return retList;
                            }
                        }

                    }

                }
            }

            return retList;

        }

        private void checkImageId(Page page, ResultDto retResult)
        {
            if (page == null || page.ImageId == null)
            {
               
            }
            else
            {
                var c = Context.ImageTables.Count(u => u.Id == page.ImageId);
                if (c == 0)
                {
                    retResult.isDone = false;
                    retResult.statusCode = enm_STATUS_CODE.INPUT_NOT_VALID;
                    retResult.addValidationMessages("Image_ID_IS_NOT_VALID");
                }
            }
        }
        private void checkUrl(Page page, ResultDto retResult)
        {
            if (page == null || String.IsNullOrEmpty(page.URL))
            {
                retResult.isDone = false;
                retResult.statusCode = enm_STATUS_CODE.INPUT_NOT_VALID;
                retResult.addValidationMessages(URL_IS_EMPTY);
            }
            else
            {
                var c = Context.Pages.Count(u =>u!=null && u.URL == page.URL);
                if (c > 0)
                {
                    retResult.isDone = false;
                    retResult.statusCode = enm_STATUS_CODE.INPUT_NOT_VALID;
                    retResult.addValidationMessages(URL_ALREADY_EXISTS);
                }
            }
        }
        private void checkTitle(Page page, ResultDto retResult)
        {
            if (page == null || String.IsNullOrEmpty(page.PageTitle))
            {
                retResult.isDone = false;
                retResult.statusCode = enm_STATUS_CODE.INPUT_NOT_VALID;
                retResult.addValidationMessages(TITLE_IS_EMPTY);
            }
           
        }
    }
}