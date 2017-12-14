using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Centauro.DigitalInvoice.BusinessLogic.Enums
{
    public enum TipoDoc
    {
        Factura = 01, 
        NotaDebito = 02, 
        NotaCredito = 03 
    }

    public enum TipoMensaje
    {
        Aceptacion = 1,
        Rechazo = 9
    }

    public enum xsdDocument
    {
        AceptaRechaza = 1,
        FacturaElectronica = 2,
        ResumenPeriodoCompras = 3,
        ResumenPeriodoVentas = 4,
        ResumenPeriodoComprasVentas = 5,
        Sample = 12
    }






    public enum LocationDistribution
    {
        Provincia = 1,
        Canton = 2,
        Distrito = 3,
        Barrio = 4
    }

}
