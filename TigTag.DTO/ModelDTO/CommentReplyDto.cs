using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TigTag.DTO.ModelDTO
{
    public class CommentReplyDto
        :BaseDto
    {
        public System.Guid AutherId { get; set; }
        public System.Guid PageCommentId { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string ReplyText { get; set; }
        
       public string AutherName { get; set; }
    }
}
