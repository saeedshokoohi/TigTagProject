using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

using TigTag.DTO.ModelDTO;
using TigTag.DTO.ModelDTO.Base;
using TigTag.WebApi.Controllers.Util;

namespace TigTag.WebApi.Controllers
{
    public class PayRequestController : ApiController
    {


        public ResultDto sendRequest(InvoiceDto payrequest )
        {
            ResultDto retResult = new ResultDto();
            PaymentUtil paymentUtil = new PaymentUtil();
            paymentUtil.sendPaymentRequest(payrequest);

            return retResult;

        }
       
        [System.Web.Http.HttpGet]
        public void getResponse()
        {

            string str = this.Request.ToString();

        }
       
    }
}