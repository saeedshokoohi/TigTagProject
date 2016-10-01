using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TigTag.Common.util
{
    public class JsonUtil
    {
        public static Guid[] convertToGuidArray(string menuList)
        {
            Guid[] retList = null;
            retList= JsonConvert.DeserializeObject<Guid[]>(menuList);
            return retList;
        }
        public static List<Guid> convertToGuidList(string menuList)
        {
            Guid[] retList = null;
            retList = JsonConvert.DeserializeObject<Guid[]>(menuList);
            return retList.ToList();
        }
    }
}
