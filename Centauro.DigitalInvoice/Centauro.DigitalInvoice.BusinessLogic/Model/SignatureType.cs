using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Centauro.DigitalInvoice.BusinessLogic.Model
{
    public class SignatureType
    {
        public SignatureType()
        {
            SignedInfo = new SignedInfo();
            SignatureValue = new SignatureValue();
            KeyInfo = new KeyInfo();
            Object = new ObjectType();
        }

        public SignedInfo SignedInfo { get; set; }
        public SignatureValue SignatureValue { get; set; }
        public KeyInfo KeyInfo { get; set; }

        public ObjectType Object { get; set; }

        public string Id { get; set; }
    }

    public class SignedInfo
    {
        public SignedInfo()
        {
            KeyValue = new KeyValueType();
            SPKIData = new SPKIDataType();
        }

        public string KeyName { get; set; }
        public KeyValueType KeyValue { get; set; }
        public string RetrievalMethod { get; set; }
        public string X509Data { get; set; }
        public string PGPData { get; set; }
        public SPKIDataType SPKIData { get; set; }
        public string MgmtData { get; set; }
    }

    public class KeyValueType
    {
        public KeyValueType()
        {
            Manifest = new ManifestType();
        }

        public ManifestType Manifest { get; set; }
    }

    public class ManifestType
    {
        public ManifestType()
        {
            Reference = new ObjectType();
        }

        public ObjectType Reference { get; set; }
    }

    public class SPKIDataType
    {
        public string SPKISexp { get; set; }
    }

    public class SignatureValue
    {

    }

    public class KeyInfo
    {

    }

    public class ObjectType
    {
        public string Id { get; set; }
        public string MimeType { get; set; }
        public string Encoding { get; set; }
    }

}
