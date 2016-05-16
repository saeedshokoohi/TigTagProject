using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TigTag.DataModel.model;
using TigTag.Repository.IModelRepository;
using TiTag.Repository.Base;


namespace TigTag.Repository.ModelRepository {

    public class MenuRepository : 
        GenericRepository<DataModelContext, Menu>, IMenuRepository  {

        public Menu GetSingle(Guid Id) {

            var query = Context.Menus.FirstOrDefault(x => x.Id ==Id );
            return query;
        }
    }
}