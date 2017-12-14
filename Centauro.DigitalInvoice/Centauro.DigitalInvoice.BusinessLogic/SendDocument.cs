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

namespace Centauro.DigitalInvoice.BusinessLogic
{
    public class SendDocument
    {
        public async Task<string> SendAceptDenyDocument(AceptaRechazaDocumento payload, AuthenticationResponse password)
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
                //passwordATV
                string psw = ConfigurationManager.AppSettings[Contants.password];
                X509Certificate2 cert = new X509Certificate2();

                byte[] rawCertificateData = File.ReadAllBytes(certificatePath);
                cert.Import(rawCertificateData, psw, X509KeyStorageFlags.PersistKeySet);

                //cert.Import(certificatePath, password);
                string certificateString = cert.ToString(true);
                payload.FirmaDigital.x509Certificado = Convert.ToBase64String(Encoding.ASCII.GetBytes(certificateString));
            }
            catch(Exception ex)
            {
                ex.Message.ToString();
            }
            
            payload.FirmaDigital.firma = Convert.ToBase64String(Encoding.ASCII.GetBytes("hola"));

            using (HttpClient client = new HttpClient())
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, ConfigurationManager.AppSettings[Contants.receptionEndpoint]);
                request.Content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, Contants.application_json); ;

                responseMessage = await client.SendAsync(request);
                return await responseMessage.Content.ReadAsStringAsync();

            }

            //return "";
        }
    }
}
