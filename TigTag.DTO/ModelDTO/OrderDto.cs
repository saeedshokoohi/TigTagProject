using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TigTag.DTO.ModelDTO
{
    public class OrderDto:BaseDto
    {
        public System.Guid PageId { get; set; }
        public System.Guid CustomerPageId { get; set; }
        public long? OrderNumber { get; set; }
        public Nullable<System.DateTime> OrderDate { get; set; }
        public string Description { get; set; }
        public Nullable<double> TotalPrice { get; set; }
        public List<OrderItemDto> OrderItemList { get; set; }
        public PageDto pageDto { get; set; }
        public PageDto CustomerPageDto { get; set; }
    }
}
