using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TigTag.DTO.ModelDTO
{
   public class InvoiceTransactionDto:BaseDto
    {
        public System.DateTime CreateDate { get; set; }
        public Nullable<int> StatusCode { get; set; }
        public string TransactionCode { get; set; }
        public string Comment { get; set; }
        public System.Guid InvoiceId { get; set; }
    }
}
