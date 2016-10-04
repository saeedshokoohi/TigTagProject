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

    public class PageMenuRepository : 
        GenericRepository<DataModelContext, PageMenu>, IPageMenuRepository  {

        public PageMenu GetSingle(Guid Id) {

            var query = Context.PageMenus.FirstOrDefault(x => x.Id ==Id );
            return query;
        }

        public void UpagePageMenuListForPage(PageDto page)
        {
            MenuRepository menuRepo = new MenuRepository();
            List<Guid> oldMenuList= Context.PageMenus.Where(pm => pm.PageId == page.Id).Select(pm=>pm.MenuId).ToList();
            List<Guid> toDeletePageMenuList = new List<Guid>();
            List<Guid> toAddPageMenuList = new List<Guid>();
            //obtain list of menu which should be added
            foreach (var pm in page.Menulist)
            {
                if (!oldMenuList.Contains(pm))
                    toAddPageMenuList.Add(pm);
            }
            //obtain list of menu which should be deleted
            foreach (var pm in oldMenuList)
            {
                if (!page.Menulist.Contains(pm))
                    toDeletePageMenuList.Add(pm);
            }
            //adding new pageMenus
            foreach (var item in toAddPageMenuList)
            {
                PageMenu newPm = new PageMenu();
                newPm.Id = Guid.NewGuid();
                newPm.MenuId = item;
                newPm.PageId = page.Id;
                Add(newPm);
                Save();
                menuRepo.increaseScore(item);
            }
            //removing some page menues
                var pmList=Context.PageMenus.Where(pm => pm.PageId == page.Id && toDeletePageMenuList.Any(tdm=>tdm==pm.MenuId));
                 Context.PageMenus.RemoveRange(pmList);
            Save();
            foreach (var item in toDeletePageMenuList)
            {
                menuRepo.DecreaseScore(item);
            }

          
           


        }
    }
}