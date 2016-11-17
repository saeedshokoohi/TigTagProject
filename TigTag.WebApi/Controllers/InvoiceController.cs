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
using TigTag.WebApi.Controllers.Util;

namespace TigTag.WebApi.Controllers
{
    public class InvoiceController : BaseController<Invoice, InvoiceDto>
    {

        InvoiceRepository InvoiceRepo = new InvoiceRepository();
        InvoiceTransactionRepository InvoiceTransactionRepo = new InvoiceTransactionRepository();

        PageRepository pageRepo = new PageRepository();
        PaymentUtil paymentUtil = new PaymentUtil();

        public InvoiceController()
        {
            eventLogRepo.Context = InvoiceRepo.Context;
            InvoiceTransactionRepo.Context = InvoiceRepo.Context;
        }
        [HttpGet]
        public ResultDto startToPayInvoice([FromUri] Guid invoiceId)
        {
            ResultDto retResult = new ResultDto();
            Invoice invoice=InvoiceRepo.GetSingle(invoiceId);
            if (invoice == null)
                retResult.addValidationMessages("invoice id is not valid");
            else
            {
                if(invoice.IsPaid)
                {
                    retResult.addValidationMessages("this invoice is paid");
                }
                else
                {
                    if(invoice.Amount>0)
                    {
                        //to start paying
                        InvoiceDto invoiceDto= Mapper<Invoice, InvoiceDto>.convertToDto(invoice);
                        PaymentInfo payInfo=paymentUtil.sendPaymentRequest(invoiceDto);
                        retResult.paymentInfo=payInfo;
                        InvoiceTransactionRepo.addNewPaymentTransaction(invoice, payInfo);

                    }
                    else
                    {
                        retResult.addValidationMessages("this invoice amount is ZERO!");
                    }
                }
            }

            return retResult;
        }
        public override IGenericRepository<Invoice> getRepository()
        {
            return InvoiceRepo;
        }

        public ResultDto addNewInvoiceForOrder(Order orderModel)
        {
            ResultDto retResult = new ResultDto();
            try
            {
                Invoice newInvoice = new Invoice();
                newInvoice.Id = Guid.NewGuid();
                newInvoice.CreateDate = DateTime.Now;
                newInvoice.Description = "سفارش شماره " + orderModel.OrderNumber;
                newInvoice.Amount =int.Parse( orderModel.TotalPrice.ToString());
             //   newInvoice.CustomerName = orderModel.Page.PageTitle;
                InvoiceRepo.Add(newInvoice);
                InvoiceRepo.Save();
                retResult.returnId = newInvoice.Id.ToString();


            }catch(Exception ex)
            {
                retResult.isDone = false;
                retResult.message = ex.Message;

            }

            return retResult;

        }
    }
}