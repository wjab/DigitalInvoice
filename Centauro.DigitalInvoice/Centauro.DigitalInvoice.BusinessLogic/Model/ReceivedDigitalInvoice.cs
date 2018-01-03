using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Centauro.DigitalInvoice.BusinessLogic.Model
{
    public class ReceivedDigitalInvoice
    {
        public string fileLocation { get; set; }
        public string reasonPhrase { get; set; }
        public int statusCode { get; set; }

        public string x_Error_Cause { get; set; }

        public string pdfInvoice { get; set; }
        public string xmlInvoice { get; set; }
    }
}
