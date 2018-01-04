using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Centauro.DigitalInvoice.BusinessLogic.Model;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.Xml;
using Centauro.DigitalInvoice.BusinessLogic.Auth;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using Centauro.DigitalInvoice.DataBase;
using Centauro.DigitalInvoice.BusinessLogic.Interface;
using Centauro.DigitalInvoice.BusinessLogic.InterfaceImp;
using Centauro.DigitalInvoice.BusinessLogic.Utilities;
using TuesPechkin;
using System.Numerics;
using Centauro.DigitalInvoice.BusinessLogic.Enums;
using Centauro.DigitalInvoice.BusinessLogic.Sign;

namespace Centauro.DigitalInvoice.BusinessLogic
{
    public class Document
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
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, ConfigurationManager.AppSettings[Constants.Constants.mhEndpoint]);
                request.Content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, Constants.Constants.application_json); ;

                responseMessage = await client.SendAsync(request);
                return await responseMessage.Content.ReadAsStringAsync();

            }
        }
        
        public async Task<ReceivedDigitalInvoice> SendElectronicInvoiceToMH(FacturaElectronica payload, string accountId)
        {
            ReceivedDigitalInvoice InvoiceResponse = new ReceivedDigitalInvoice();
            IRequestRecordLog requestRecordLog;
            BuildFiles buildFiles;
            HttpResponseMessage responseMessage;
            XmlDocument xmlDocInvoice;
            string pdfInvoice;
            object newRequest = null;            

            try
            {
                buildFiles = new BuildFiles();
                xmlDocInvoice = new XmlDocument();
                requestRecordLog = new RequestRecordLog();

                var container = new
                {
                    facturaelectronicaxml = payload
                };

                string json = JsonConvert.SerializeObject(container, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                xmlDocInvoice = (XmlDocument)JsonConvert.DeserializeXmlNode(json);    

                #region Sign XML and Validate against XSD

                CustomSignature.SignXML(accountId, payload.FechaEmision, ref xmlDocInvoice);

                /*IList<IError> listErros = DataValidator.Instance().ValidateXML(xmlDocInvoice.OuterXml, xsdDocument.FacturaElectronica);
                if(listErros.Count > 0)
                {
                    throw new Exception("El XML contine errores " + Utils.ConcatElements(listErros));
                }*/

                #endregion

                #region Crear PDF
                pdfInvoice = buildFiles.GetPDFInvoice(payload, accountId);
                #endregion

                #region Crea el request de Factura Digital para Hacienda  

                if (payload.Receptor != null)
                {
                    newRequest = new
                    {
                        clave = payload.Clave,
                        fecha = payload.FechaEmision,
                        emisor = new
                        {
                            tipoIdentificacion = payload.Emisor.Identificacion.Tipo,
                            numeroIdentificacion = payload.Emisor.Identificacion.Numero
                        },
                        comprobanteXml = Convert.ToBase64String(Encoding.UTF8.GetBytes(xmlDocInvoice.OuterXml)),
                        callbackUrl = string.Format(Constants.Constants.RequestApiFormat_2,
                                                ConfigurationManager.AppSettings[Constants.Constants.serverDomain],
                                                ConfigurationManager.AppSettings[Constants.Constants.CallBackUrl])
                    };
                }
                else
                {
                    newRequest = new
                    {
                        clave = payload.Clave,
                        fecha = payload.FechaEmision,
                        emisor = new
                        {
                            tipoIdentificacion = payload.Emisor.Identificacion.Tipo,
                            numeroIdentificacion = payload.Emisor.Identificacion.Numero
                        },
                        receptor = new
                        {
                            tipoIdentificacion = payload.Receptor.Identificacion.Tipo,
                            numeroIdentificacion = payload.Receptor.Identificacion.Tipo
                        },
                        comprobanteXml = Convert.ToBase64String(Encoding.UTF8.GetBytes(xmlDocInvoice.OuterXml)),
                        callbackUrl = string.Format(Constants.Constants.RequestApiFormat_2,
                                                ConfigurationManager.AppSettings[Constants.Constants.serverDomain],
                                                ConfigurationManager.AppSettings[Constants.Constants.CallBackUrl])
                    };
                }

                #endregion

                try
                {
                    HttpCustomClient client = new HttpCustomClient();
                    responseMessage = await client.Post(newRequest, Constants.Constants.mhActionRecepcion, Constants.Constants.mhEndpoint, accountId);

                    #region Evaluate response
                    if (responseMessage != null)
                    {
                        InvoiceResponse.reasonPhrase = responseMessage.ReasonPhrase;
                        InvoiceResponse.statusCode = Convert.ToInt32(responseMessage.StatusCode);

                        if (responseMessage.StatusCode != HttpStatusCode.Accepted)
                        {
                            InvoiceResponse.x_Error_Cause = Convert.ToString(responseMessage.Headers.GetValues(Constants.Constants.header_errorCause).FirstOrDefault());
                            #region Otros Datos Header - de momento no se usan
                            //headerContainer.headerResponse.Breadcrumbid = Convert.ToString(responseMessage.Headers.GetValues(Constants.Constants.header_groupId).FirstOrDefault());
                            //headerContainer.headerResponse.X_Ratelimit_Limit = Convert.ToInt32(responseMessage.Headers.GetValues(Constants.Constants.header_rateLimit).FirstOrDefault());
                            //headerContainer.headerResponse.X_Ratelimit_Remaining = Convert.ToInt32(responseMessage.Headers.GetValues(Constants.Constants.header_reteRemaining).FirstOrDefault());
                            //headerContainer.headerResponse.X_Ratelimit_Reset = Convert.ToInt32(responseMessage.Headers.GetValues(Constants.Constants.header_rateReset).FirstOrDefault());
                            #endregion
                        }
                        else
                        {
                            InvoiceResponse.fileLocation = responseMessage.Headers.GetValues(Constants.Constants.header_location).FirstOrDefault();

                            #region Bitacora request
                            requestRecordLog.RegisterRequestRecord(payload.Clave, accountId, InvoiceResponse.reasonPhrase);
                            #endregion
                        }

                        InvoiceResponse.xmlInvoice = Convert.ToBase64String(Encoding.UTF8.GetBytes(xmlDocInvoice.OuterXml));
                        InvoiceResponse.pdfInvoice = pdfInvoice;
                    }
                    else
                    {
                        throw new Exception(Constants.Constants.fail_send_electronic_invoice);
                    }
                    #endregion                                        
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (CryptographicException e)
            {
                throw e;
            }

            return InvoiceResponse;
        }

        public void StoreCallBackResponse(CallBackResponse callBack)
        {
            try
            {
                using (DigitalInvoiceEntities context = new DigitalInvoiceEntities())
                {
                    context.CallBackReceived.Add(new CallBackReceived()
                    {
                        IdCallBack = Guid.NewGuid().ToString(),
                        clave = callBack.clave,
                        fecha = callBack.fecha,
                        indEstado = callBack.indEstado,
                        respuestaXml = callBack.respuestaXml,
                        callBackUrl = callBack.callbackUrl,
                        fechaCallBack = DateTime.Now
                    });

                    context.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                ex.Message.ToString();
            }
        }

        public async Task<CallBackResponse> GetDocumentInfo(string accountId, string documentId)
        {
            CallBackResponse callBackResponse = new CallBackResponse();
            HttpCustomClient client;
            string stringResponse = string.Empty;

            try
            {
                client = new HttpCustomClient();
                stringResponse = await client.Get(documentId, Constants.Constants.mhActionRecepcion);

                if (!string.IsNullOrEmpty(stringResponse))
                {
                    callBackResponse = JsonConvert.DeserializeObject<CallBackResponse>(stringResponse);
                }
                else
                {
                    throw new Exception(Constants.Constants.fail_get_document_from_hacienda);
                }
            }
            catch(Exception ex)
            {
                ex.Message.ToString();
                throw ex;
            }

            return callBackResponse;
        }
                
    }
}
