using Centauro.DigitalInvoice.BusinessLogic;
using Centauro.DigitalInvoice.BusinessLogic.Auth;
using Centauro.DigitalInvoice.BusinessLogic.Constants;
using Centauro.DigitalInvoice.BusinessLogic.Enums;
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
    public class AuthController : ApiController
    {
        /*
         * Global variables
         */
        private GenericResponse response;

        [HttpGet]
        public async Task<IHttpActionResult> hola()
        {
            string a = "jojojoj";

            Document sendDocument = new Document();
            AceptaRechazaDocumento aceptaDocumento = new AceptaRechazaDocumento()
            {
                NombreEmisor = "Centauro Solutions",
                NumCedulaEmisor = 1010362541,
                FechaEmisionDoc = DateTime.Now,
                NumConsecutivoCompr = 1,
                TipoDoc = (int)TipoDoc.Factura,
                Mensaje = (int)TipoMensaje.Aceptacion,
                DetalleMensaje = "jojojo detalle mensaje",
                NombreReceptor = "Walter Arguello",
                NumCedulaReceptor = 206370571,
                IdentificacionExtranjero = "206370571",
                NumConsecutivorecep = 1
            };

            AuthenticationResponse aa = await Authentication.Instance().AuthenticationMH();

            return Ok(aa.access_token);
        }         
    }
}
