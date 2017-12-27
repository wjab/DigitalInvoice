using Centauro.DigitalInvoice.BusinessLogic;
using Centauro.DigitalInvoice.BusinessLogic.Auth;
using Centauro.DigitalInvoice.BusinessLogic.Constants;
using Centauro.DigitalInvoice.BusinessLogic.Enums;
using Centauro.DigitalInvoice.BusinessLogic.Interface;
using Centauro.DigitalInvoice.BusinessLogic.Model;
using Centauro.DigitalInvoice.BusinessLogic.Utilities;
using Centauro.DigitalInvoice.BusinessLogic.Xades;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml;

namespace Centauro.DigitalInvoice.WebApi.Controllers
{
    public class DocumentsController : ApiController
    {
        private GenericResponse response;
        private Dictionary<string, object> input;
        private SendDocument sendDocument;
        private DataValidator validator;

        [HttpPost]
        public async Task<IHttpActionResult> AcceptDocument([FromBody] object document)
        {
            response = new GenericResponse();
            sendDocument = new SendDocument();

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

                var result = await sendDocument.SendAceptDenyDocument(aceptaDocumento, dynamicObject.accountId);
                                
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
            sendDocument = new SendDocument();

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

                var result = await sendDocument.SendAceptDenyDocument(aceptaDocumento, dynamicObject.accountId);

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
            sendDocument = new SendDocument();

            string accountId = string.Empty;
            facturaelectronicaxml electronicInvoice;
            
            try
            {
                dynamic dynamicObject = JObject.Parse(request.ToString());
                accountId = dynamicObject.accountId.ToString();
                electronicInvoice = JsonConvert.DeserializeObject<facturaelectronicaxml>(dynamicObject.document.ToString());

                electronicInvoice = new facturaelectronicaxml();

                electronicInvoice.EmisorType.Nombre = "";

                electronicInvoice.ExoneracionType.NombreInstitucion = "";

                electronicInvoice.IdentificacionType.Tipo = "";

                electronicInvoice.ImpuestoType.Codigo = "";

                electronicInvoice.ReceptorType.Nombre = "";

                electronicInvoice.TelefonoType.CodigoPais = "";

                electronicInvoice.UbicacionType.Provincia = "";

                electronicInvoice.ClaveType = "";
                electronicInvoice.DecimalDineroType = 0;
                electronicInvoice.NumeroConsecutivoType = "";
                electronicInvoice.UnidadMedidaType = "";

                electronicInvoice.FacturaElectronica.Clave = "";
                electronicInvoice.FacturaElectronica.NumeroConsecutivo = "";
                electronicInvoice.FacturaElectronica.FechaEmision = DateTime.Now;
                electronicInvoice.FacturaElectronica.Emisor = "";
                electronicInvoice.FacturaElectronica.Receptor = "";
                electronicInvoice.FacturaElectronica.CondicionVenta = "";
                electronicInvoice.FacturaElectronica.PlazoCredito = "";
                electronicInvoice.FacturaElectronica.MedioPago = "";
                
                response.results = await sendDocument.SendElectronicInvoice(electronicInvoice, accountId); 
                response.input = request;
                response.status = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                Utils.SetExceptionToResponse(ref response, ex);
            }

            return Ok(JObject.Parse(JsonConvert.SerializeObject(response)));
        }
        
    }
}
