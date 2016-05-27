using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TigTag.DataModel.model;
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

       
    }
}