using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TigTag.DataModel.model;
using TigTag.DTO.ModelDTO;
using TigTag.DTO.ModelDTO.Base;
using TigTag.Repository.IModelRepository;
using TiTag.Repository.Base;


namespace TigTag.Repository.ModelRepository {

    public class MenuRepository : 
        GenericRepository<DataModelContext, Menu>, IMenuRepository  {

        public Menu GetSingle(Guid Id) {

            var query = Context.Menus.FirstOrDefault(x => x.Id ==Id );
            return query;
        }

        public ResultDto validateMenu(Menu menumodel)
        {
            ResultDto retResult = new ResultDto();
            retResult.isDone = true;
            checkPageId(menumodel, retResult);
             return retResult;
        }
        private void checkPageId(Menu menumodel, ResultDto retResult)
        {
            if (menumodel == null || menumodel.PageId == null)
            {
                retResult.isDone = false;
                retResult.statusCode = enm_STATUS_CODE.INPUT_NOT_VALID;
                retResult.addValidationMessages("Page_Id is null or empty while it is required for creating menu");
            }
            else
            {
                List<Page> c = Context.Pages.Where(u => u.Id == menumodel.PageId).ToList();
                if (c.Count == 0)
                {
                    retResult.isDone = false;
                    retResult.statusCode = enm_STATUS_CODE.INPUT_NOT_VALID;
                    retResult.addValidationMessages("Page_ID_IS_NOT_VALID");
                }
              
            }
        }
        private void checkPageId(PageMenu menumodel, ResultDto retResult)
        {
            if (menumodel == null || menumodel.PageId == null)
            {
                retResult.isDone = false;
                retResult.statusCode = enm_STATUS_CODE.INPUT_NOT_VALID;
                retResult.addValidationMessages("Page_Id is null or empty while it is required for creating page menu");
            }
            else
            {
                List<Page> c = Context.Pages.Where(u => u.Id == menumodel.PageId).ToList();
                if (c.Count == 0)
                {
                    retResult.isDone = false;
                    retResult.statusCode = enm_STATUS_CODE.INPUT_NOT_VALID;
                    retResult.addValidationMessages("Page_ID_IS_NOT_VALID");
                }

            }
        }
        private void checkMenuId(PageMenu menumodel, ResultDto retResult)
        {
            if (menumodel == null || menumodel.PageId == null)
            {
                retResult.isDone = false;
                retResult.statusCode = enm_STATUS_CODE.INPUT_NOT_VALID;
                retResult.addValidationMessages("Page_Id is null or empty while it is required for creating page menu");
            }
            else
            {
                List<Menu> c = Context.Menus.Where(u => u.Id == menumodel.MenuId).ToList();
                if (c.Count == 0)
                {
                    retResult.isDone = false;
                    retResult.statusCode = enm_STATUS_CODE.INPUT_NOT_VALID;
                    retResult.addValidationMessages("Menu_ID_IS_NOT_VALID");
                }

            }
        }

        public void increaseScore(Guid menuId)
        {
            var me = GetSingle(menuId);
            if(me!=null)
            {
                if (me.Score == null) me.Score = 0;
                me.Score = me.Score + 1;
                Edit(me);
                Save();
            }
        }

        public ResultDto validatePageMenu(PageMenu pagemenumodel)
        {
            ResultDto retResult = new ResultDto();
            retResult.isDone = true;
            checkPageId(pagemenumodel, retResult);
            checkMenuId(pagemenumodel, retResult);
            checkUnquness(pagemenumodel, retResult);
            return retResult;
        }

        public void checkUnquness(PageMenu pagemenumodel, ResultDto retResult)
        {
            if(Context.PageMenus.Where(pm => pm.MenuId == pagemenumodel.MenuId && pm.PageId == pagemenumodel.PageId).Count()>0)
            {
                retResult.addValidationMessages("MENUID_HAS_ALREADY_ADDED_TO_PAGE");
            }
        }

        public List<Menu> findByPageId(Guid pageid)
        {
            return Context.PageMenus.Where(pm => pm.PageId == pageid).Select(p => p.Menu).Distinct().ToList();
        }

        public Guid addOrGetMenuByTitle(string menuTitle,Guid pageid)
        {
           List<Menu> menus=  Context.Menus.Where(m => m.MenuTitle.ToLower().Equals(menuTitle.ToLower())).ToList();
            if (menus.Count > 0)
                return menus[0].Id;
            else
            {
                Menu newMenu = new Menu();
                newMenu.Id = Guid.NewGuid();
                newMenu.MenuTitle = menuTitle;
                newMenu.PageId = pageid;
                newMenu.Score = 0;
                newMenu.CreateDate = DateTime.Now;
                Context.Menus.Add(newMenu);
                Context.SaveChanges();
                return newMenu.Id;
            }
        }

        public List<MenuDto> queryUsedMenuListByPageId(int searchType,Guid pageid,Guid[] selectedMenuIds)
        {
          
            List<MenuDto> retList = new List<MenuDto>();
            //if(selectedMenuIds!=null && selectedMenuIds.Count()>0)
            //{
            //    Context.PageMenus.Where()
            //}
            var result = Context.PageMenus.Where(pm =>
              pm.Page.PageType == searchType &&
             (pageid == Guid.Empty || pm.Page.PageId == pageid || (pm.Page.Page1.Any(p => p.PageId == pageid))) &&
             ( selectedMenuIds.All(mi => pm.Page.PageMenus.Any(pm2 => pm2.MenuId == mi)))).GroupBy(pm => pm.MenuId)
             .Select(g => new GroupClass { Id = g.Key, count = g.Count() }).OrderByDescending(g=>g.count);
            foreach (var item in result)
            {
                MenuDto newMenuDto = new MenuDto();
                newMenuDto = Mapper<MenuDto, MenuDto>.convertToDto(GetSingle(item.Id));
                newMenuDto.usedCount = item.count;
                retList.Add(newMenuDto);
            }

            return retList;


        }
        class GroupClass
        {
            public Guid Id { get; set; }
            public int count { get; set; }
        }
    }
}