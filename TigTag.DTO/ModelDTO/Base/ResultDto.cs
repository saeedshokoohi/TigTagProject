using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TigTag.DTO.ModelDTO.RequestDto;

namespace TigTag.DTO.ModelDTO.Base
{
   public  class ResultDto
    {
        public bool isDone { get; set; }
        public enm_STATUS_CODE statusCode { get; set; }
        public string message { get; set; }
        public string returnId { get; set; }
        public List<string> validationMessages { get; set; }
        public ResultDto()
        {
            validationMessages = new List<string>();
            statusCode = enm_STATUS_CODE.NO_ACTION;

        }
        public void addValidationMessages(string message)
        {
            if(!String.IsNullOrEmpty(message))
            validationMessages.Add(message);
        }

        public static ResultDto failedResult(string msg)
        {
            ResultDto result = new ResultDto();
            result.isDone = false;
            result.statusCode = enm_STATUS_CODE.FAILED_WITH_ERROR;
            result.message = msg;
            return result;
        }
        public static ResultDto successResult(string returnid,string msg)
        {
            ResultDto result = new ResultDto();
            result.isDone = true;
            result.statusCode = enm_STATUS_CODE.DONE_SUCCESSFULLY;
            result.message = msg;
            result.returnId = returnid;
            return result;
        }

        public static ResultDto invalidResult(string validationMessage)
        {
            ResultDto result = new ResultDto();
            result.isDone = false;
            result.statusCode = enm_STATUS_CODE.INPUT_NOT_VALID;
            result.addValidationMessages(validationMessage);
            return result;
        }

        public static ResultDto exceptionResult(Exception ex)
        {
            ResultDto returnResult = new ResultDto();
            returnResult.isDone = false;
            returnResult.statusCode = enm_STATUS_CODE.FAILED_WITH_ERROR;
            returnResult.message = ex.Message;
            if (ex.InnerException != null)
            {
                returnResult.message += ex.InnerException.Message;
                if(ex.InnerException.InnerException!=null)
                    returnResult.message += ex.InnerException.InnerException.Message;
            }
            return returnResult;
        }

      
    }
    public enum enm_STATUS_CODE
    {
        NO_ACTION=100,
        DONE_SUCCESSFULLY=101,
        DONE_WITH_ERROR=102,
        INPUT_NOT_VALID=103,
        FAILED_WITH_ERROR =104,
        FAILED_UNKNOWN_ERROR=105

    }
}
