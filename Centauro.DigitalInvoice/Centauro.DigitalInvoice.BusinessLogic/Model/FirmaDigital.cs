using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Centauro.DigitalInvoice.BusinessLogic.Model
{
    public class FirmaDigital
    {
        public byte[] firma { get; set; }
        public byte[] x509Certificado { get; set; }
    }
}
