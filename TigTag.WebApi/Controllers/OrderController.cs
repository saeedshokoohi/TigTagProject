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
                        if (Order.ProfileId == Guid.Empty) Order.ProfileId = getCurrentProfileId();
                        eventLogRepo.AddOrderEvent(Order.ProfileId, OrderModel);
                        
                        if(returnResult.isDone)
                        {
                            //adding orderItems
                            OrderItemController orderItemController = new OrderItemController();
                            ResultDto ItemResult= orderItemController.addOrderItemList(OrderModel.Id, Order.OrderItemList);
                            if(!ItemResult.isDone)
                            {
                                //rollBack 
                                //removing order
                                OrderRepo.Delete(OrderModel);
                                OrderRepo.Save();
                                return ItemResult;
                            }
                            //add participant
                            ParticipantController participantController = new ParticipantController();
                            ParticipantDto participantDto = new ParticipantDto();
                            participantDto.PageId = OrderModel.PageId;
                            participantDto.ParticipantPageId = OrderModel.CustomerPageId;
                            participantDto.RequestStatus = enmFollowRequestStatus.APPROVED.GetHashCode();
                            
                            participantController.addParticipant(participantDto);
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
            return OrderRepo.queryByPageId(pageid);
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
            return OrderRepo.queryByCustomerPageId(pageid);
        }
    }
}