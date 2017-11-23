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
        public IList<IError> ValidateXML(string xmlData)
        {
            IList<IError> errorList = new List<IError>();

            string errorHolder = string.Empty;
            ValidationHandler handler = new ValidationHandler();

            try
            {
                string xsd = @"C:\Users\Administrador\documents\visual studio 2015\Projects\Centauro.DigitalInvoice\Centauro.DigitalInvoice.BusinessLogic\XSD\Acepta_Rechaza_DocumentoXML.xsd";

                string xsd1 = @"<xs:schema xmlns:xs=""http://www.w3.org/2001/XMLSchema"" elementFormDefault=""qualified"" attributeFormDefault=""unqualified"">
	                                <xs:element name=""books"">
		                                <xs:complexType>
			                                <xs:sequence>
				                                <xs:element name=""book"" maxOccurs=""unbounded"" minOccurs=""0"">
					                                <xs:complexType>
						                                <xs:sequence>
							                                <xs:element type=""xs:string"" name=""author"" />
							                                <xs:element type=""xs:string"" name=""title"" />
							                                <xs:element type=""xs:float"" name=""price"" />
							                                <xs:element name=""TipoDoc"" nillable=""false"">
								                                <xs:annotation>
									                                <xs:documentation>Tipo de Documento ElectrÃ³nico: 01 Factura, 02 Nota de DÃ©bito, 03 Nota de CrÃ©dito</xs:documentation>
								                                </xs:annotation>
								                                <xs:simpleType>
									                                <xs:restriction base=""xs:int"">
										                                <xs:totalDigits value=""2""/>
										                                <xs:enumeration value=""01""/>
										                                <xs:enumeration value=""02""/>
										                                <xs:enumeration value=""03""/>
									                                </xs:restriction>
								                                </xs:simpleType>
							                                </xs:element>
						                                </xs:sequence>
						                                <xs:attribute type=""xs:string"" name=""id"" use=""optional""/>
					                                </xs:complexType>
				                                </xs:element>
			                                </xs:sequence>
		                                </xs:complexType>
	                                </xs:element>
                                </xs:schema>";

                XmlReader xmlReaderXSD = XmlReader.Create(new StringReader(xsd1));

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ValidationType = ValidationType.Schema;
                settings.Schemas.Add(null, xmlReaderXSD);//XmlReader.Create(xsd)

                XmlReader xmlReader = XmlReader.Create(new StringReader(xmlData), settings);
                settings.ValidationEventHandler += new ValidationEventHandler(handler.HandleValidationError);
                using (XmlReader validatingReader = XmlReader.Create(new StringReader(xmlData), settings))
                {
                    while (validatingReader.Read()) { }
                }

                errorList = handler.ValidationErrors;
            }
            catch (Exception ex)
            {            }
            
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
