using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TigTag.Common.Enumeration;
using TigTag.DataModel.model;
using TigTag.DTO.ModelDTO;
using TigTag.DTO.ModelDTO.Base;
using TigTag.Repository.IModelRepository;
using TiTag.Repository.Base;


namespace TigTag.Repository.ModelRepository
{


    public class EventsLogRepository :
        GenericRepository<DataModelContext, EventsLog>, IEventsLogRepository
    {
        PageRepository pageRepo = new PageRepository();

        public override EventsLog GetSingle(Guid Id)
        {

            var query = Context.EventsLogs.FirstOrDefault(x => x.Id == Id);
            return query;
        }
        public ResultDto AddProfileEvent(Guid profleId,Page page)
        {
            return AddSingleLog(profleId, page, enmEventsActionType.CREATE_PROFILE);
        }

        public List<EventsLogDto> geProfileEvents(Guid pageId)
        {
            var List= Context.EventsLogs.Where(el => el.OwnerPageId == pageId).OrderByDescending(el => el.CreateDate).ToList();
            return Mapper<EventsLog, EventsLogDto>.convertListToDto(List);
        }

        public ResultDto AddPageEvent(Guid profleId, Page page)
        {
            return AddSingleLog(profleId, page, enmEventsActionType.CREATE_PAGE);
        }
        public ResultDto AddTeamEvent(Guid profleId, Page page)
        {
            return AddSingleLog(profleId, page, enmEventsActionType.CREATE_TEAM);
        }
        public ResultDto AddPostEvent(Guid profleId, Page page)
        {
            return AddSingleLog(profleId, page, enmEventsActionType.CREATE_POST);
        }
        public ResultDto AddParticipantEvent(Guid profleId, Participant p)
        {
            return AddSingleLog(profleId, p, enmEventsActionType.TAKE_PARTICIPATE);
        }

        public ResultDto AddOrderEvent(Guid profileId, Order orderModel)
        {
            return AddSingleLog(profileId, orderModel, enmEventsActionType.CREATE_ORDER);
        }

        public ResultDto AddOrderItemEvent(Guid profileId, OrderItem orderItemModel)
        {
            return AddSingleLog(profileId, orderItemModel, enmEventsActionType.ADD_ORDER_ITEM);
        }

        public ResultDto addPageAdminEvent(Guid profileId, PageAdmin pageAdminModel)
        {
            return AddSingleLog(profileId, pageAdminModel, enmEventsActionType.ROLE_AS_ADMIN
                );
        }

        public ResultDto AddContactInfoEvent(Guid profileId, ContactInfo contactInfoModel)
        {
            return AddSingleLog(profileId, contactInfoModel, enmEventsActionType.ADD_CONTACT_INFO);
        }

        public ResultDto AddTicketEvent(Guid profileId, Ticket ticketModel)
        {
            return AddSingleLog(profileId, ticketModel, enmEventsActionType.CREATE_TICKET);
        }

        public ResultDto AddFollowPageEvent(Guid profleId, Follow p)
        {
            return AddSingleLog(profleId, p, enmEventsActionType.FOLLOWING_PAGE);
        }
        public ResultDto AddFollowMenuEvent(Guid profleId, FollowMenu p)
        {
            return AddSingleLog(profleId, p, enmEventsActionType.FOLLOWING_MENU);
        }
        public ResultDto AddCommentEvent(Guid profleId, PageComment p)
        {
            return AddSingleLog(profleId, p, enmEventsActionType.ADD_COMMENT);
        }
        public ResultDto AddCommentReply(Guid profleId, CommentReply p)
        {
            return AddSingleLog(profleId, p, enmEventsActionType.ADD_COMMENTREPLY);
        }



        private ResultDto AddSingleLog(Guid actorProfileId ,Page page,enmEventsActionType actionType)
        {
            ResultDto retResultDto = new ResultDto();

            switch (actionType)
            {
                case enmEventsActionType.CREATE_PROFILE:
                //    retResultDto = addGeneralEvent(page.Id, page.Id, page.Id, enmEventsActionType.CREATE_PROFILE);
                  //  Page masterPage= pageRepo.getMasterPage();
                //    if(masterPage!=null)
                    retResultDto = addGeneralEvent(page.Id, page.Id, page.Id, enmEventsActionType.CREATE_PROFILE);
                    break;
                case enmEventsActionType.ROLE_AS_ADMIN:
                case enmEventsActionType.CREATE_PAGE:
                case enmEventsActionType.CREATE_TEAM:
                case enmEventsActionType.CREATE_POST:


                    retResultDto= addGeneralEventWithParent(actorProfileId, page.Id, (Guid)page.Id,actionType);
                 
                
                    break;
                default:
                    break;
            }
            return retResultDto;
        }
        private ResultDto AddSingleLog(Guid actorProfileId, PageAdmin page, enmEventsActionType actionType)
        {
            ResultDto retResultDto = new ResultDto();

            switch (actionType)
            {
               
                case enmEventsActionType.ROLE_AS_ADMIN:
                    retResultDto = addGeneralEvent(actorProfileId, page.Id, actorProfileId, actionType);
                    retResultDto = addGeneralEventWithParent(actorProfileId, page.Id, (Guid)page.PageId, actionType);


                    break;
                default:
                    break;
            }
            return retResultDto;
        }
        private ResultDto AddSingleLog(Guid actorProfileId,  Participant participant, enmEventsActionType actionType)
        {
            ResultDto retResultDto = new ResultDto();

            switch (actionType)
            {
             
                case enmEventsActionType.TAKE_PARTICIPATE:
                    retResultDto=addGeneralEventWithParent(actorProfileId, participant.Id, participant.ParticipantPageId, actionType);
                    retResultDto = addGeneralEventWithParent(actorProfileId, participant.Id, actorProfileId, actionType);

                    break;
                default:
                    break;
            }
            return retResultDto;
        }
        private ResultDto AddSingleLog(Guid actorProfileId, Order order, enmEventsActionType actionType)
        {
            ResultDto retResultDto = new ResultDto();

            switch (actionType)
            {

                case enmEventsActionType.CREATE_ORDER:
                    retResultDto = addGeneralEventWithParent(actorProfileId, order.Id, order.CustomerPageId, actionType);
                    retResultDto = addGeneralEventWithParent(actorProfileId, order.Id, order.PageId, actionType);
       

                    break;
                default:
                    break;
            }
            return retResultDto;
        }
        private ResultDto AddSingleLog(Guid actorProfileId, OrderItem order, enmEventsActionType actionType)
        {
            ResultDto retResultDto = new ResultDto();

            switch (actionType)
            {

                case enmEventsActionType.ADD_ORDER_ITEM:
                    retResultDto = addGeneralEventWithParent(actorProfileId, order.Id, actorProfileId, actionType);
                  


                    break;
                default:
                    break;
            }
            return retResultDto;
        }
        private ResultDto AddSingleLog(Guid actorProfileId, ContactInfo contactInfo, enmEventsActionType actionType)
        {
            ResultDto retResultDto = new ResultDto();

            switch (actionType)
            {

                case enmEventsActionType.ADD_CONTACT_INFO:
                    retResultDto = addGeneralEventWithParent(actorProfileId, contactInfo.Id, contactInfo.PageId, actionType);



                    break;
                default:
                    break;
            }
            return retResultDto;
        }
        private ResultDto AddSingleLog(Guid actorProfileId, Ticket ticket, enmEventsActionType actionType)
        {
            ResultDto retResultDto = new ResultDto();

            switch (actionType)
            {

                case enmEventsActionType.CREATE_TICKET:
                    retResultDto = addGeneralEventWithParent(actorProfileId, ticket.Id, ticket.PageId, actionType);



                    break;
                default:
                    break;
            }
            return retResultDto;
        }
        private ResultDto AddSingleLog(Guid actorProfileId, Follow follow, enmEventsActionType actionType)
        {
            ResultDto retResultDto = new ResultDto();

            switch (actionType)
            {

        
                case enmEventsActionType.FOLLOWING_PAGE:
                    retResultDto = addGeneralEvent(actorProfileId, follow.Id, actorProfileId, actionType);
                    retResultDto = addGeneralEventWithParent(actorProfileId, follow.Id, follow.FollowingPageId, actionType);
                 
                    break;
            
                default:
                    break;
            }
            return retResultDto;

        }
        private ResultDto AddSingleLog(Guid actorProfileId, FollowMenu followMenu, enmEventsActionType actionType)
        {
            ResultDto retResultDto = new ResultDto();

            switch (actionType)
            {


                case enmEventsActionType.FOLLOWING_MENU:
                    retResultDto = addGeneralEvent(actorProfileId, followMenu.Id, actorProfileId, actionType);
                  
                    break;
              
                default:
                    break;
            }
            return retResultDto;

        }
        private ResultDto AddSingleLog(Guid actorProfileId, PageComment pageComment, enmEventsActionType actionType)
        {
            ResultDto retResultDto = new ResultDto();

            switch (actionType)
            {


                case enmEventsActionType.ADD_COMMENT:
                    retResultDto = addGeneralEvent(actorProfileId, pageComment.Id, actorProfileId, actionType);

                    retResultDto = addGeneralEventWithParent(actorProfileId, pageComment.Id, pageComment.PageId, actionType);

                    break;
                case enmEventsActionType.ADD_COMMENTREPLY:
                    break;
                default:
                    break;
            }
            return retResultDto;

        }
        private ResultDto AddSingleLog(Guid actorProfileId, CommentReply commentReply, enmEventsActionType actionType)
        {
            ResultDto retResultDto = new ResultDto();

            switch (actionType)
            {


                case enmEventsActionType.ADD_COMMENTREPLY:
                    retResultDto = addGeneralEvent(actorProfileId, commentReply.Id, actorProfileId, actionType);
                    PageComment pc = Context.PageComments.Where(p => p.Id == commentReply.PageCommentId).Single();
                    if (pc != null)
                    {
                        retResultDto = addGeneralEventWithParent(actorProfileId, commentReply.Id, pc.PageId, actionType);
                        retResultDto = addGeneralEventWithParent(actorProfileId, commentReply.Id, pc.AutherId, actionType);
                    }

                    break;
             
                default:
                    break;
            }
            return retResultDto;

        }


        private ResultDto addGeneralEventWithParent(Guid actorProfileId, Guid targetId, Guid ownerId, enmEventsActionType actionType)
        {
            ResultDto retResultDto = new ResultDto();
            addGeneralEvent(actorProfileId, targetId, ownerId, actionType);
            Guid? parentid = findParentPage(ownerId);
            int maxlevel = 6;
            while (parentid != null && maxlevel>0)
            {
                try
                {
                    retResultDto = addGeneralEvent(actorProfileId, targetId, (Guid)parentid, actionType);
                    parentid = findParentPage(parentid);
                    maxlevel--;
                }catch
                { 
                    maxlevel = 0;
                }
            }
            return retResultDto;
        }

        private Guid? findParentPage(Guid? pageId)
        {
            Guid? retPageId = null;
            if(pageId!=null)
            retPageId= Context.Pages.Where(p => p.Id == (Guid)pageId).Select(p => p.PageId).ToList()[0];
            return retPageId;
        }

        public ResultDto addGeneralEvent(Guid actorId, Guid targetid, Guid ownerid, enmEventsActionType action)
        {
            ResultDto result = new ResultDto();
            EventsLog newEventsLog = new EventsLog();
            
            if(actorId==null)

            if (!isPageIdValid(actorId))
            {
                result.addValidationMessages("actor page id is not valid!");
            }
          
            if (!isPageIdValid(ownerid))
            {
                result.addValidationMessages("owner page id is not valid!");
            }
            if (result.isDone)
            {
                newEventsLog.Id = Guid.NewGuid();
                newEventsLog.CreateDate = DateTime.Now;
                newEventsLog.ActionCode = action.GetHashCode();
                newEventsLog.IsActive = true;
                newEventsLog.OwnerPageId = ownerid;
                newEventsLog.TargetId = targetid;
                newEventsLog.ActorPageId = actorId;
                newEventsLog.Status = 0;
                
                try
                {
                    Add(newEventsLog);
                    Save();
                }
                catch (Exception ex)
                {
                    result = ResultDto.exceptionResult(ex);
                }
            }
            return result;
        }

        private bool isPageIdValid(Guid actorId)
        {
            return (actorId != null && Context.Pages.Count(p => p.Id == actorId) > 0);

        }

        public ResultDto validateEventsLog(EventsLog EventsLog)
        {
            ResultDto retResult = new ResultDto();
            retResult.isDone = true;
            checkPageCommentId(EventsLog, retResult);
            if (retResult.isDone)
                retResult.statusCode = enm_STATUS_CODE.DONE_SUCCESSFULLY;
            return retResult;

        }

        private void checkPageCommentId(EventsLog EventsLog, ResultDto retResult)
        {

        }


    }
}