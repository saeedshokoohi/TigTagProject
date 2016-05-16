using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
