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

    public class OrderItemRepository :
        GenericRepository<DataModelContext, OrderItem>, IOrderItemRepository
    {
        PageRepository pageRepo = new PageRepository();

        public OrderItem GetSingle(Guid Id)
        {

            var query = Context.OrderItems.FirstOrDefault(x => x.Id == Id);
            return query;
        }

        public ResultDto validateOrderItem(OrderItem orderItemModel)
        {
            return validateOrderItem(orderItemModel, true);
        }

        private void checkTicketCapacity(OrderItem orderItemModel, ResultDto retResult)
        {
            try
            {
                var c = Context.Tickets.First(p => p.Id == orderItemModel.TicketId);
                if (c.SoldCapacity + 1 >= c.Capacity)
                    retResult.addValidationMessages("No Available Capacity!! for TicketId : " + orderItemModel.TicketId);
            }catch(Exception ex)
            {
                retResult.addValidationMessages("ticketId is not valid!!");
            }
        }

        private void checkOrderId(OrderItem orderModel, ResultDto retResult)
        {
            var c = Context.Orders.Count(p => p.Id == orderModel.OrderId);
            if (c == 0 || orderModel.OrderId == null)
            {
                retResult.addValidationMessages("OrderId is not valid or is null!");
            }
             c = Context.Tickets.Count(p => p.Id == orderModel.TicketId);
            if (c == 0 || orderModel.TicketId == null)
            {
                retResult.addValidationMessages("TicketId is not valid or is null!");
            }
        }

        public ResultDto validateOrderItem(OrderItem orderItemModel, bool ifcheckOrderId)
        {
            ResultDto retResult = new ResultDto();
            retResult.isDone = true;
            if(ifcheckOrderId)
            checkOrderId(orderItemModel, retResult);
            checkTicketCapacity(orderItemModel, retResult);
            if (retResult.isDone)
                retResult.statusCode = enm_STATUS_CODE.DONE_SUCCESSFULLY;
            return retResult;
        }
    }
       
}