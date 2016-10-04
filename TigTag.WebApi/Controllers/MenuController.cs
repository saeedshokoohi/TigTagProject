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
using TigTag.Common.Enumeration;
using TigTag.Common.util;
using System.Web.OData;

namespace TigTag.WebApi.Controllers
{
    public class MenuController : BaseController<Menu,MenuDto>
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
                        menuRepo.increaseScore(pagemenumodel.MenuId);
                        pageMenuRepo.Save();
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

      

        public List<MenuDto> getMenuByPageId(Guid pageid)
        {
            List<Menu> menus= menuRepo.findByPageId(pageid);
           List<MenuDto> retList=  Mapper<Menu, MenuDto>.convertListToDto(menus);
            return retList;
        }
        [HttpGet]
        [EnableQueryAttribute]
        public IQueryable<MenuDto> queryUsedMenuListByPageId(string searchType, string pageid, string selectedMenuIds)
        {
            Guid[] menuidlist = JsonUtil.convertToGuidArray(selectedMenuIds);
            Guid pageuId = Guid.Empty;
            int searchTypeInt = 0;
            try
            {
                pageuId = Guid.Parse(pageid);
               
            }
            catch {
                throwException("pageid is not valid!!");
            }
            try
            {
                searchTypeInt = Int32.Parse(searchType);
           
            }
            catch {
                throwException("searchType is not valid!!");
            }
            return menuRepo.queryUsedMenuListByPageId(searchTypeInt, pageuId, menuidlist).AsQueryable();
        }

      
        public override IGenericRepository<Menu> getRepository()
        {
            return menuRepo;
        }
    }
}