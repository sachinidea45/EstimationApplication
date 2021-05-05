using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimationApplication.Entities
{
    public class ExstimationApplicationDataException : Exception
    {
        public ExstimationApplicationDataException()
        {
        }

        public ExstimationApplicationDataException(string message)
            : base(message)
        {

        }

        public ExstimationApplicationDataException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
