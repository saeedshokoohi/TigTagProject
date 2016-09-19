using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TigTag.DTO.ModelDTO
{
   public class OrderItemDto:BaseDto
    {
        public System.Guid Id { get; set; }
        public Nullable<System.Guid> AttachmentId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public System.Guid TicketId { get; set; }
        public System.Guid OrderId { get; set; }
        public string Description { get; set; }
        public TicketDto TicketDto { get; set; }
    }
}
