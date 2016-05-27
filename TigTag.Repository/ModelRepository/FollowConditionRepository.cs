using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TigTag.DataModel.model;
using TigTag.Repository.IModelRepository;
using TiTag.Repository.Base;


namespace TigTag.Repository.ModelRepository {

    public class FollowConditionRepository : 
        GenericRepository<DataModelContext, FollowCondition>, IFollowConditionRepository  {

        public FollowCondition GetSingle(Guid Id) {

            var query = Context.FollowConditions.FirstOrDefault(x => x.Id ==Id );
            return query;
        }
    }
}