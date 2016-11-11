﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TigTag.DTO
{
   public class BaseDto
    {
        public Guid Id { get; set; }
        public Guid ProfileId { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<System.Guid> ModifiedBy { get; set; }

    }
}
