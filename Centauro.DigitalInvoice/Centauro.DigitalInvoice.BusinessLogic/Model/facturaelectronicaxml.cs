using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Centauro.DigitalInvoice.BusinessLogic.Model
{
    public class facturaelectronicaxml
    {
        public facturaelectronicaxml()
        {
            CodigoType = new CodigoType();
            EmisorType = new EmisorType();
            ExoneracionType = new ExoneracionType();
            IdentificacionType = new IdentificacionType();
            ImpuestoType = new ImpuestoType();
            ReceptorType = new ReceptorType();
            TelefonoType = new TelefonoType();
            UbicacionType = new UbicacionType();

            FacturaElectronica = new FacturaElectronica();
        }

        /*public decimal Cantidad { get; set; }
        public decimal Monto { get; set; }

        public FacturaElectronicaXML FacturaElectronicaXML { get; set; }*/

        public CodigoType CodigoType { get; set; }

        public EmisorType EmisorType { get; set; }
        public ExoneracionType ExoneracionType { get; set; }
        public IdentificacionType IdentificacionType { get; set; }
        public ImpuestoType ImpuestoType { get; set; }
        public ReceptorType ReceptorType { get; set; }
        public TelefonoType TelefonoType { get; set; }
        public UbicacionType UbicacionType { get; set; }

        public string ClaveType { get; set; }
        public decimal DecimalDineroType { get; set; }
        public string NumeroConsecutivoType { get; set; }
        public string UnidadMedidaType { get; set; }

        public FacturaElectronica FacturaElectronica { get; set; }
    }

    public class CodigoType
    {
        public string Tipo { get; set; }
        public string Codigo { get; set; }
    }

    public class EmisorType
    {
        public EmisorType()
        {
            Identificacion = new IdentificacionType();
            Ubicacion = new UbicacionType();
            Telefono = new TelefonoType();
            Fax = new TelefonoType();
        }

        public string Nombre { get; set; }
        public IdentificacionType Identificacion { get; set; }
        public string NombreComercial { get; set; }
        public UbicacionType Ubicacion { get; set; }
        public TelefonoType Telefono { get; set; }
        public TelefonoType Fax { get; set; }
        public string CorreoElectronico { get; set; }
    }

    public class ExoneracionType
    {
        public string TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public string NombreInstitucion { get; set; }
        public DateTime FechaEmision { get; set; }
        public decimal MontoImpuesto { get; set; }
        public int PorcentajeCompra { get; set; }
    }

    public class IdentificacionType
    {
        public string Tipo { get; set; }
        public string Numero { get; set; }
    }

    public class ImpuestoType
    {
        public ImpuestoType()
        {
            Exoneracion = new ExoneracionType();
        }

        public string Codigo { get; set; }
        public decimal Tarifa { get; set; }
        public decimal Monto { get; set; }
        public ExoneracionType Exoneracion { get; set; }
    }

    public class ReceptorType
    {
        public ReceptorType()
        {
            Identificacion = new IdentificacionType();
            Ubicacion = new UbicacionType();
            Telefono = new TelefonoType();
            Fax = new TelefonoType();
        }

        public string Nombre { get; set; }
        public IdentificacionType Identificacion { get; set; }
        public string IdentificacionExtranjero { get; set; }
        public string NombreComercial { get; set; }
        public UbicacionType Ubicacion { get; set; }
        public TelefonoType Telefono { get; set; }
        public TelefonoType Fax { get; set; }
        public string CorreoElectronico { get; set; }
    }

    public class TelefonoType
    {
        public string CodigoPais { get; set; }
        public string NumTelefono { get; set; }
    }

    public class UbicacionType
    {
        public string Provincia { get; set; }
        public string Canton { get; set; }
        public string Distrito { get; set; }
        public string Barrio { get; set; }
        public string OtrasSenas { get; set; }
    }

    public class FacturaElectronica
    {
        public FacturaElectronica()
        {
            DetalleServicio = new List<LineaDetalle>();
            ResumenFactura = new ResumenFactura();
            InformacionReferencia = new InformacionReferencia();
            Normativa = new Normativa();
            Otros = new Otros();
        }

        public string Clave { get; set; }
        public string NumeroConsecutivo { get; set; }
        public DateTime FechaEmision { get; set; }
        public string Emisor { get; set; }
        public string Receptor { get; set; }
        public string CondicionVenta { get; set; }
        public string PlazoCredito { get; set; }
        public string MedioPago { get; set; }

        public List<LineaDetalle> DetalleServicio { get; set; }
        public ResumenFactura ResumenFactura { get; set; }
        public InformacionReferencia InformacionReferencia { get; set; }
        public Normativa Normativa { get; set; }
        public Otros Otros { get; set; }
    }      

    public class LineaDetalle
    {
        public int NumeroLinea { get; set; }
        public object Codigo { get; set; }
        public decimal Cantidad { get; set; }
        public object UnidadMedida { get; set; }
        public string UnidadMedidaComercial { get; set; }
        public string Detalle { get; set; }
        public object PrecioUnitario { get; set; }
        public object MontoTotal { get; set; }
        public object MontoDescuento { get; set; }
        public string NaturalezaDescuento { get; set; }
        public object SubTotal { get; set; }
        public object Impuesto { get; set; }
        public object MontoTotalLinea { get; set; }

    }

    public class ResumenFactura
    {
        public string CodigoMoneda { get; set; }
        public object TipoCambio { get; set; }
        public object TotalServGravados { get; set; }
        public object TotalServExentos { get; set; }
        public object TotalMercanciasGravadas { get; set; }
        public object TotalMercanciasExentas { get; set; }
        public object TotalGravado { get; set; }
        public object TotalExento { get; set; }
        public object TotalVenta { get; set; }
        public object TotalDescuentos { get; set; }
        public object TotalVentaNeta { get; set; }
        public object TotalImpuesto { get; set; }
        public object TotalComprobante { get; set; }
    }

    public class InformacionReferencia
    {
        public string TipoDoc { get; set; }
        public string Numero { get; set; }
        public DateTime FechaEmision { get; set; }
        public string Codigo { get; set; }
        public string Razon { get; set; }
    }

    public class Normativa
    {
        public string NumeroResolucion { get; set; }
        public string FechaResolucion { get; set; }
    }

    public class Otros
    {
        public Otros()
        {
            OtroTexto = new List<Model.OtroTexto>();
            TipoDoc = new List<OtroContenido>();
        }

        public List<OtroTexto> OtroTexto { get; set; }
        public List<OtroContenido> TipoDoc { get; set; }
    }

    public class OtroTexto
    {
        public string _codigo { get; set; }
    }

    public class OtroContenido
    {
        public object any { get; set; }
        public string _codigo { get; set; }
    }   

}
