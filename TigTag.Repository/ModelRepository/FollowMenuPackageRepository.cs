using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TigTag.DataModel.model;
using TigTag.Repository.IModelRepository;
using TiTag.Repository.Base;


namespace TigTag.Repository.ModelRepository {

    public class FollowMenuPackageRepository : 
        GenericRepository<DataModelContext, FollowMenuPackage>, IFollowMenuPackageRepository  {

        public FollowMenuPackage GetSingle(Guid Id) {

            var query = Context.FollowMenuPackages.FirstOrDefault(x => x.Id ==Id );
            return query;
        }
    }
}