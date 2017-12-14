using Centauro.DigitalInvoice.BusinessLogic;
using Centauro.DigitalInvoice.BusinessLogic.Auth;
using Centauro.DigitalInvoice.BusinessLogic.Model;
using Centauro.DigitalInvoice.BusinessLogic.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Centauro.DigitalInvoice.WebApi.Controllers
{
    public class DocumentsController : ApiController
    {
        private GenericResponse response;
        private ICatalogs catalogInterface;
        private Dictionary<string, object> input;
        
        [HttpPost]
        public async Task<IHttpActionResult> AcceptDocument(AceptaRechazaDocumento document)
        {
            response = new GenericResponse();

            try
            {
                SendDocument sendDocument = new SendDocument();
                AceptaRechazaDocumento aceptaDocumento = new AceptaRechazaDocumento()
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
                };

                AuthenticationResponse aa = await Authentication.AUthenticationMH_1();
                var result = await sendDocument.SendAceptDenyDocument(aceptaDocumento, aa);
                                
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
        public async Task<IHttpActionResult> DenyDocument(AceptaRechazaDocumento document)
        {
            response = new GenericResponse();

            try
            {
                SendDocument sendDocument = new SendDocument();
                AceptaRechazaDocumento aceptaDocumento = new AceptaRechazaDocumento()
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
                };

                AuthenticationResponse aa = await Authentication.AUthenticationMH_1();
                var result = await sendDocument.SendAceptDenyDocument(aceptaDocumento, aa);

                response.input = document;
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
