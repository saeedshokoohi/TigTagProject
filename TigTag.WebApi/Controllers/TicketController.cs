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
}