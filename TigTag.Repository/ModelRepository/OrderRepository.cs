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

    public class OrderRepository :
        GenericRepository<DataModelContext, Order>, IOrderRepository
    {
        PageRepository pageRepo = new PageRepository();
        private readonly long START_ORDER_NUMBER=10000000;

        public Order GetSingle(Guid Id)
        {

            var query = Context.Orders.FirstOrDefault(x => x.Id == Id);
            return query;
        }

        public ResultDto validateOrder(Order orderModel)
        {
            ResultDto retResult = new ResultDto();
            retResult.isDone = true;
            checkPageId(orderModel, retResult);
            if (retResult.isDone)
                retResult.statusCode = enm_STATUS_CODE.DONE_SUCCESSFULLY;
            return retResult;
        }

        private void checkPageId(Order orderModel, ResultDto retResult)
        {
            var c = Context.Pages.Count(p => p.Id == orderModel.PageId);
            if (c == 0 || orderModel.PageId == null)
            {
                retResult.addValidationMessages("pageId is not valid or is null!");
            }
             c = Context.Pages.Count(p => p.Id == orderModel.CustomerPageId);
            if (c == 0 || orderModel.PageId == null)
            {
                retResult.addValidationMessages("CustomerPageId is not valid or is null!");
            }
        }

        public long getNextOrderNumber()
        {
            if (Context.Orders.Count() == 0) return START_ORDER_NUMBER;
           var maxOrderNumber= Context.Orders.Max(o => o.OrderNumber);
          
            if (maxOrderNumber == 0)
                maxOrderNumber = START_ORDER_NUMBER;
            else
                maxOrderNumber++;
            return (long)maxOrderNumber;
        }

    

        public IQueryable<OrderDto> queryByPageId(Guid pageid)
        {
            var orders = Context.Orders.Where(o => o.PageId == pageid).ToList();
            List<OrderDto> orderDtoList = Mapper<Order, OrderDto>.convertListToDto(orders);
            orderDtoList = addDetailToOrderDtoList(orderDtoList);
            return orderDtoList.AsQueryable();
        }

        private List<OrderDto> addDetailToOrderDtoList(List<OrderDto> orderDtoList)
        {
            List<OrderDto> retList = new List<OrderDto>();
            foreach (var item in orderDtoList)
            {
                retList.Add(addDetailToOrderDto(item));

            }
            return retList;
        }

        private OrderDto addDetailToOrderDto(OrderDto order)
        {
            PageRepository pageRepo = new PageRepository();
            order.pageDto = Mapper<Page, PageDto>.convertToDto(pageRepo.GetSingle(order.PageId));
            order.CustomerPageDto = Mapper<Page, PageDto>.convertToDto(pageRepo.GetSingle(order.CustomerPageId));
            List<OrderItem> items= Context.OrderItems.Where(oi => oi.OrderId == order.Id).ToList();
            List<OrderItemDto> itemsDto = new List<OrderItemDto>();
            foreach (var item in items)
            {
                OrderItemDto orderItemDto = Mapper<OrderItem, OrderItemDto>.convertToDto(item);
                orderItemDto.TicketDto = Mapper<Ticket, TicketDto>.convertToDto(item.Ticket);

                itemsDto.Add(orderItemDto);
            }
            order.OrderItemList = itemsDto;
            return order;
        }

        public IQueryable<OrderDto> queryByCustomerPageId(Guid pageid)
        {
            var orders = Context.Orders.Where(o => o.CustomerPageId  == pageid).ToList();
            List<OrderDto> orderDtoList = Mapper<Order, OrderDto>.convertListToDto(orders);
            orderDtoList = addDetailToOrderDtoList(orderDtoList);
            return orderDtoList.AsQueryable();
        }

       
        public Order getOrderByInvoiceId(Guid invoiceId)
        {
            var or= Context.Orders.Where(o => o.InvoiceId == invoiceId);
            if (or.Count() > 0) return or.First();
            else return null;
        }
    }
       
}