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
        public List<FollowDto> getFollowingRequestForProfileAndPages(Guid profileId)
        {
            var fl=Context.Follows.Where(f => f.FollowingPageId == profileId || f.Page.PageId == profileId).ToList();
            return Mapper<Follow, FollowDto>.convertListToDto(fl);

        }
        public List<FollowDto> getFollowingRequestForProfile(Guid profileId)
        {
            var fl = Context.Follows.Where(f => f.FollowingPageId == profileId).ToList();
            return Mapper<Follow, FollowDto>.convertListToDto(fl);

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

        public List<FollowDto> getPageFollowers(Guid pageid)
        {
         var fls=   Context.Follows.Where(f => f.FollowingPageId == pageid).ToList();
            return Mapper<Follow, FollowDto>.convertListToDto(fls);
        }
        public IQueryable<PageDto> getPageFollowers(Guid pageid,Guid[] menulist)
        {
            if (menulist == null) menulist = new Guid[0];
            var fls = Context.Follows.Where(f => f.FollowingPageId == pageid && menulist.All(mi => f.Page1.PageMenus.Any(pm => mi == pm.MenuId))).Select(fp=>fp.Page1).AsQueryable();
            return Mapper<Page, PageDto>.convertIquerybleToDto(fls);
        }
    }
}