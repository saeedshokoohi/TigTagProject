using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TigTag.DataModel.model;
using TigTag.Repository.IModelRepository;
using TiTag.Repository.Base;


namespace TigTag.Repository.ModelRepository {

    public class ImageTableRepository : 
        GenericRepository<DataModelContext, ImageTable>, IImageTableRepository  {

        public ImageTable GetSingle(Guid Id) {

            var query = Context.ImageTables.FirstOrDefault(x => x.Id ==Id );
            return query;
        }
    }
}