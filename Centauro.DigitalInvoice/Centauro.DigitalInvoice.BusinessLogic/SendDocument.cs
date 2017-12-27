using Centauro.DigitalInvoice.BusinessLogic.Constants;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Centauro.DigitalInvoice.BusinessLogic.Model;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using Centauro.DigitalInvoice.BusinessLogic.Xades;
using System.Xml;
using System.Xml.Linq;
using Centauro.DigitalInvoice.BusinessLogic.Auth;
using System.Net.Http.Headers;

namespace Centauro.DigitalInvoice.BusinessLogic
{
    public class SendDocument
    {
        public async Task<string> SendAceptDenyDocument(AceptaRechazaDocumento payload, string accountId)
        {
            string respuesta = string.Empty;
            HttpResponseMessage responseMessage;

            /*var request = (HttpWebRequest)WebRequest.Create("https://www.verisign.com/");
            var response = (HttpWebResponse)request.GetResponse();
            response.Close();

            X509Certificate cert = request.ServicePoint.Certificate;
            var x509 = new X509Certificate2(cert);

            File.WriteAllBytes("custom.cert", cert.Export());

            Console.WriteLine(x509.Issuer);*/
            

            string certificatePath = @"C:\github\DigitalInvoice\Centauro.DigitalInvoice\Centauro.DigitalInvoice.BusinessLogic\Certificate\310156726431.p12";

            try
            {
                X509Certificate2 cert = new X509Certificate2();

                byte[] rawCertificateData = File.ReadAllBytes(certificatePath);
                cert.Import(rawCertificateData, ConfigurationManager.AppSettings[Constants.Constants.certificatePIN], X509KeyStorageFlags.PersistKeySet);

                payload.FirmaDigital.x509Certificado = Convert.ToBase64String(Encoding.ASCII.GetBytes(cert.ToString(true)));
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            
            payload.FirmaDigital.firma = Convert.ToBase64String(Encoding.ASCII.GetBytes("hola"));

            using (HttpClient client = new HttpClient())
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, ConfigurationManager.AppSettings[Constants.Constants.receptionEndpoint]);
                request.Content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, Constants.Constants.application_json); ;

                responseMessage = await client.SendAsync(request);
                return await responseMessage.Content.ReadAsStringAsync();

            }
        }

        public async Task<string> SendElectronicInvoice(facturaelectronicaxml payload, string accountId)
        {
            string respuesta = string.Empty;
            string ccc;
            HttpResponseMessage responseMessage = null;

            string certificatePath = @"C:\github\DigitalInvoice\Centauro.DigitalInvoice\Centauro.DigitalInvoice.BusinessLogic\Certificate\310156726431.p12";

            try
            {
                string psw = ConfigurationManager.AppSettings[Constants.Constants.certificatePIN];
                X509Certificate2 cert = new X509Certificate2(certificatePath, psw);

                var o = new
                {
                    facturaelectronicaxml = payload
                };

                XmlDocument doc = (XmlDocument)JsonConvert.DeserializeXmlNode(JsonConvert.SerializeObject(o));

                var tempO = new
                {
                    FacturaElectronica = payload.FacturaElectronica
                };

                var fe = doc.SelectSingleNode("//FacturaElectronica");
                var feXML = doc.SelectSingleNode("/facturaelectronicaxml");
                feXML.RemoveChild(fe);
                XmlDocument tempDoc = (XmlDocument)JsonConvert.DeserializeXmlNode(JsonConvert.SerializeObject(tempO));
                var howToSign = XadesHelper.Sign(tempDoc).Using(cert).IncludingCertificateInSignature();

                //howToSign.WithProperty("uno", txtPropertyValue.Text, "http://xades.codeplex.com/#properties");

                

                var resultXML = howToSign.SignXML();
                var newfe = feXML.OwnerDocument.ImportNode(resultXML.SelectSingleNode("//FacturaElectronica"), true);
                feXML.AppendChild(newfe);
                //response.errorList = validator.ValidateXML(doc.InnerXml, xsdDocument.FacturaElectronica);


                DataValidator validator = new DataValidator();
                ccc = Constants.Constants.myTestXML;
                //validator.ValidateXML(ccc/*feXML.OuterXml*/, Enums.xsdDocument.FacturaElectronica);
                
                using (HttpClient client = new HttpClient())
                {
                    //ccc = feXML.OuterXml.ToString();
                    //ccc = Constants.Constants.myTestXML;
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, ConfigurationManager.AppSettings[Constants.Constants.receptionEndpoint]);
                    request.Content = new StringContent(ccc, Encoding.UTF8, Constants.Constants.application_json);

                    AuthenticationResponse authenticationResponse = await Authentication.Instance().AuthenticationMH();
                    request.Headers.Authorization = new AuthenticationHeaderValue(Constants.Constants.auth_header_bearer, authenticationResponse.access_token);

                    responseMessage = await client.SendAsync(request);
                }

                

            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }

            string responseString = await responseMessage.Content.ReadAsStringAsync();
            return responseString;
        }
    }
}
