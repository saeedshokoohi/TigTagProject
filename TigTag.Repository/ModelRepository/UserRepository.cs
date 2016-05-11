using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TigTag.DataModel.model;
using TigTag.Repository.IModelRepository;
using TiTag.Repository.Base;


namespace TigTag.Repository.ModelRepository {

    public class UserRepository : 
        GenericRepository<DataModelContext, User>, IUserRepository  {

        public User GetSingle(Guid Id) {

            var query = Context.Users.FirstOrDefault(x => x.Id ==Id );
            return query;
        }
    }
}