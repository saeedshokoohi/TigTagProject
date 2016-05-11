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
        public int statusCode { get; set; }
        public string message { get; set; }
        public string returnId { get; set; }
    }
}
