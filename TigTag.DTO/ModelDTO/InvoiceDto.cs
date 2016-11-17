using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TigTag.DTO.ModelDTO
{
   public class InvoiceDto:BaseDto
    {

        public System.DateTime CreateDate { get; set; }
        public string Description { get; set; }
        public int Amount { get; set; }
        public bool IsPaid { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerEmailAddress { get; set; }
        public string Comment { get; set; }
    }
}
