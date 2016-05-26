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
        /// <summary>
        /// this method just retrieves the thumbnail image from database
        /// </summary>
        /// <param name="imageid"></param>
        /// <returns></returns>
        public ImageTable GetThumbnailImageOnly(Guid imageid)
        {
            ImageTable imageTable = new ImageTable();
            var query = from i in Context.ImageTables where i.Id == imageid
                        select new ImageTable(i.Id,i.ImageName,i.ImageType,i.ThumbnailData);
            List<ImageTable> images= query.ToList();
            if (images.Count > 0)
                return images[0];
            else
                return new ImageTable();

        }
    }
}