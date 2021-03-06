﻿using Centauro.DigitalInvoice.BusinessLogic;
using Centauro.DigitalInvoice.BusinessLogic.Interface;
using Centauro.DigitalInvoice.BusinessLogic.InterfaceImp;
using Centauro.DigitalInvoice.BusinessLogic.Log;
using Centauro.DigitalInvoice.BusinessLogic.Model;
using Centauro.DigitalInvoice.BusinessLogic.Utilities;
using Centauro.DigitalInvoice.DataBase;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace Centauro.DigitalInvoice.WebApi.Controllers
{
    public class DocumentsController : ApiController
    {
        private GenericResponse response;
        private Dictionary<string, object> input;
        private Document actionDocument;

        [HttpPost]
        public async Task<IHttpActionResult> AcceptDocument([FromBody] object document)
        {
            response = new GenericResponse();
            actionDocument = new Document();

            try
            {
                dynamic dynamicObject = JObject.Parse(document.ToString());
                AceptaRechazaDocumento aceptaDocumento = JsonConvert.DeserializeObject<AceptaRechazaDocumento>(dynamicObject.document.ToString());
                /*new AceptaRechazaDocumento()
            {
                NombreEmisor = document.NombreEmisor,
                NumCedulaEmisor = document.NumCedulaEmisor,
                FechaEmisionDoc = DateTime.Now,
                NumConsecutivoCompr = document.NumConsecutivoCompr,
                TipoDoc = document.TipoDoc,
                Mensaje = document.Mensaje,
                DetalleMensaje = document.DetalleMensaje,
                NombreReceptor = document.NombreReceptor,
                NumCedulaReceptor = document.NumCedulaReceptor,
                IdentificacionExtranjero = document.IdentificacionExtranjero,
                NumConsecutivorecep = document.NumConsecutivorecep
            };*/

                //var result = await actionDocument.SendAceptDenyDocument(aceptaDocumento, dynamicObject.accountId);
                                
                response.input = document;
                response.status = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                Utils.SetExceptionToResponse(ref response, ex);
            }

            return Ok(JObject.Parse(JsonConvert.SerializeObject(response)));
        }

        [HttpPost]
        public async Task<IHttpActionResult> DenyDocument([FromBody] object document)
        {
            response = new GenericResponse();
            actionDocument = new Document();

            try
            {
                dynamic dynamicObject = JObject.Parse(document.ToString());
                                
                AceptaRechazaDocumento aceptaDocumento = JsonConvert.DeserializeObject<AceptaRechazaDocumento>(dynamicObject.document.ToString());
                    /*new AceptaRechazaDocumento()
                {
                    NombreEmisor = document.NombreEmisor,
                    NumCedulaEmisor = document.NumCedulaEmisor,
                    FechaEmisionDoc = DateTime.Now,
                    NumConsecutivoCompr = document.NumConsecutivoCompr,
                    TipoDoc = document.TipoDoc,
                    Mensaje = document.Mensaje,
                    DetalleMensaje = document.DetalleMensaje,
                    NombreReceptor = document.NombreReceptor,
                    NumCedulaReceptor = document.NumCedulaReceptor,
                    IdentificacionExtranjero = document.IdentificacionExtranjero,
                    NumConsecutivorecep = document.NumConsecutivorecep
                }; */               

                //var result = await actionDocument.SendAceptDenyDocument(aceptaDocumento, dynamicObject.accountId);

                response.input = document;
                response.status = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                Utils.SetExceptionToResponse(ref response, ex);
            }

            return Ok(JObject.Parse(JsonConvert.SerializeObject(response)));
        }

        [HttpPost]
        public async Task<IHttpActionResult> ElectronicInvoice([FromBody] object request)
        {
            response = new GenericResponse();
            actionDocument = new Document();
            ReceivedDigitalDocument digitalInvoiceResponse;

            string accountId = string.Empty;
            FacturaElectronica electronicInvoice;
            
            try
            {
                dynamic dynamicObject = JObject.Parse(request.ToString());
                accountId = dynamicObject.accountId.ToString();
                electronicInvoice = JsonConvert.DeserializeObject<FacturaElectronica>(dynamicObject.document.ToString());

                digitalInvoiceResponse = await actionDocument.SendElectronicInvoiceToMH(electronicInvoice, accountId); 

                if(digitalInvoiceResponse.statusCode != Convert.ToInt32(HttpStatusCode.Accepted))
                {
                    response.message = digitalInvoiceResponse.reasonPhrase;
                    response.errorList.Add(new Error() { errorMessage = digitalInvoiceResponse.x_Error_Cause });
                    response.status = (HttpStatusCode)digitalInvoiceResponse.statusCode;
                }
                else
                {
                    response.results = digitalInvoiceResponse;
                    response.status = HttpStatusCode.Accepted;
                }
                
                response.input = request;                
            }
            catch (Exception ex)
            {
                Utils.SetExceptionToResponse(ref response, ex);
            }

            return Ok(JObject.Parse(JsonConvert.SerializeObject(response)));
        }

        /// <summary>
        /// Recibe la llamada de confirmación de aceptación o rechazo de una factura
        /// </summary>
        /// <param name="request">CallBackResponse</param>
        /// <returns>OK(200)</returns>
        [HttpPost]
        public IHttpActionResult DocumentCallBack([FromBody] CallBackResponse request)
        {
            IRequestRecordLog requestRecordLog = new RequestRecordLog();
            actionDocument = new Document();

            try
            {
                RequestRecord record = new RequestRecord()
                {
                    clave = request.clave,
                    docDatetime = DateTime.Parse(request.fecha),
                    indState = request.indEstado,
                    responseXML = request.respuestaXml,
                    callBacakDatetime = DateTime.Now
                };

                requestRecordLog.UpdateRequestRecord(record);
                //actionDocument.StoreCallBackResponse(request);
            }
            catch(Exception ex)
            {
                ex.Message.ToString();
                LogManager.RegisterCallBackFailToDataBase(JsonConvert.SerializeObject(request));
            }

            return Ok();
        }

        /// <summary>
        /// Consulta los documentos de una persona o empresa y se puede
        /// determinar el estado del documento
        /// </summary>
        /// <param name="accountId">Identificador de la persona / empresa</param>
        /// <param name="documentId">Identificador del documento</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IHttpActionResult> GetDocumentInfo(string accountId, string documentId)
        {
            response = new GenericResponse();
            actionDocument = new Document();
            input = new Dictionary<string, object>();

            try
            {
                input.Add(nameof(accountId), accountId);
                input.Add(nameof(documentId), documentId);
                response.input = input;

                if (!string.IsNullOrEmpty(accountId) && !string.IsNullOrEmpty(documentId))
                {
                    response.results = await actionDocument.GetDocumentInfo(accountId, documentId);
                    response.status = HttpStatusCode.OK;
                }
                else
                {
                    throw new Exception("AccountId or documentId can't be null or empty");
                }
            }
            catch(Exception ex)
            {
                Utils.SetExceptionToResponse(ref response, ex);
            }

            return Ok(JObject.Parse(JsonConvert.SerializeObject(response)));
        }
        
        [HttpPost]
        public async Task<IHttpActionResult> SendUnregisteredReceptorResponse([FromBody] object request)
        {
            response = new GenericResponse();
            actionDocument = new Document();
            CredentialData credential;
            AceptaRechazaDocumento documento;
            RequestAdditionalInfo additionalInfo;
            ReceivedDigitalDocument digitalInvoiceResponse;

            try
            {
                dynamic dynamicObject = JObject.Parse(request.ToString());

                documento = JsonConvert.DeserializeObject<AceptaRechazaDocumento>(dynamicObject.document.ToString());
                credential = JsonConvert.DeserializeObject<AceptaRechazaDocumento>(dynamicObject.credentials.ToString());
                additionalInfo = JsonConvert.DeserializeObject<RequestAdditionalInfo>(dynamicObject.additionalInfo.ToString());

                digitalInvoiceResponse = await actionDocument.SendAceptDenyDocument(documento, credential, additionalInfo);

                if (digitalInvoiceResponse.statusCode != Convert.ToInt32(HttpStatusCode.Accepted))
                {
                    response.message = digitalInvoiceResponse.reasonPhrase;
                    response.errorList.Add(new Error() { errorMessage = digitalInvoiceResponse.x_Error_Cause });
                    response.status = (HttpStatusCode)digitalInvoiceResponse.statusCode;
                }
                else
                {
                    response.results = digitalInvoiceResponse;
                    response.status = HttpStatusCode.Accepted;
                }

                response.input = request;
            }
            catch (Exception ex)
            {
                Utils.SetExceptionToResponse(ref response, ex);
            }

            return Ok(JObject.Parse(JsonConvert.SerializeObject(response)));
        }

    }
}
