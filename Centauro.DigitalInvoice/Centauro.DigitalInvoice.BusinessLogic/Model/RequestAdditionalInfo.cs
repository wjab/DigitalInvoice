using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Centauro.DigitalInvoice.BusinessLogic.Model
{
    public class RequestAdditionalInfo
    {
        public RequestAdditionalInfo()
        {
            Emisor = new EnteEmisorReceptor();
            Receptor = new EnteEmisorReceptor();
        }

        public EnteEmisorReceptor Emisor { get; set; }

        public EnteEmisorReceptor Receptor { get; set; }

        public string ConsecutivoReceptor { get; set; }
    }

    public class EnteEmisorReceptor
    {
        public string tipoIdentificacion { get; set; }
        public string numeroIdentificacion { get; set; }
    }
}
