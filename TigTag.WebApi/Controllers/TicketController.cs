using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TigTag.DataModel.model;
using TigTag.DTO.ModelDTO.Base;
using TigTag.DTO.ModelDTO;
using TigTag.Repository.ModelRepository;

using TiTag.Repository;
using TigTag.Repository;
using TigTag.Common.Enumeration;
using TigTag.Common.util;
using System.Web.OData;

namespace TigTag.WebApi.Controllers
{
    public class TicketController : BaseController<Ticket, TicketDto>
    {

        TicketRepository TicketRepo = new TicketRepository();
        MenuRepository menuRepo = new MenuRepository();
        PageRepository pageRepo = new PageRepository();

        public TicketController()
        {
            eventLogRepo.Context = TicketRepo.Context;
        }
        public override IGenericRepository<Ticket> getRepository()
        {
            return TicketRepo;
        }
        public ResultDto addTicketList(TicketDto[] tickets)
        {
            ResultDto returnResult = new ResultDto();
            if (tickets!=null)
            {
                foreach (var item in tickets)
                {
                    Ticket ticketModel = Mapper<Ticket, TicketDto>.convertToModel(item);
                    returnResult = TicketRepo.validateTicket(ticketModel);
                    if (!returnResult.isDone)
                        return returnResult;
                }
                List<string> retIdList = new List<string>();

                foreach (var item in tickets)
                {
                  returnResult=addTicket(item);
                    if (returnResult.isDone)
                        retIdList.Add(returnResult.returnId);
                }
                returnResult.returnIdList = retIdList;
               
           }
            return returnResult;
        }
        public ResultDto editTicketList(TicketListDto TicketListDto)
        {
            ResultDto result = new ResultDto();

            Page p = pageRepo.GetSingle(TicketListDto.pageId);
            pageRepo.Detach(p);
            if (p == null) throwException("pageid is not valid");
            if (p.UserId != getCurrentUserId()) throwException("current user is not the owner of pageid and can not edit pageAdmin");
            List<Ticket> currentList = TicketRepo.getTicketByPage(p.Id);
            List<Guid> toDeleteList = new List<Guid>();
            List<TicketDto> toAddList = new List<TicketDto>();
            List<TicketDto> toEditList = new List<TicketDto>();

            foreach (var item in TicketListDto.TicketList)
            {
                if (item.Id != Guid.Empty)
                {
                    Ticket temp = TicketRepo.GetSingle(item.Id);
                    if (temp == null) throwException("ticket id:" + item.Id + "is not valid");
                    toEditList.Add(item);
                }
            }
            foreach (var item in TicketListDto.TicketList)
            {

                item.PageId = TicketListDto.pageId;
                if (!currentList.Any(pa => pa.Id == item.Id))
                    toAddList.Add(item);

            }
            foreach (var item in currentList)
            {
                TicketRepo.Detach(item);
                if (!TicketListDto.TicketList.Any(ci => ci.Id == item.Id))
                    toDeleteList.Add(item.Id);
            }
            foreach (var item in toEditList)
            {
                result = editTicketNotSave(item);
                if (!result.isDone) return result;
            }
            foreach (var item in toAddList)
            {
                result = addTicketNotSave(item);
                if (!result.isDone) return result;
            }
            foreach (var item in toDeleteList)
            {
                Ticket temp = TicketRepo.GetSingle(item);

                eventLogRepo.RemoveTicketEvent(getCurrentProfileId(), temp);
                TicketRepo.Delete(temp);

            }
            try
            {
                TicketRepo.Save();


                return ResultDto.successResult("", String.Format("{0} item added and {1} item removed and {2} item edited ",
                    toAddList.Count().ToString(), toDeleteList.Count().ToString(), toEditList.Count()));
            }
            catch (Exception ex)
            {
                return ResultDto.exceptionResult(ex);
            }


        }

        private ResultDto addTicketNotSave(TicketDto ticket)
        {
            if (ticket == null) return ResultDto.failedResult("Invalid Raw Payload data, it must be an json object  ");
            else
            {
                ResultDto returnResult = new ResultDto();
                Ticket ticketModel = Mapper<Ticket, TicketDto>.convertToModel(ticket);
                ticketModel.Id = Guid.NewGuid();
                ticketModel.SoldCapacity = 0;

                returnResult = TicketRepo.validateTicket(ticketModel);
                if (returnResult.isDone)
                {
                    try
                    {
                        TicketRepo.Add(ticketModel);
                     
                        returnResult.isDone = true;
                        returnResult.message = "new Ticket created successfully";
                        returnResult.returnId = ticketModel.Id.ToString();
                        if (ticket.ProfileId == Guid.Empty) ticket.ProfileId = getCurrentProfileId();
                        eventLogRepo.AddTicketEvent(ticket.ProfileId, ticketModel);
                    }
                    catch (Exception ex)
                    {
                        returnResult = ResultDto.exceptionResult(ex);


                    }

                }

                return returnResult;
            }
        }
        private ResultDto editTicketNotSave(TicketDto ticket)
        {
            if (ticket == null) return ResultDto.failedResult("Invalid Raw Payload data, it must be an json object  ");
            else
            {
                ResultDto returnResult = new ResultDto();
                Ticket ticketModel = Mapper<Ticket, TicketDto>.convertToModel(ticket);
                ticketModel.ModifiedBy = getCurrentUserId();
                ticketModel.ModifiedDate = DateTime.Now;
            

                returnResult = TicketRepo.validateTicket(ticketModel);
                if (returnResult.isDone)
                {
                    try
                    {
                        TicketRepo.Edit(ticketModel);

                        returnResult.isDone = true;
                        returnResult.message = " Ticket edited successfully";
                        returnResult.returnId = ticketModel.Id.ToString();
                        if (ticket.ProfileId == Guid.Empty) ticket.ProfileId = getCurrentProfileId();
                        eventLogRepo.EditTicketEvent(ticket.ProfileId, ticketModel);
                    }
                    catch (Exception ex)
                    {
                        returnResult = ResultDto.exceptionResult(ex);


                    }

                }

                return returnResult;
            }
        }

       

        public ResultDto addTicket(TicketDto ticket)
        {
            if (ticket == null) return ResultDto.failedResult("Invalid Raw Payload data, it must be an json object  ");
            else
            {
                ResultDto returnResult = new ResultDto();
                Ticket ticketModel = Mapper<Ticket, TicketDto>.convertToModel(ticket);
                ticketModel.Id = Guid.NewGuid();
                ticketModel.SoldCapacity = 0;
               
                returnResult = TicketRepo.validateTicket(ticketModel);
                if (returnResult.isDone)
                {
                    try
                    {
                        TicketRepo.Add(ticketModel);
                        TicketRepo.Save();
                        returnResult.isDone = true;
                        returnResult.message = "new Ticket created successfully";
                        returnResult.returnId = ticketModel.Id.ToString();
                        if (ticket.ProfileId == Guid.Empty) ticket.ProfileId = getCurrentProfileId();
                        eventLogRepo.AddTicketEvent(ticket.ProfileId, ticketModel);
                    }
                    catch (Exception ex)
                    {
                        returnResult = ResultDto.exceptionResult(ex);


                    }

                }

                return returnResult;
            }
        }
    }

    public class TicketListDto
    {
        public Guid pageId { get;  set; }
        public List<TicketDto> TicketList { get;  set; }
    }
}