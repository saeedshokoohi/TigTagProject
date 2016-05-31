using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TigTag.DataModel.model;
using TigTag.DTO.ModelDTO.Base;
using TigTag.Repository.IModelRepository;
using TiTag.Repository.Base;


namespace TigTag.Repository.ModelRepository {

    public class FollowMenuRepository : 
        GenericRepository<DataModelContext, FollowMenu>, IFollowMenuRepository  {

        public FollowMenu GetSingle(Guid Id) {

            var query = Context.FollowMenus.FirstOrDefault(x => x.Id ==Id );
            return query;
        }

        public ResultDto validateFollowMenu(FollowMenu followModel)
        {
            ResultDto retResult = new ResultDto();
            retResult.isDone = true;
            checkFollowerPageId(followModel, retResult);
            if (retResult.isDone)
                retResult.statusCode = enm_STATUS_CODE.DONE_SUCCESSFULLY;
            return retResult;
        }
        private void checkFollowerPageId(FollowMenu followModel, ResultDto retResult)
        {
            if (followModel == null || followModel.FollowerUserId == null)
            {
                retResult.isDone = false;
                retResult.statusCode = enm_STATUS_CODE.INPUT_NOT_VALID;
                retResult.addValidationMessages("follower page_id is null or empty while it is required for creating followMenu");
            }
            else
            {
                var c = Context.Pages.Count(u => u.Id == followModel.FollowerUserId);
                if (c == 0)
                {
                    retResult.isDone = false;
                    retResult.statusCode = enm_STATUS_CODE.INPUT_NOT_VALID;
                    retResult.addValidationMessages("FOLLOWER_PAGE_ID_IS_NOT_VALID");
                }
            }
        }
    }
}