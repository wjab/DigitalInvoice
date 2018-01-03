using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Centauro.DigitalInvoice.BusinessLogic.Model
{
    public class CallBackResponse
    {
        public CallBackResponse()
        {
            respuestaXml = string.Empty;
        }

        /// <summary>
        /// cadena de 50 caracteres
        /// </summary>
        /// <remarks>Requerido</remarks>
        public string clave { get; set; }

        /// <summary>
        /// Fecha de la factura en formato [yyyy-MM-dd'T'HH:mm:ssZ] como se 
        /// define en [http://tools.ietf.org/html/rfc3339#section-5.6] (date-time).
        /// </summary>
        /// <remarks>Requerido</remarks>
        public string fecha { get; set; }

        [JsonProperty(PropertyName = "ind-estado")]
        /// <summary>
        /// "enum": ["RECIBIDO", "PROCESANDO", "ACEPTADO", "RECHAZADO", "ERROR"]
        /// </summary>
        /// <remarks>Requerido</remarks>
        public string indEstado { get; set; }

        /// <summary>
        /// URL utilizado para que Hacienda envíe la respuesta de aceptación o rechazo. 
        /// Muestra el URL que fue enviado por el obligado tributario.
        /// </summary>
        public string callbackUrl { get; set; }

        [JsonProperty(PropertyName = "respuesta-xml")]
        /// <summary>
        /// Respuesta de aceptación o rechazo en XML firmada por el Ministerio de Hacienda utilizando XAdES-XL. 
        /// El texto del XML debe convertirse a un byte array y codificarse en Base64. 
        /// El mapa de caracteres a utilizar en el XML y en la codificación Base64 es UTF8.
        /// </summary>
        public string respuestaXml { get; set; }
    }
  
}
