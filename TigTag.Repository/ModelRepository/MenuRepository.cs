using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TigTag.DataModel.model;
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
    }
}