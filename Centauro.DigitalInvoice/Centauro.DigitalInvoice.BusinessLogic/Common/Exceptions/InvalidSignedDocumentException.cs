using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Centauro.DigitalInvoice.BusinessLogic.Common.Exceptions
{
    public class InvalidSignedDocumentException : Exception
    {
        public InvalidSignedDocumentException(string message) : base(message)
        {
        }
    }
}
