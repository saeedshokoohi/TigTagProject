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
    public class OrderItemController : BaseController<OrderItem, OrderItemDto>
    {

        OrderItemRepository OrderItemRepo = new OrderItemRepository();
        MenuRepository menuRepo = new MenuRepository();
        PageRepository pageRepo = new PageRepository();

        public override IGenericRepository<OrderItem> getRepository()
        {
            return OrderItemRepo;
        }
        public ResultDto addOrderItem(OrderItemDto OrderItem)
        {
            if (OrderItem == null) return ResultDto.failedResult("Invalid Raw Payload data, it must be an json object  ");
            else
            {
                ResultDto returnResult = new ResultDto();
                OrderItem OrderItemModel = Mapper<OrderItem, OrderItemDto>.convertToModel(OrderItem);
                OrderItemModel.Id = Guid.NewGuid();

                returnResult = OrderItemRepo.validateOrderItem(OrderItemModel);
                if (returnResult.isDone)
                {
                    try
                    {
                        OrderItemRepo.Add(OrderItemModel);
                        OrderItemRepo.Save();
                        returnResult.isDone = true;
                        returnResult.message = "new OrderItem created successfully";
                        returnResult.returnId = OrderItemModel.Id.ToString();
                        if (OrderItem.ProfileId == Guid.Empty) OrderItem.ProfileId = getCurrentProfileId();
                        eventLogRepo.AddOrderItemEvent(OrderItem.ProfileId, OrderItemModel);

                        //update soled Capacity
                        TicketRepository tickerRepo = new TicketRepository();
                        tickerRepo.addSoledCapacity(OrderItemModel.TicketId,1);

                     //   tickerRepo.Save();
                    }
                    catch (Exception ex)
                    {
                        returnResult = ResultDto.exceptionResult(ex);


                    }

                }

                return returnResult;
            }
        }

        internal ResultDto addOrderItemList(Guid orderid, List<OrderItemDto> orderItems,out double totalPrice)
        {
            totalPrice = 0;
            ResultDto retResult = new ResultDto();

           if(orderItems!=null)
                foreach (var item in orderItems)
                {
                    item.OrderId = orderid;
                    OrderItem itemModel = Mapper<OrderItem, OrderItemDto>.convertToModel(item);
                    retResult = OrderItemRepo.validateOrderItem(itemModel);
                  
                    if (!retResult.isDone) return retResult;
                }
            TicketRepository ticketRepo = new TicketRepository();
            foreach (var item in orderItems)
            {
                retResult= addOrderItem(item);
               Ticket t = ticketRepo.GetSingle(item.TicketId);
                if(t!=null && t.Price!=null)
                totalPrice+= ((double)t.Price);
            }
          
            return retResult;
        }
    }
}