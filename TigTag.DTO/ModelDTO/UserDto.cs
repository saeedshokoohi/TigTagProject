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
        public string EmailAddress { get; set; }
        public string password { get; set; }
    }
}
