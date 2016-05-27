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
    public class MenuController : BaseController<Menu>
    {
        UserRepository userRepo = new UserRepository();
        PageRepository pageRepo = new PageRepository();
        PageMenuRepository pageMenuRepo = new PageMenuRepository();
        MenuRepository menuRepo = new MenuRepository();
      /// <summary>
      /// simply create menu 
      /// </summary>
      /// <param name="menu"></param>
      /// <returns></returns>
        public ResultDto CreateMenu([FromBody]MenuDto menu)
        {
            if (menu == null) return ResultDto.failedResult("Invalid Raw Payload data, it must be an json object  ");
            else
            {
                ResultDto returnResult = new ResultDto();
                Menu menumodel = Mapper<Menu, MenuDto>.convertToModel(menu);
                menumodel.Id = Guid.NewGuid();
                menumodel.CreateDate = DateTime.Now;
                returnResult = menuRepo.validateMenu(menumodel);
                if (returnResult.isDone)
                {
                    try
                    {
                        menuRepo.Add(menumodel);
                        menuRepo.Save();
                        returnResult.isDone = true;
                        returnResult.message = "new menu created successfully";
                        returnResult.returnId = menumodel.Id.ToString();
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
        /// this service add menu to page
        /// </summary>
        /// <param name="pageMenu"></param>
        /// <returns></returns>
        public ResultDto AddPageMenu([FromBody]PageMenuDto pageMenu)
        {
            if (pageMenu == null) return ResultDto.failedResult("Invalid Raw Payload data, it must be an json object  ");
            else
            {
                ResultDto returnResult = new ResultDto();
                PageMenu pagemenumodel = Mapper<PageMenu, PageMenuDto>.convertToModel(pageMenu);
                pagemenumodel.Id = Guid.NewGuid();
               
                returnResult = menuRepo.validatePageMenu(pagemenumodel);
                if (returnResult.isDone)
                {
                    try
                    {
                        pageMenuRepo.Add(pagemenumodel);
                        menuRepo.Save();
                        returnResult.isDone = true;
                        returnResult.message = "new menu added to page successfully";
                        returnResult.returnId = pagemenumodel.Id.ToString();
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
            pageModel.PageType = false;
            returnResult = pageRepo.validatePage(pageModel);
            if (returnResult.isDone)
            {
                try
                {
                    pageRepo.Add(pageModel);
                    //adding menu list to page
                    if(page.Menulist!=null)
                    foreach (var menuid in page.Menulist)
                    {
                        //var menu= menuRepo.GetSingle(menuid);
                        PageMenu pageMenu = new PageMenu();
                        pageMenu.Id = Guid.NewGuid();
                        pageMenu.MenuId = menuid;
                        pageMenu.PageId = pageModel.Id;
                        pageMenuRepo.Add(pageMenu);

                    }
                    pageRepo.Save();
                    returnResult.isDone = true;
                    returnResult.message = "new page created successfully";
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