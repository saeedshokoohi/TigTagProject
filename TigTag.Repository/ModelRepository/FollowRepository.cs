using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TigTag.DataModel.model;
using TigTag.Repository.IModelRepository;
using TiTag.Repository.Base;


namespace TigTag.Repository.ModelRepository {

    public class FollowRepository : 
        GenericRepository<DataModelContext, Follow>, IFollowRepository  {

        public Follow GetSingle(Guid Id) {

            var query = Context.Follows.FirstOrDefault(x => x.Id ==Id );
            return query;
        }
    }
}