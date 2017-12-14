using Centauro.DigitalInvoice.BusinessLogic.Enums;
using Centauro.DigitalInvoice.BusinessLogic.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;

namespace Centauro.DigitalInvoice.BusinessLogic
{
    public class DataValidator
    {
        public IList<IError> ValidateXML(string xmlData, xsdDocument xsdDocumentType)
        {
            IList<IError> errorList = new List<IError>();
            string xsd = string.Empty;

            string errorHolder = string.Empty;
            ValidationHandler handler = new ValidationHandler();

            try
            {
                switch (xsdDocumentType)
                {
                    case xsdDocument.Sample:
                        {
                            xsd = @"C:\github\DigitalInvoice\Centauro.DigitalInvoice\Centauro.DigitalInvoice.BusinessLogic\XSD\sample.xsd";
                        }break;
                    case xsdDocument.AceptaRechaza:
                        {
                            xsd = @"C:\github\DigitalInvoice\Centauro.DigitalInvoice\Centauro.DigitalInvoice.BusinessLogic\XSD\Acepta_Rechaza_DocumentoXML.xsd";
                        }
                        break;
                    default:
                        {

                        }break;
                }

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ValidationType = ValidationType.Schema;
                settings.Schemas.Add(null, XmlReader.Create(xsd));

                XmlReader xmlReader = XmlReader.Create(new StringReader(xmlData), settings);
                settings.ValidationEventHandler += new ValidationEventHandler(handler.HandleValidationError);
                using (XmlReader validatingReader = XmlReader.Create(new StringReader(xmlData), settings))
                {
                    while (validatingReader.Read()) { }
                }

                errorList = handler.ValidationErrors;
            }
            catch (Exception ex)
            {
                errorList.Add(new ValidationError() { Error = ex.Message });
            }
            
            return errorList;
        }

        public class ValidationHandler
        {
            private IList<IError> validationErrorList = new List<IError>();
            public IList<IError> ValidationErrors { get { return validationErrorList; } }

            public void HandleValidationError(object sender, System.Xml.Schema.ValidationEventArgs ve)
            {   
                if (ve.Severity == XmlSeverityType.Error || ve.Severity == XmlSeverityType.Warning)
                {
                    validationErrorList.Add(new ValidationError() { Error = ve.Exception.Message });
                }
            }
    }
    }
}
