using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TigTag.DTO.ModelDTO
{
    public class UserDto:BaseDto
    {

        public string UserName { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string EmailAddress { get; set; }
        public Nullable<System.DateTime> LastLoginDate { get; set; }
        public string ProfileInfo { get; set; }
        public Nullable<System.Guid> ProfileImageId { get; set; }
        public Nullable<bool> Gender { get; set; }

    }
}
