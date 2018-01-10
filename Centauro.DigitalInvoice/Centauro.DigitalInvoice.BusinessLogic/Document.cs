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
        public async Task<ReceivedDigitalDocument> SendAceptDenyDocument(AceptaRechazaDocumento documento, CredentialData credential, RequestAdditionalInfo additionalInfo)
        {
            ReceivedDigitalDocument InvoiceResponse = new ReceivedDigitalDocument();
            string respuesta = string.Empty;
            X509Certificate2 cert;
            AuthenticationResponse oAuthResponse;
            XmlDocument xmlDocInvoice;
            object newRequest = null;

            try
            {
                if (!string.IsNullOrEmpty(credential.certificate) && !string.IsNullOrEmpty(credential.certificatePassword))
                {
                    cert = new X509Certificate2(Convert.FromBase64String(credential.certificate), credential.certificatePassword);
                }
                else
                {
                    throw new Exception(Constants.Constants.fail_CertificateInfo_incomplete);
                }
                               
                xmlDocInvoice = Utils.GetXmlFromObject(documento);

                CustomSignature.SignXML(documento.FechaEmisionDoc, ref xmlDocInvoice, null, cert);
                
                oAuthResponse = await Authentication.Instance().AuthenticationMH(credential.atvUser, credential.atvPassword);

                if (oAuthResponse != null)
                {
                    newRequest = new
                    {
                        clave = documento.Clave,
                        fecha = documento.FechaEmisionDoc,
                        emisor = new
                        {
                            additionalInfo.Emisor.tipoIdentificacion,
                            additionalInfo.Emisor.numeroIdentificacion
                        },
                        receptor = new
                        {
                            additionalInfo.Receptor.tipoIdentificacion,
                            additionalInfo.Receptor.numeroIdentificacion
                        },
                        consecutivoReceptor = additionalInfo.ConsecutivoReceptor,
                        comprobanteXml = Convert.ToBase64String(Encoding.UTF8.GetBytes(xmlDocInvoice.OuterXml)),
                        callbackUrl = string.Format(Constants.Constants.RequestApiFormat_2,
                                                ConfigurationManager.AppSettings[Constants.Constants.serverDomain],
                                                ConfigurationManager.AppSettings[Constants.Constants.CallBackUrl])
                    };
                }
                else
                {
                    throw new Exception(Constants.Constants.fail_communication_oauth_hacienda);
                }

                try
                {
                    HttpCustomClient client = new HttpCustomClient();
                    InvoiceResponse = await client.Post(newRequest, Constants.Constants.mhActionRecepcion, Constants.Constants.mhEndpoint, null, oAuthResponse.access_token);

                    InvoiceResponse.xmlInvoice = Convert.ToBase64String(Encoding.UTF8.GetBytes(xmlDocInvoice.OuterXml));
                    InvoiceResponse.pdfInvoice = "";
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }

            return InvoiceResponse;
        }
        
        public async Task<ReceivedDigitalDocument> SendElectronicInvoiceToMH(FacturaElectronica payload, string accountId)
        {
            ReceivedDigitalDocument InvoiceResponse = new ReceivedDigitalDocument();
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
    
                xmlDocInvoice = Utils.GetXmlFromObject(payload);

                #region Sign XML and Validate against XSD

                CustomSignature.SignXML(payload.FechaEmision, ref xmlDocInvoice, accountId);

                //IList<IError> listErros = DataValidator.Instance().ValidateXML(xmlDocInvoice.OuterXml, xsdDocument.FacturaElectronica);
                //if(listErros.Count > 0)
                //{
                //    throw new Exception(string.Format(Constants.Constants.RequestApiFormat_2, Constants.Constants.xml_has_errors, Utils.ConcatElements(listErros)) );
                //}

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
                    InvoiceResponse = await client.Post(newRequest, Constants.Constants.mhActionRecepcion, Constants.Constants.mhEndpoint, accountId);

                    InvoiceResponse.xmlInvoice = Convert.ToBase64String(Encoding.UTF8.GetBytes(xmlDocInvoice.OuterXml));
                    InvoiceResponse.pdfInvoice = pdfInvoice;
                                                           
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
