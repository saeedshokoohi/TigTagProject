using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TigTag.DataModel.model;
using TigTag.DTO.ModelDTO;
using TigTag.DTO.ModelDTO.Base;
using TigTag.Repository.IModelRepository;
using TiTag.Repository.Base;


namespace TigTag.Repository.ModelRepository {

 
    public class CommentReplyRepository : 
        GenericRepository<DataModelContext, CommentReply>, ICommentReplyRepository  {

        public override CommentReply GetSingle(Guid Id) {

            var query = Context.CommentReplys.FirstOrDefault(x => x.Id ==Id );
            return query;
        }

    }
}