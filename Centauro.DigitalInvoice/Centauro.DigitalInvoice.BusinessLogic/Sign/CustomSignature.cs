using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Centauro.DigitalInvoice.BusinessLogic.Sign
{
    public class CustomSignature
    {
        public void BuildSign(XmlDocument xmlDoc)
        {
            SignedXml signedXml = new SignedXml(xmlDoc);



        }
    }
}
