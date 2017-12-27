using Centauro.DigitalInvoice.BusinessLogic.Common;
using Centauro.DigitalInvoice.BusinessLogic.Common.Exceptions;
using Centauro.DigitalInvoice.BusinessLogic.Utilities;
using Centauro.DigitalInvoice.BusinessLogic.Utilities.Cryptography;
using Centauro.DigitalInvoice.BusinessLogic.Utilities.Xml;
using Centauro.DigitalInvoice.BusinessLogic.XmlDsig;
using Centauro.DigitalInvoice.BusinessLogic.XmlDsig.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Centauro.DigitalInvoice.BusinessLogic.Xades.Operations
{
    class XadesVerifyOperation
    {
        public static void Verify(VerificationParameters parameters)
        {
            var result = XmlDsigVerifyOperation.VerifyAndGetResults(parameters);
            VerifySigningCertificate(parameters, result.SigningCertificate);
        }

        private static void VerifySigningCertificate(VerificationParameters parameters, X509Certificate2 signingCertificate)
        {
            var certificateBase64 = signingCertificate.RawData;
            var calculatedHash = CryptoHelper.GetBytesSHA256(certificateBase64);

            var xmlDocument = new XmlDocument();
            xmlDocument.Load(parameters.InputPath);
            var signingCertificateNode = XmlHelper.FindNodesIn(xmlDocument.DocumentElement,
                "Signature/Object/QualifyingProperties/SignedProperties/" +
                "SignedSignatureProperties/SigningCertificate/Cert/CertDigest");
            var certificateHashNode = XmlHelper.FindNodesIn(signingCertificateNode[0],
                "DigestValue");
            var certificateHashInSignature = Convert.FromBase64String(certificateHashNode[0].InnerText);
            if (!ArrayHelper.ArraysAreEqual(certificateHashInSignature, calculatedHash))
            {
                throw new InvalidSignedDocumentException("SigningCertificate cannot be verified");
            }
        }

        public static VerificationResults VerifyAndGetResults(VerificationParameters parameters)
        {
            var result = XmlDsigVerifyOperation.VerifyAndGetResults(parameters);
            VerifySigningCertificate(parameters, result.SigningCertificate);
            return result;
        }
    }
}
