using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Centauro.DigitalInvoice.BusinessLogic.Constants
{
    public class Contants
    {
        public const string access_token = "access_token";
        public const string grant_type = "grant_type";
        public const string client_id = "client_id";
        public const string apiStag = "api-stag";
        public const string client_secret = "client_secret";
        public const string scope = "scope";
        public const string username = "username";
        public const string password = "password";
        public const string tokenEndpoint = "tokenEndpoint";
        public const string userATV = "userATV";
        public const string passwordATV = "passwordATV";

        public const string receptionEndpoint = "mhEndpointRecepcion";

        public const string application_json = "application/json";


        /* 
         * Hacienda
         */
        public static string RequestApiFormat = "{0}{1}";



        public const string XMLTest = @"<?xml version=""1.0""?>
                            <books>
	                            <book id=""bk01"">                                     
		                            <author>walter</author>
		                            <title>programacion</title>
		                            <price>10.30</price>
                                    <TipoDoc>01</TipoDoc>
                                </book>
	                            <book id=""bk02"">
		                            <author>roberto</author>
		                            <title>ciclismmo</title>
		                            <price>11.90</price>
                                    <TipoDoc>2</TipoDoc>  
	                            </book>
                            </books>";

        public const string bookSample = @"<xs:schema xmlns:xs=""http://www.w3.org/2001/XMLSchema"" elementFormDefault=""qualified"" attributeFormDefault=""unqualified"">
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
    }
}
