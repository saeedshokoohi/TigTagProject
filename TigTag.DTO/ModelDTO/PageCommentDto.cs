using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TigTag.DTO.ModelDTO
{
    public class PageCommentDto
        :BaseDto
    {
        public System.Guid AutherId { get; set; }
        public System.Guid PageId { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CommentText { get; set; }
        public int repliesCount { get; set; }
        public CommentReplyDto lastCommentReply { get; set; }
        public Nullable<bool> IsPublic { get; set; }
    }
}
