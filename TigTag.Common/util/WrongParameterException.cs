using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TigTag.Common.util
{
   public class WrongParameterException : Exception
    {
        public WrongParameterException(string message):base(message)
        {
          
        }

    }
}
