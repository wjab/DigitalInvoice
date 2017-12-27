using Centauro.DigitalInvoice.BusinessLogic.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Centauro.DigitalInvoice.BusinessLogic.Operations.Signers
{
    internal class XmlDsigDetachedSignOperation : XmlDsigSignOperation
    {
        protected override void CreateAndAddReferenceTo(SignedXml signedXml, XmlDocument document, string inputPath, string xpathToNodeToSign)
        {
            if (signedXml == null)
            {
                throw new InvalidParameterException("Signed Xml cannot be null");
            }
            if (inputPath == null)
            {
                throw new InvalidParameterException("Input path cannot be null");
            }

            var reference = new Reference { Uri = "file://" + inputPath.Replace("\\", "/") };
            signedXml.AddReference(reference);
        }
    }
}
