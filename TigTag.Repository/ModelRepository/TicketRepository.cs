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

    public class TicketRepository :
        GenericRepository<DataModelContext, Ticket>, ITicketRepository
    {
        PageRepository pageRepo = new PageRepository();

        public Ticket GetSingle(Guid Id)
        {

            var query = Context.Tickets.FirstOrDefault(x => x.Id == Id);
            return query;
        }

        public ResultDto validateTicket(Ticket ticketModel)
        {
            ResultDto retResult = new ResultDto();
            retResult.isDone = true;
            checkPageId(ticketModel, retResult);
          
            if (retResult.isDone)
                retResult.statusCode = enm_STATUS_CODE.DONE_SUCCESSFULLY;
            return retResult;

        }


        private void checkPageId(Ticket ticketMode, ResultDto retResult)
        {
            var c = Context.Pages.Count(p => p.Id == ticketMode.PageId);
            if (c == 0 || ticketMode.PageId == null)
            {
                retResult.addValidationMessages("pageId is not valid or is null!");
            }
            

        }

        public void addSoledCapacity(Guid ticketId, int soldCount)
        {
            Ticket ticket= GetSingle(ticketId);
            if (ticket != null)
            {
                if (ticket.SoldCapacity == null) ticket.SoldCapacity = 0;
                ticket.SoldCapacity = ticket.SoldCapacity + soldCount;
                Edit(ticket);
                Save();
              
            }
          
        }
    }
       
}