using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimationApplication.Entities
{
    public class ExstimationApplicationBusinessException : Exception
    {
        public ExstimationApplicationBusinessException()
        {
        }

        public ExstimationApplicationBusinessException(string message)
            : base(message)
        {
        }

        public ExstimationApplicationBusinessException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
