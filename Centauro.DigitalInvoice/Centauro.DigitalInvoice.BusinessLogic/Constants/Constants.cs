using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Centauro.DigitalInvoice.BusinessLogic.Constants
{
    public class Constants
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
        public const string certificatePIN = "certificatePIN";
        public const string serverDomain = "serverDomain";



        public const string mhEndpoint = "mhEndpoint";
        public const string mhActionRecepcion = "mhActionRecepcion";
        public const string mhActionComprobantes = "mhActionComprobantes";



        public const string SignIdentifier = "SignIdentifier";
        public const string CallBackUrl = "CallBackUrl";



        public const string application_json = "application/json";


        /* 
         * Hacienda
         */
        public const string RequestApiFormat_2 = "{0}{1}";
        public const string RequestApiFormat_3 = "{0}{1}{2}";
        public const string auth_header_bearer = "bearer";
        public const string encoding_UTF_8 = "utf-8";

        public const string miliseconds = "miliseconds";
        public const string tokenExpiration = "tokenExpiration";


        #region Headers de respuesta de Hacienda

        public const string header_groupId = "Jmsxgroupid";
        public const string header_errorCause = "X-Error-Cause";
        public const string header_rateLimit = "X-Ratelimit-Limit";
        public const string header_reteRemaining = "X-Ratelimit-Remaining";
        public const string header_rateReset = "X-Ratelimit-Reset";
        public const string header_location = "Location";
        

        #endregion


        #region XML-TEST
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
        #endregion


        #region Book-Sample
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

        #endregion



        public const string myTestXML = @"<facturaelectronicaxml><xs:schema xmlns=""https://tribunet.hacienda.go.cr/docs/esquemas/2017/v4.2/facturaElectronica"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"" xmlns:vc=""http://www.w3.org/2007/XMLSchema-versioning"" xmlns:ds=""http://www.w3.org/2000/09/xmldsig#"" targetNamespace=""https://tribunet.hacienda.go.cr/docs/esquemas/2017/v4.2/facturaElectronica"" elementFormDefault=""qualified"" attributeFormDefault=""unqualified"" version=""4.2"" vc:minVersion=""1.1""><CodigoType> <Tipo /> <Codigo /> </CodigoType> <EmisorType> <Nombre></Nombre> <Identificacion> <Tipo /> <Numero /> </Identificacion> <NombreComercial /> <Ubicacion> <Provincia /> <Canton /> <Distrito /> <Barrio /> <OtrasSenas /> </Ubicacion> <Telefono> <CodigoPais /> <NumTelefono /> </Telefono> <Fax> <CodigoPais /> <NumTelefono /> </Fax> <CorreoElectronico /> </EmisorType> <ExoneracionType> <TipoDocumento /> <NumeroDocumento /> <NombreInstitucion></NombreInstitucion> <FechaEmision>0001-01-01T00:00:00</FechaEmision> <MontoImpuesto>0</MontoImpuesto> <PorcentajeCompra>0</PorcentajeCompra> </ExoneracionType> <IdentificacionType> <Tipo></Tipo> <Numero /> </IdentificacionType> <ImpuestoType> <Codigo></Codigo> <Tarifa>0</Tarifa> <Monto>0</Monto> <Exoneracion> <TipoDocumento /> <NumeroDocumento /> <NombreInstitucion /> <FechaEmision>0001-01-01T00:00:00</FechaEmision> <MontoImpuesto>0</MontoImpuesto> <PorcentajeCompra>0</PorcentajeCompra> </Exoneracion> </ImpuestoType> <ReceptorType> <Nombre></Nombre> <Identificacion> <Tipo /> <Numero /> </Identificacion> <IdentificacionExtranjero /> <NombreComercial /> <Ubicacion> <Provincia /> <Canton /> <Distrito /> <Barrio /> <OtrasSenas /> </Ubicacion> <Telefono> <CodigoPais /> <NumTelefono /> </Telefono> <Fax> <CodigoPais /> <NumTelefono /> </Fax> <CorreoElectronico /> </ReceptorType> <TelefonoType> <CodigoPais></CodigoPais> <NumTelefono /> </TelefonoType> <UbicacionType> <Provincia></Provincia> <Canton /> <Distrito /> <Barrio /> <OtrasSenas /> </UbicacionType> <ClaveType></ClaveType> <DecimalDineroType>0</DecimalDineroType> <NumeroConsecutivoType></NumeroConsecutivoType> <UnidadMedidaType></UnidadMedidaType> <FacturaElectronica> <Clave></Clave> <NumeroConsecutivo></NumeroConsecutivo> <FechaEmision>2017-12-26T16:54:35.9994301-06:00</FechaEmision> <Emisor></Emisor> <Receptor></Receptor> <CondicionVenta></CondicionVenta> <PlazoCredito></PlazoCredito> <MedioPago></MedioPago> <ResumenFactura> <CodigoMoneda /> <TipoCambio /> <TotalServGravados /> <TotalServExentos /> <TotalMercanciasGravadas /> <TotalMercanciasExentas /> <TotalGravado /> <TotalExento /> <TotalVenta /> <TotalDescuentos /> <TotalVentaNeta /> <TotalImpuesto /> <TotalComprobante /> </ResumenFactura> <InformacionReferencia> <TipoDoc /> <Numero /> <FechaEmision>0001-01-01T00:00:00</FechaEmision> <Codigo /> <Razon /> </InformacionReferencia> <Normativa> <NumeroResolucion /> <FechaResolucion /> </Normativa> <Otros /> <ds:Signature Id=""id-e34ffbff277e8d1432e864436aa11882"" xmlns:ds=""http://www.w3.org/2000/09/xmldsig#""> <ds:SignedInfo> <ds:CanonicalizationMethod Algorithm=""http://www.w3.org/2001/10/xml-exc-c14n#""/> <ds:SignatureMethod Algorithm=""http://www.w3.org/2001/04/xmldsig-more#rsa-sha256""/> <ds:Reference Id=""r-id-1"" Type="" URI=""> <ds:Transforms> <ds:Transform Algorithm=""http://www.w3.org/TR/1999/REC-xpath-19991116""> <ds:XPath>not(ancestor-or-self::ds:Signature)</ds:XPath> </ds:Transform> <ds:Transform Algorithm=""http://www.w3.org/2001/10/xml-exc-c14n#""/> </ds:Transforms> <ds:DigestMethod Algorithm=""http://www.w3.org/2001/04/xmlenc#sha256""/> <ds:DigestValue>ql0urtXTsc9W0GMIhTdzYHXnQYfnieoIttOBn9fGw7A=</ds:DigestValue> </ds:Reference> <ds:Reference Type=""http://uri.etsi.org/01903#SignedProperties"" URI=""#xades-ide34ffbff277e8d1432e864436aa11882""> <ds:Transforms> <ds:Transform Algorithm=""http://www.w3.org/2001/10/xml-exc-c14n#""/> </ds:Transforms> <ds:DigestMethod Algorithm=""http://www.w3.org/2001/04/xmlenc#sha256""/> <ds:DigestValue>5JVZPTwN5Lj0sGTfFzaUeMKCo/xbCAj7fw6TLUFtZIk=</ds:DigestValue> </ds:Reference> </ds:SignedInfo> <ds:SignatureValue Id=""value-ide34ffbff277e8d1432e864436aa11882"">Mt1TUuPK3W8/0eRtJX5t45GV9bHvMjwL2I5GKkz9FEExrcqLDY5SiR5ncrhZckJVx0n0FvY4n1ZBv guyEWHqZFoFsAjK8hFfJCfVOQCfrjh3E9L7wfUTZz1aLUFV8AVX07c6e7vSqHE2utta3Qz9dFT+sGX4NXHLggMYcE+jv/kqM9/+MQzppmmDx2VAEmGlR2haLPqyjUJ1dEGcRfZ8uUG+Lgq7ptSdzeziGhfaATBV/8+dIq+u4Gw3LQu8nB5LlTS/ZJEszKq+JyCosp9Er6aHVac6iVF6QEO6QQ4kITo pu/BZrWKna09cZHJ5pcifwhb/G/LYThHgbwnbRYqrlQ==</ds:SignatureValue> <ds:KeyInfo> <ds:X509Data> <ds:X509Certificate>MIIFpTCCBI2gAwIBAgIKK+xhaAABAARyvDANBgkqhkiG9w0BAQUFADCBmjEVMBMGA1UEBRMMNC0wMDAtMDA0MDE3MQs wCQYDVQQGEwJDUjEkMCIGA1UEChMbQkFOQ08gQ0VOVFJBTCBERSBDT1NUQSBSSUNBMSowKAYDVQQLEyFESVZJU0lPTiBERSBTRVJWSUNJT1MgRk lOQU5DSUVST1MxIjAgBgNVBAMTGUNBIFNJTlBFIC0gUEVSU09OQSBGSVNJQ0EwHhcNMTYwMjEyMTk0MTI2WhcNMTgwMjExMTk0MTI2WjCBszEZM BcGA1UEBRMQQ1BGLTAxLTEzMDItMDQ4NzEVMBMGA1UEBBMMTU9OR0UgQ0hBVkVTMRcwFQYDVQQqEw5NQVJWSU4gVklOSUNJTzELMAkGA1UEBhMCQ1IxFzAVBgNVBAoTDlBFUlNPTkEgRklTSUNBMRIwEAYDVQQLEwlDSVVEQURBTk8xLDAqBgNVBAMTI01BUlZJTiBWSU5JQ0lPIE1PTkdFIENIQVZFUyAoRklSTUEpMIIBIjANBgkqhkiG9w0BAQE34ghAQ8AMIIBCgKCAQEAtMLNAbhh2TzTB+Q6TBgLHCQGtn73eyOMH2ESIuam9jrnx50gOxM13QD8vXw3fyiyFxvbkxmrVgmihsQ3DC3sVo7S+zwtgp40RiDhEEgSqg8JaZcU+BPiLTNmO0J9lKU9nvYrOCam02oxVNiBUxR8jq7QZ2yG4QREgCQ68/ yXOdPLWAJf6afdxn2CiV1Tkeq11J+UDlPdWj65LG/JSQv1vJj0fGw3el/C+WDbIkY/g1ksZ44AdIJwSlhmgwrjOU7Z+bibbYdkN5Ky62bPvFk0irI6/oWV9K8mBgzLla1Zyj7CI2k5hi6vQF0doDSE0js5dFTIwCUDIsHvo4WwFZegoQIDAQABo4IB0DCCAcwwHQYDVR0OBBYEFEy03328b5QRrhdnwoLwjpwSNkfYMB8GA1UdIwQYMBaAFEjUipShoDKIP6qxNhCUK+6UQYKsMFwGA1UdHwRVMFMwUaBPoE2GS2h0dHA6Ly9mZGkuc2lucGUuZmkuY3IvcmVwb3NpdG9yaW8vQ0ElMjBTSU5QRSUyMC0lMjBQRVJTT05BJTIwRklTSUNBKDEpLmNybDCBkwYIKwYBBQUHAQEEgYYwgYMwKAYIKwYBBQUHMAGGHGh0dHA6Ly9vY3NwLnNpbnBlLmZpLmNyL29jc3AwVwYIKwYBBQUHMAKGS2h0dHA6Ly9mZGkuc2lucGUuZmkuY3IvcmVwb3NpdG9yaW8vQ0ElMjBTSU5QRSUyMC0lMjBQRVJTT05BJTIwRklTSUNBKDEpLmNydDAOBgNVHQ8BAf8EBAMCBsAwPQYJKAGCNxUHBDAwLgYmKwYBBAGCNxUIhcTqW4LR 4zWVkRuC+ZcYhqXLa4F/gb3yRYe7oj4CAWQCAQQwEwYDVR0lBAwwCgYIKwYBBQUHAwQwFQYDVR0gBA4wDDAKBghggTwBAQEBAjAbBgkrBgEEAYI3FQoEDjAMMAoGCCsGAQUFBwMEMA0GCSqGSIb3DQEBBQUAA4IBAQBnHXQC+D4U7DdZuq0iaqOs4NTqrLfTkYGIo6adAnUWWpN/XjC4y/4EUjRSbhQc/l2pRAlYG4pFJ1ftVvwSZMlcukHR/i6uLo1mWCE2SwWpKyw+5b58eHlJkomzExT92chv1gicHJdb7ZazXKa/0Bkf3+Olh9Nruqo2qs6uCetScCyqc+NTEwSTded4x+wbYIecElkxHje6pqr367zF0rL3khHJYPYCHsadOx0w9XR3K+yFsXksasZzBrsMzZ6/m3+23E0OC2oTb24/z3cn5dNm2y4H m/fxYl/CE3cQf5NOn7yt80Kdu+jaKDqIkGdSkuf6QppyoxkhuKK1x27fg</ds:X509Certificate> </ds:X509Data> </ds:KeyInfo> <ds:Object> <xades:QualifyingProperties xmlns:xades=""http://uri.etsi.org/01903/v1.3.2#"" Target=""#ide34ffbff277e8d1432e864436aa11882""> <xades:SignedProperties Id=""xades-id-e34ffbff277e8d1432e864436aa11882""> <xades:SignedSignatureProperties> <xades:SigningTime>2016-11-25T16:35:06Z</xades:SigningTime> <xades:SigningCertificate> <xades:Cert> <xades:CertDigest> <ds:DigestMethod Algorithm=""http://www.w3.org/2000/09/xmldsig#sha1""/> <ds:DigestValue>LoXZC86JwDL7zWC35qj7Q4AzrRQ=</ds:DigestValue> </xades:CertDigest> <xades:IssuerSerial> <ds:X509IssuerName>CN=CA SINPE - PERSONA FISICA,OU=DIVISION DE SERVICIOS FINANCIEROS,O=BANCO CENTRAL DE COSTA RICA,C=CR,2.5.4.5=#130c342d3030302d303034303137</ds:X509IssuerName> <ds:X509SerialNumber>207422209224813750547132</ds:X509SerialNumber> </xades:IssuerSerial> </xades:Cert> </xades:SigningCertificate> <xades:SignaturePolicyIdentifier> <xades:SignaturePolicyId> <xades:SigPolicyId> <xades:Identifier>https://tribunet.hacienda.go.cr/docs/esquemas/2016/v4.1/Resolucion_Comprobantes_Electronicos_DGT-R-48-2016.pdf</xades:Identifier> </xades:SigPolicyId> <xades:SigPolicyHash> <ds:DigestMethod Algorithm=""http://www.w3.org/2001/04/xmlenc#sha256""/> <ds:DigestValue>NmI5Njk1ZThkNzI0MmIzMGJmZDAyNDc4YjUwNzkzODM2NTBiOWUxNTBkMmI2YjgzYzZjM2I5NTZlNDQ4OWQzMQ==</ds:DigestValue> </xades:SigPolicyHash> </xades:SignaturePolicyId> </xades:SignaturePolicyIdentifier> </xades:SignedSignatureProperties> <xades:SignedDataObjectProperties> <xades:DataObjectFormat ObjectReference=""#r-id-1""> <xades:MimeType>application/octet-stream</xades:MimeType> </xades:DataObjectFormat> </xades:SignedDataObjectProperties> </xades:SignedProperties> </xades:QualifyingProperties> </ds:Object> </ds:Signature> </FacturaElectronica> </facturaelectronicaxml>";

        #region Rutas XSD y templates

        public const string signTemplate = "signTemplate";
        public const string invoiceTemplate = "InvoiceTemplate";
        public const string invoiceTemplateCss = "InvoiceTemplateCss";

        public const string sampleXSD = "sampleXSD";
        public const string aceptaRechazaXSD = "aceptaRechazaXSD";
        public const string facturaElectronicaXSD = "facturaElectronicaXSD";
        public const string resumentPeriodoComprasXSD = "resumentPeriodoComprasXSD";
        public const string resumentPeriodoCompraVentasXSD = "resumentPeriodoCompraVentasXSD";
        public const string resumenPeriodoVentasXSD = "resumenPeriodoVentasXSD";

        #endregion

        #region Html 

        public const string lineaDetalle = @"<td id=""item"">{0}</td> <td class=""desc""><h3>{1}</h3></td> <td class=""unit"">{2}</td> <td class=""qty"">{3}</td> <td id=""item"">{4}</td>";

        #endregion


    }
}
