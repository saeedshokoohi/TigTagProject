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
    public class OrderController : BaseController<Order, OrderDto>
    {

        OrderRepository OrderRepo = new OrderRepository();
        InvoiceRepository invoiceRepo = new InvoiceRepository();
        MenuRepository menuRepo = new MenuRepository();
        PageRepository pageRepo = new PageRepository();

        public override IGenericRepository<Order> getRepository()
        {
            return OrderRepo;
        }
        public ResultDto addOrder(OrderDto Order)
        {
            if (Order == null) return ResultDto.failedResult("Invalid Raw Payload data, it must be an json object  ");
            else
            {
                ResultDto returnResult = new ResultDto();
                Order OrderModel = Mapper<Order, OrderDto>.convertToModel(Order);
                OrderModel.Id = Guid.NewGuid();

                returnResult = OrderRepo.validateOrder(OrderModel);
                if (returnResult.isDone)
                {
                    try
                    {
                        OrderModel.OrderNumber = OrderRepo.getNextOrderNumber();
                        OrderModel.OrderDate = DateTime.Now;
                        OrderRepo.Add(OrderModel);
                        OrderRepo.Save();
                        returnResult.isDone = true;
                        returnResult.message = "new Order created successfully";
                        returnResult.returnId = OrderModel.Id.ToString();
                      
                        
                        if(returnResult.isDone)
                        {
                            //adding orderItems
                            OrderItemController orderItemController = new OrderItemController();
                            double totalPrice = 0;
                            ResultDto ItemResult= orderItemController.addOrderItemList(OrderModel.Id, Order.OrderItemList,out totalPrice);
                            OrderModel.TotalPrice = totalPrice;
                            OrderRepo.Save();
                            if (!ItemResult.isDone)
                            {
                                //rollBack 
                                //removing order
                                OrderRepo.Delete(OrderModel);
                                OrderRepo.Save();
                                return ItemResult;
                            }

                            //add new Invoice for Order
                            if (OrderModel.TotalPrice > 0)
                            {
                                InvoiceController invoiceController = new InvoiceController();
                                ResultDto invoiceResult= invoiceController.addNewInvoiceForOrder(OrderModel);
                                if(invoiceResult.isDone)
                                {
                                    OrderModel.InvoiceId = Guid.Parse(invoiceResult.returnId);
                                    OrderRepo.Save();
                                }


                            }
                            else
                            {

                                //add participant
                                ParticipantController participantController = new ParticipantController();
                                ParticipantDto participantDto = new ParticipantDto();
                                participantDto.PageId = OrderModel.PageId;
                                participantDto.ParticipantPageId = OrderModel.CustomerPageId;
                                participantDto.RequestStatus = enmFollowRequestStatus.APPROVED.GetHashCode();

                                participantController.addParticipant(participantDto);

                                FollowController followController = new FollowController();
                                FollowDto followDto = new FollowDto();
                                followDto.FollowingPageId = OrderModel.PageId;
                                followDto.FollowerUserId = OrderModel.CustomerPageId;
                                followDto.RequestStatus = enmFollowRequestStatus.APPROVED.GetHashCode();

                                followController.followProfile(followDto);

                            }
                            if (Order.ProfileId == Guid.Empty) Order.ProfileId = getCurrentProfileId();
                            eventLogRepo.AddOrderEvent(Order.ProfileId, OrderModel);
                        }
                    }
                    catch (Exception ex)
                    {
                        returnResult = ResultDto.exceptionResult(ex);


                    }

                }

                return returnResult;
            }
        }

        public void confirmOrder(Guid invoiceId)
        {
            Order OrderModel = OrderRepo.getOrderByInvoiceId(invoiceId);
            if(OrderModel != null)
            {
                //add participant
                ParticipantController participantController = new ParticipantController();
                ParticipantDto participantDto = new ParticipantDto();
                participantDto.PageId = OrderModel.PageId;
                participantDto.ParticipantPageId = OrderModel.CustomerPageId;
                participantDto.RequestStatus = enmFollowRequestStatus.APPROVED.GetHashCode();

                participantController.addParticipant(participantDto);

                FollowController followController = new FollowController();
                FollowDto followDto = new FollowDto();
                followDto.FollowingPageId = OrderModel.PageId;
                followDto.FollowerUserId = OrderModel.CustomerPageId;
                followDto.RequestStatus = enmFollowRequestStatus.APPROVED.GetHashCode();

                followController.followProfile(followDto);
            }
        }

        [HttpGet]
        [EnableQueryAttribute]
        public IQueryable<OrderDto> queryByPageId(String pageId)
        {
            Guid pageid = Guid.NewGuid();
            try
            {
                pageid = Guid.Parse(pageId);
            }
            catch { return null; }
            var orders= OrderRepo.queryByPageId(pageid);
            foreach (var or in orders)
            {
                addInvoiceDetail(or);
            }
            return orders;
        }

        private void addInvoiceDetail(OrderDto or)
        {
            if (or.InvoiceId != null)
            {
                Invoice inv= invoiceRepo.GetSingle((Guid)or.InvoiceId);
                or.invoiceDto = Mapper<Invoice, InvoiceDto>.convertToDto(inv);
            }
        }

        [HttpGet]
        [EnableQueryAttribute]
        public IQueryable<OrderDto> queryByCustomerPageId(String customerPageId)
        {
            Guid pageid = Guid.NewGuid();
            try
            {
                pageid = Guid.Parse(customerPageId);
            }
            catch { return null; }
          
            var orders = OrderRepo.queryByCustomerPageId(pageid);
            foreach (var or in orders)
            {
                addInvoiceDetail(or);
            }
            return orders;
        }
    }
}