using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Centauro.DigitalInvoice.BusinessLogic.Model
{
    public class AceptaRechazaDocumento
    {
        /// <summary>
        /// Clave numÃ©rica del comprobante
        /// </summary>
        public string Clave { get; set; }

        /// <summary>
        /// NÃºmero de cÃ©dula fisica/jurÃ­dica/NITE/DIMEX del vendedor
        /// </summary>
        public long NumeroCedulaEmisor { get; set; }

        /// <summary>
        /// Fecha de emision de la confirmaciÃ³n
        /// </summary>
        public string FechaEmisionDoc { get; set; }

        /// <summary>
        /// Codigo del mensaje de respuesta. 1 aceptado, 2 aceptado parcialmente, 3 rechazado
        /// </summary>
        public int Mensaje { get; set; }

        /// <summary>
        /// Detalle del mensaje
        /// </summary>
        public string DetalleMensaje { get; set; }

        /// <summary>
        /// Monto total del impuesto, que es obligatorio si el comprobante tenga impuesto.
        /// </summary>
        public decimal MontoTotalImpuesto { get; set; }

        /// <summary>
        /// Monto total de la factura
        /// </summary>
        public decimal TotalFactura { get; set; }

        /// <summary>
        /// NÃºmero de cÃ©dula fisica/jurÃ­dica/NITE/DIMEX del comprador
        /// </summary>
        public long NumeroCedulaReceptor { get; set; }

        /// <summary>
        /// NumeraciÃ³n consecutiva de los mensajes de confirmaciÃ³n
        /// </summary>
        public string NumeroConsecutivoReceptor { get; set; }
    }
}
