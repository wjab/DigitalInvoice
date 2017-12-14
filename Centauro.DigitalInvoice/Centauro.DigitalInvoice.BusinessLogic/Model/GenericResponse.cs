using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Centauro.DigitalInvoice.BusinessLogic.Model
{
    public class GenericResponse
    {
        public HttpStatusCode status { get; set; }
        public object input { get; set; }
        public string message { get; set; }
        public IList<IError> errorList { get; set; }
        public object results { get; set; }
    }
}
