﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TigTag.DataModel.model;
using TigTag.Repository.IModelRepository;
using TiTag.Repository.Base;


namespace TigTag.Repository.ModelRepository {

    public class BasePageRepository : 
        GenericRepository<DataModelContext, BasePage>, IBasePageRepository  {

        public BasePage GetSingle(Guid Id) {

            var query = Context.BasePages.FirstOrDefault(x => x.Id ==Id );
            return query;
        }
    }
}