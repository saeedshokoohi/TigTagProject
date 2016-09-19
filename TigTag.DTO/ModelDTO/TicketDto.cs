using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TigTag.DTO.ModelDTO
{
   public class TicketDto:BaseDto
    {
        public string Title { get; set; }
        public System.Guid PageId { get; set; }
        public string Description { get; set; }
        public Nullable<int> Capacity { get; set; }
        public Nullable<double> Price { get; set; }
        public Nullable<System.DateTime> FromDate { get; set; }
        public Nullable<System.DateTime> ToDate { get; set; }
        public Nullable<System.DateTime> DeadlineDate { get; set; }
        public string Address { get; set; }
        public string Location { get; set; }
        public Nullable<int> SoldCapacity { get; set; }
    }
}
