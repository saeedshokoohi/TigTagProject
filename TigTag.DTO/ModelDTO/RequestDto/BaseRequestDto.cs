using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TigTag.Common.util;

namespace TigTag.DTO.ModelDTO.RequestDto
{
    public class BaseRequestDto
    {
        public Guid currentPageId { get; set; }
        public int maxResult { get; set; }
        public string searchExp { get; set; }
        public string parameters { get; set; }
        public BaseRequestDto()
        {
            maxResult = 100;

        }
        public static Guid extractParameterValueAsGuid(BaseRequestDto request, string paramName)
        {

            string value = extractParameterValue(request, paramName);
            Guid retId = new Guid();
            if( Guid.TryParse(value, out retId))
            {
                return retId;
            }else
            {
                throw new WrongParameterException("value of parameter '" + paramName + "'  must be in GUID format");
            }
           
        }
        public static String extractParameterValue(BaseRequestDto request, string paramName)
        {
            if (request != null && paramName != null)
            {
                var parameters = JObject.Parse(request.parameters);
                if (parameters[paramName] == null)
                {
                    throw new WrongParameterException("parameter with name:=> '" + paramName + "' must be in parameters ...");
                }
                else
                {
                    string value = parameters[paramName].ToString();
                    return value;
                }
            }
            else
            {
                throw new WrongParameterException("invalid json object for service request.");
            }
        }
    }
}
