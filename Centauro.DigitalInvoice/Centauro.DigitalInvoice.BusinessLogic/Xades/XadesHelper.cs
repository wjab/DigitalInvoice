using Centauro.DigitalInvoice.BusinessLogic.Xades.Dsl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Centauro.DigitalInvoice.BusinessLogic.Xades
{
    public class XadesHelper
    {
        public static XadesSignDsl Sign(string inputPath)
        {
            var signDsl = new XadesSignDsl();
            signDsl.InputPath(inputPath);
            return signDsl;
        }
        public static XadesSignDsl Sign(XmlDocument xmlDocument)
        {
            var signDsl = new XadesSignDsl();
            signDsl.InputXml(xmlDocument);
            return signDsl;
        }

        public static XadesVerifyDsl Verify(string signaturePath)
        {
            var verifyDsl = new XadesVerifyDsl();
            verifyDsl.SignaturePath(signaturePath);
            return verifyDsl;
        }
    }
}
