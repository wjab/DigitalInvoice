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
            HttpResponseMessage responseMessage;
            XmlDocument xmlDocSignature;
            XmlDocument xmlDocInvoice;
            IAccount accountImp;
            string singIdentifier, htmlInvoice, imageQR;
            SHA256 sha256;
            object newRequest = null;
            Account accountData;
            X509Certificate2 cert;
            StringBuilder lineasDetalle;
            List<object> argsLinea, argsDocument;

            try
            {
                xmlDocSignature = new XmlDocument();
                xmlDocInvoice = new XmlDocument();
                sha256 = SHA256Managed.Create();
                accountImp = new AccountImp();
                BigInteger bigIntClave = new BigInteger(Encoding.UTF8.GetBytes(payload.Clave));
                payload.Clave = bigIntClave.ToString();

                singIdentifier = ConfigurationManager.AppSettings[Constants.Constants.SignIdentifier];

                var container = new
                {
                    facturaelectronicaxml = payload
                };

                string json = JsonConvert.SerializeObject(container, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                xmlDocInvoice = (XmlDocument)JsonConvert.DeserializeXmlNode(json);
                accountData = accountImp.GetAccountById(accountId);
                if(accountData != null)
                {
                    cert = new X509Certificate2(Convert.FromBase64String(accountData.certificate), accountData.certificatePIN.ToString());
                }
                else
                {
                    throw new Exception("Fallo al obtener información de la cuenta");
                }
                
                xmlDocSignature.Load(string.Format(Constants.Constants.RequestApiFormat_2, AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings[Constants.Constants.signTemplate]));

                #region Generar QR

                imageQR = Utils.GenerateQRCode(
                                string.Format(ConfigurationManager.AppSettings[Constants.Constants.urlS3Invoice], 
                                                payload.Clave, 
                                                string.Format(Constants.Constants.RequestApiFormat_2, 
                                                                payload.Clave, 
                                                                Constants.Constants.xmlExtension)));

                #endregion


                #region Estructura para firma digital

                /*
                 * * 0 : id
                 * 1 : XML document base64
                 * 2 : (no se que es) Properties
                 * 3 : current datetime
                 * 4 : certificate data base64
                 * 5 : X509IssuerName
                 * 6 : X509SerialNumber
                 * 7 : Identifier
                 * 8 : Identifier sha256
                 */
                string xmlSignature = xmlDocSignature.OuterXml;
                List<string> parameters = new List<string>();
                parameters.Add(Guid.NewGuid().ToString("N")); // 0
                parameters.Add(Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(xmlDocSignature.OuterXml)))); // 1
                parameters.Add("5JVZPTwN5Lj0sGTfFzaUeMKCo/xbCAj7fw6TLUFtZIk="); // 2
                parameters.Add(DateTime.Now.ToString()); // 3
                parameters.Add(Convert.ToBase64String(Encoding.ASCII.GetBytes(cert.ToString(true)))); // 4
                parameters.Add(cert.IssuerName.ToString()); // 5
                parameters.Add(cert.SerialNumber); // 6
                parameters.Add(singIdentifier); // 7
                parameters.Add(Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(singIdentifier)))); // 8

                xmlSignature = string.Format(xmlSignature, parameters.ToArray());

                XmlDocumentFragment xFrag = xmlDocInvoice.CreateDocumentFragment();
                xFrag.InnerXml = xmlSignature;
                xmlDocInvoice.ChildNodes[0].AppendChild(xFrag);

                #region Validate against XSD
                /*IList<IError> listErros = DataValidator.Instance().ValidateXML(xmlDocInvoice.OuterXml, xsdDocument.FacturaElectronica);
                if(listErros.Count > 0)
                {
                    throw new Exception("El XML contine errores " + Utils.ConcatElements(listErros));
                }*/
                #endregion

                #endregion

                #region Crear HTML

                htmlInvoice = File.ReadAllText(string.Format(Constants.Constants.RequestApiFormat_2, AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings[Constants.Constants.invoiceTemplate]));
                
                lineasDetalle = new StringBuilder();
                argsDocument = new List<object>();

                #region Datos Emisor
                argsDocument.Add(payload.Emisor.Nombre);
                argsDocument.Add(payload.Emisor.Ubicacion.OtrasSenas);
                argsDocument.Add(string.Format(Constants.Constants.RequestApiFormat_2, payload.Emisor.Telefono.CodigoPais, payload.Emisor.Telefono.NumTelefono ));
                argsDocument.Add(payload.Emisor.CorreoElectronico);
                #endregion

                #region Datos Receptor
                argsDocument.Add(payload.Receptor.Nombre);
                argsDocument.Add(payload.Receptor.Ubicacion.OtrasSenas);
                argsDocument.Add(payload.Receptor.CorreoElectronico);
                #endregion
                                
                argsDocument.Add(payload.Clave);
                argsDocument.Add(payload.FechaEmision);                
                
                #region Footer Otros - Notas
                argsDocument.Add(payload.Otros.OtroContenido.codigo);
                argsDocument.Add(payload.Otros.OtroTexto.text);
                #endregion

                #region Lineas Detalle
                foreach (var item in payload.DetalleServicio.LineaDetalle)
                {
                    argsLinea = new List<object>();
                    argsLinea.Add(item.Codigo[0].Codigo);
                    argsLinea.Add(item.Detalle);
                    argsLinea.Add(item.PrecioUnitario);
                    argsLinea.Add(item.Cantidad);
                    argsLinea.Add(item.MontoTotal);

                    lineasDetalle.Append(string.Format(Constants.Constants.lineaDetalle, argsLinea.ToArray()));
                }
                argsDocument.Add(lineasDetalle.ToString());

                #endregion

                #region Resumen - Totales
                argsLinea = new List<object>();
                argsLinea.Add(payload.ResumenFactura.TotalServGravados);
                argsLinea.Add(payload.ResumenFactura.TotalServExentos);
                argsLinea.Add(payload.ResumenFactura.TotalMercanciasGravadas);
                argsLinea.Add(payload.ResumenFactura.TotalMercanciasExentas);
                argsLinea.Add(payload.ResumenFactura.TotalVenta);
                argsLinea.Add(payload.ResumenFactura.TotalDescuentos);
                argsLinea.Add(payload.ResumenFactura.TotalImpuesto);
                argsLinea.Add(payload.ResumenFactura.TotalComprobante);

                argsDocument.Add(string.Format(Constants.Constants.lineaTotales, argsLinea.ToArray()));
                #endregion                

                argsDocument.Add(imageQR);
                argsDocument.Add(imageQR);

                htmlInvoice = string.Format(htmlInvoice, argsDocument.ToArray());

                #endregion

                #region Crear PDF

                IDocument htmlDocToPdf = new HtmlToPdfDocument
                {
                    Objects =
                                {
                                    new ObjectSettings
                                    {
                                        HtmlText = htmlInvoice,
                                        ProduceExternalLinks = true,
                                        ProduceLocalLinks = true,
                                        WebSettings =
                                        {
                                            PrintBackground = true,
                                            PrintMediaType = true,
                                            LoadImages = true,
                                            DefaultEncoding = Constants.Constants.encoding_UTF_8,
                                            UserStyleSheet = string.Format( Constants.Constants.RequestApiFormat_2,
                                                                            AppDomain.CurrentDomain.BaseDirectory,
                                                                            ConfigurationManager.AppSettings[Constants.Constants.invoiceTemplateCss])
                                        }
                                    }
                                }
                };
                
                #endregion

                try
                {
                    #region Build Request Factura Digital   
                         
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

                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, string.Format(Constants.Constants.RequestApiFormat_2,
                                                                                                        ConfigurationManager.AppSettings[Constants.Constants.mhEndpoint],
                                                                                                        ConfigurationManager.AppSettings[Constants.Constants.mhActionRecepcion])); //"https://api.comprobanteselectronicos.go.cr/recepcion-sandbox/v1/recepcion");

                    request.Content = new StringContent(JsonConvert.SerializeObject(newRequest), Encoding.UTF8, Constants.Constants.application_json);
                    request.Headers.Authorization = new AuthenticationHeaderValue(Constants.Constants.auth_header_bearer, await Authentication.Instance().AuthenticationMHById(accountId)/*authenticationResponse.access_token*/);

                    using (HttpClient client = new HttpClient())
                    {
                        responseMessage = await client.SendAsync(request);

                        if (responseMessage != null)
                        {
                            InvoiceResponse.reasonPhrase = responseMessage.ReasonPhrase;
                            InvoiceResponse.statusCode = Convert.ToInt32(responseMessage.StatusCode);

                            if (responseMessage.StatusCode != HttpStatusCode.Accepted)
                            {
                                //headerContainer.headerResponse.Breadcrumbid = Convert.ToString(responseMessage.Headers.GetValues(Constants.Constants.header_groupId).FirstOrDefault());
                                InvoiceResponse.x_Error_Cause = Convert.ToString(responseMessage.Headers.GetValues(Constants.Constants.header_errorCause).FirstOrDefault());
                                //headerContainer.headerResponse.X_Ratelimit_Limit = Convert.ToInt32(responseMessage.Headers.GetValues(Constants.Constants.header_rateLimit).FirstOrDefault());
                                //headerContainer.headerResponse.X_Ratelimit_Remaining = Convert.ToInt32(responseMessage.Headers.GetValues(Constants.Constants.header_reteRemaining).FirstOrDefault());
                                //headerContainer.headerResponse.X_Ratelimit_Reset = Convert.ToInt32(responseMessage.Headers.GetValues(Constants.Constants.header_rateReset).FirstOrDefault());
                            }
                            else
                            {
                                InvoiceResponse.fileLocation = responseMessage.Headers.GetValues(Constants.Constants.header_location).FirstOrDefault();
                            }
                            
                            InvoiceResponse.xmlInvoice = Convert.ToBase64String(Encoding.UTF8.GetBytes(xmlDocInvoice.OuterXml));
                            InvoiceResponse.pdfInvoice = Convert.ToBase64String(pdfConverter.Convert(htmlDocToPdf));
                        }            
                        else
                        {
                            throw new Exception("Fallo al enviar la factura electrónica");
                        }            
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw ex;
                }
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
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
                    throw new Exception("Fallo al optener el Documento en el servidor del Ministerio de Hacienda");
                }
            }
            catch(Exception ex)
            {
                ex.Message.ToString();
                throw ex;
            }

            return callBackResponse;
        }

        #region HTML to PDF Converter
        static IConverter pdfConverter =
            new ThreadSafeConverter(
                new RemotingToolset<PdfToolset>(
                    new Win32EmbeddedDeployment(new TempFolderDeployment())
                )
            );
        #endregion
    }
}
