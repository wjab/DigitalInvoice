using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Centauro.DigitalInvoice.BusinessLogic.XmlDsig.Common
{
    public class VerificationResults
    {
        public string Timestamp { get; set; }
        public XmlDocument OriginalDocument { get; set; }
        public X509Certificate2 SigningCertificate { get; set; }
    }
}
