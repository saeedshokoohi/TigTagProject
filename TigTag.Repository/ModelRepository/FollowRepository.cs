using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TigTag.DataModel.model;
using TigTag.DTO.ModelDTO.Base;
using TigTag.Repository.IModelRepository;
using TiTag.Repository.Base;


namespace TigTag.Repository.ModelRepository {

    public class FollowRepository : 
        GenericRepository<DataModelContext, Follow>, IFollowRepository  {
        PageRepository pageRepo = new PageRepository();

        public Follow GetSingle(Guid Id) {

            var query = Context.Follows.FirstOrDefault(x => x.Id ==Id );
            return query;
        }

        public ResultDto validateFollow(Follow followModel)
        {
            ResultDto retResult = new ResultDto();
            retResult.isDone = true;
            checkFollowerPageId(followModel, retResult);
            checkFollowingPageId(followModel, retResult);
            
            if (retResult.isDone)
                retResult.statusCode = enm_STATUS_CODE.DONE_SUCCESSFULLY;
            return retResult;
        }

        private void checkFollowingPageId(Follow followModel, ResultDto retResult)
        {
            if (followModel == null || followModel.FollowingPageId == null)
            {
                retResult.isDone = false;
                retResult.statusCode = enm_STATUS_CODE.INPUT_NOT_VALID;
                retResult.addValidationMessages("following page_id is null or empty while it is required for creating follow");
            }
            else
            {
                var c = Context.Pages.Count(u => u.Id == followModel.FollowingPageId);
                if (c == 0)
                {
                    retResult.isDone = false;
                    retResult.statusCode = enm_STATUS_CODE.INPUT_NOT_VALID;
                    retResult.addValidationMessages("FOLLOWING_PAGE_ID_IS_NOT_VALID");
                }
            }
        }

        private void checkFollowerPageId(Follow followModel, ResultDto retResult)
        {
            if (followModel == null || followModel.FollowerUserId == null)
            {
                retResult.isDone = false;
                retResult.statusCode = enm_STATUS_CODE.INPUT_NOT_VALID;
                retResult.addValidationMessages("follower page_id is null or empty while it is required for creating follow");
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