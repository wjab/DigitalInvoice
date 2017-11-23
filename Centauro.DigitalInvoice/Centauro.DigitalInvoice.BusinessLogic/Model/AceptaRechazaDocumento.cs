using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Centauro.DigitalInvoice.BusinessLogic.Model
{
    public class AceptaRechazaDocumento
    {
        public AceptaRechazaDocumento()
        {
            FirmaDigital = new FirmaDigital();
        }

        public string NombreEmisor { get; set; }
        public long NumCedulaEmisor { get; set; }
        public DateTime FechaEmisionDoc { get; set; }
        public long NumConsecutivoCompr { get; set; }
        public int TipoDoc { get; set; }
        public int Mensaje { get; set; }
        public string DetalleMensaje { get; set; }
        public string NombreReceptor { get; set; }
        public long NumCedulaReceptor { get; set; }

        public string IdentificacionExtranjero { get; set; }
        public long NumConsecutivorecep { get; set; }

        public FirmaDigital FirmaDigital { get; set; }
    }
}
