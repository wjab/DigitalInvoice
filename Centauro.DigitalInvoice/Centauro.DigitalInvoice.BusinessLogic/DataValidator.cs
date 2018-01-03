using Centauro.DigitalInvoice.BusinessLogic.Enums;
using Centauro.DigitalInvoice.BusinessLogic.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
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
        private static DataValidator DataValidatorInstance;
        private static ConcurrentDictionary<string, XmlReaderSettings> xsdDictionary;
        private static object syncLockoAuthData = new object();

        public static DataValidator Instance()
        {
            if(DataValidatorInstance == null)
            {
                DataValidatorInstance = new DataValidator();
            }

            return DataValidatorInstance;
        }

        public DataValidator()
        {
            
            xsdDictionary = new ConcurrentDictionary<string, XmlReaderSettings>();

            #region Load XSD's            

            xsdDictionary.GetOrAdd(xsdDocument.Sample.ToString(), LoadXSD(string.Format(Constants.Constants.RequestApiFormat_2, AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings[Constants.Constants.sampleXSD])));

            xsdDictionary.GetOrAdd(xsdDocument.AceptaRechaza.ToString(), LoadXSD(string.Format(Constants.Constants.RequestApiFormat_2, AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings[Constants.Constants.aceptaRechazaXSD])));
            xsdDictionary.GetOrAdd(xsdDocument.FacturaElectronica.ToString(), LoadXSD(string.Format(Constants.Constants.RequestApiFormat_2, AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings[Constants.Constants.facturaElectronicaXSD])));
            //xsdDictionary.GetOrAdd(xsdDocument.ResumenPeriodoCompras.ToString(), LoadXSD(string.Format(Constants.Constants.RequestApiFormat_2, AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings[Constants.Constants.resumentPeriodoComprasXSD])));
            //xsdDictionary.GetOrAdd(xsdDocument.ResumenPeriodoComprasVentas.ToString(), LoadXSD(string.Format(Constants.Constants.RequestApiFormat_2, AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings[Constants.Constants.resumentPeriodoCompraVentasXSD])));
            //xsdDictionary.GetOrAdd(xsdDocument.ResumenPeriodoVentas.ToString(), LoadXSD(string.Format(Constants.Constants.RequestApiFormat_2, AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings[Constants.Constants.resumenPeriodoVentasXSD])));

            #endregion
        }

        public XmlReaderSettings LoadXSD(string path)
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.Schemas.Add(null, XmlReader.Create(path));

            return settings;
        }


        public IList<IError> ValidateXML(string xmlData, xsdDocument xsdDocumentType)
        {
            IList<IError> errorList = new List<IError>();
            

            string errorHolder = string.Empty;
            ValidationHandler handler = new ValidationHandler();

            try
            {
                /*string xsd = GetXSD(xsdDocumentType);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ValidationType = ValidationType.Schema;
                settings.Schemas.Add(null, XmlReader.Create(xsd));*/

                XmlReaderSettings settings = GetXSD(xsdDocumentType);

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
                errorList.Add(new ValidationError() { errorMessage = ex.Message });
            }
            
            return errorList;
        }

        private XmlReaderSettings GetXSD(xsdDocument xsdDocumentType)
        {
            XmlReaderSettings xsd = null;
            bool resultget;

            lock (syncLockoAuthData)
            {
                resultget = xsdDictionary.TryGetValue(xsdDocumentType.ToString(), out xsd);
            }

            return xsd;
        }

        public class ValidationHandler
        {
            private IList<IError> validationErrorList = new List<IError>();
            public IList<IError> ValidationErrors { get { return validationErrorList; } }

            public void HandleValidationError(object sender, System.Xml.Schema.ValidationEventArgs ve)
            {   
                if (ve.Severity == XmlSeverityType.Error || ve.Severity == XmlSeverityType.Warning)
                {
                    validationErrorList.Add(new ValidationError() { errorMessage = ve.Exception.Message });
                }
            }
    }
    }
}
