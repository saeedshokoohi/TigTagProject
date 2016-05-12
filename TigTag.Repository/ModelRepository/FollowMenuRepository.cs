using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TigTag.DataModel.model;
using TigTag.Repository.IModelRepository;
using TiTag.Repository.Base;


namespace TigTag.Repository.ModelRepository {

    public class FollowMenuRepository : 
        GenericRepository<DataModelContext, FollowMenu>, IFollowMenuRepository  {

        public FollowMenu GetSingle(Guid Id) {

            var query = Context.FollowMenus.FirstOrDefault(x => x.Id ==Id );
            return query;
        }
    }
}