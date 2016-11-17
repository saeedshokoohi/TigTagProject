using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TigTag.DTO.ModelDTO
{
   public  class PayRequestDto
    {
        public int amount { get; set; }
        public string customerEmail { get; set; }
        public string customerPhoneNumber { get; set; }
        public string description { get; set; }
    }
}
