using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Centauro.DigitalInvoice.BusinessLogic.Model
{
    //public class facturaelectronicaxml
    //{
    //    public facturaelectronicaxml()
    //    {
    //        CodigoType = new CodigoType();
    //        EmisorType = new EmisorType();
    //        ExoneracionType = new ExoneracionType();
    //        IdentificacionType = new IdentificacionType();
    //        ImpuestoType = new ImpuestoType();
    //        ReceptorType = new ReceptorType();
    //        TelefonoType = new TelefonoType();
    //        UbicacionType = new UbicacionType();

    //        FacturaElectronica = new FacturaElectronica();
    //    }

    //    /*public decimal Cantidad { get; set; }
    //    public decimal Monto { get; set; }

    //    public FacturaElectronicaXML FacturaElectronicaXML { get; set; }*/

    //    public CodigoType CodigoType { get; set; }

    //    public EmisorType EmisorType { get; set; }
    //    public ExoneracionType ExoneracionType { get; set; }
    //    public IdentificacionType IdentificacionType { get; set; }
    //    public ImpuestoType ImpuestoType { get; set; }
    //    public ReceptorType ReceptorType { get; set; }
    //    public TelefonoType TelefonoType { get; set; }
    //    public UbicacionType UbicacionType { get; set; }

    //    public string ClaveType { get; set; }
    //    public decimal DecimalDineroType { get; set; }
    //    public string NumeroConsecutivoType { get; set; }
    //    public string UnidadMedidaType { get; set; }

    //    public FacturaElectronica FacturaElectronica { get; set; }
    //}

    

    

    

    

    

    

    

    


    /// <summary>
    /// Elemento Raiz de la Facturacion ElectrÃ³nica
    /// </summary>
    public class FacturaElectronica
    {
        public FacturaElectronica()
        {
            Emisor = new EmisorType();
            Receptor = new ReceptorType();
            MedioPago = new List<string>();
            DetalleServicio = new DetalleLineas();
            ResumenFactura = new ResumenFactura();
            InformacionReferencia = new InformacionReferencia();
            Normativa = new Normativa();
            Otros = new Otros();
        }

        public string Clave { get; set; }
        public string NumeroConsecutivo { get; set; }
        public string FechaEmision { get; set; }
        public EmisorType Emisor { get; set; }
        public ReceptorType Receptor { get; set; }

        /// <summary>
        /// Condiciones de la venta: 01 Contado, 02 CrÃ©dito, 03 ConsignaciÃ³n, 04 Apartado, 05 Arrendamiento con opciÃ³n de compra, 06 Arrendamiento en funciÃ³n financiera, 99 Otros
        /// </summary>
        public string CondicionVenta { get; set; }

        /// <summary>
        /// Plazo del crÃ©dito, es obligatorio cuando la venta del producto o prestaciÃ³n del servicio sea a crÃ©dito
        /// </summary>
        public string PlazoCredito { get; set; }

        /// <summary>
        /// Corresponde al medio de pago empleado: 01 Efectivo, 02 Tarjeta, 03 Cheque, 04 Transferencia - depÃ³sito bancario, 05 - Recaudado por terceros, 99 Otros
        /// </summary>
        public List<string> MedioPago { get; set; }

        public DetalleLineas DetalleServicio { get; set; }
        public ResumenFactura ResumenFactura { get; set; }
        public InformacionReferencia InformacionReferencia { get; set; }
        public Normativa Normativa { get; set; }
        public Otros Otros { get; set; }
    }

    public class DetalleLineas
    {
        public DetalleLineas()
        {
            LineaDetalle = new List<LineaDetalle>();
        }

        public List<LineaDetalle> LineaDetalle { get; set; }
    }


    /// <summary>
    /// Detalle de la mercancia o servicio prestado
    /// </summary>
    public class LineaDetalle
    {
        public LineaDetalle()
        {
            Codigo = new List<CodigoType>();
            Impuesto = new List<ImpuestoType>();
        }

        /// <summary>
        /// Cada línea del detalle de la mercancia o servicio prestado
        /// </summary>
        public int NumeroLinea { get; set; }

        /// <summary>
        /// Número de línea del detalle
        /// </summary>
        public List<CodigoType> Codigo { get; set; }

        /// <summary>
        /// Cantidad
        /// </summary>
        public decimal Cantidad { get; set; }

        /// <summary>
        /// Unidad de medida
        /// </summary>
        public string UnidadMedida { get; set; }

        /// <summary>
        /// Unidad de medida comercial
        /// </summary>
        public string UnidadMedidaComercial { get; set; }

        /// <summary>
        /// Detalle de la mercancia transferida o servicio prestado
        /// </summary>
        public string Detalle { get; set; }

        /// <summary>
        /// Precio Unitario
        /// </summary>
        public decimal PrecioUnitario { get; set; }

        /// <summary>
        /// Se obtiene de multiplicar el campo cantidad por el campo precio unitario
        /// </summary>
        public decimal MontoTotal { get; set; }

        /// <summary>
        /// Monto de descuento concedido, el cual es obligatorio si existe descuento
        /// </summary>
        public decimal MontoDescuento { get; set; }

        /// <summary>
        /// Naturaleza del descuento, que es obligatorio si existe descuento
        /// </summary>
        public string NaturalezaDescuento { get; set; }

        /// <summary>
        /// Se obtiene de la resta del campo monto total menos monto de descuento concedido
        /// </summary>
        public decimal SubTotal { get; set; }

        /// <summary>
        /// Cuando el producto o servicio este gravado con algÃºn impuesto se debe indicar cada uno de ellos.
        /// </summary>
        public List<ImpuestoType> Impuesto { get; set; }

        /// <summary>
        /// Se obtiene de la suma de los campos subtotal mÃ¡s monto de los impuestos
        /// </summary>
        public decimal MontoTotalLinea { get; set; }
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
        public string FechaEmision { get; set; }
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
            OtroTexto = new OtroTextoObject();
            OtroContenido = new OtroContenidoObject();
        }

        public OtroTextoObject OtroTexto { get; set; }
        public OtroContenidoObject OtroContenido { get; set; }
    }

    public class OtroTextoObject
    {
        [JsonProperty(PropertyName = "-codigo")]
        public string OtroTexto { get; set; }

        [JsonProperty(PropertyName = "#text")]
        public string text { get; set; }
    }

    public class OtroContenidoObject
    {
        [JsonProperty(PropertyName = "-codigo")]
        public string codigo { get; set; }
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

    public class ReceptorType
    {
        public ReceptorType()
        {
            //Identificacion = new IdentificacionType();
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

    public class IdentificacionType
    {
        public string Tipo { get; set; }
        public string Numero { get; set; }
    }

    public class UbicacionType
    {
        public string Provincia { get; set; }
        public string Canton { get; set; }
        public string Distrito { get; set; }
        public string Barrio { get; set; }
        public string OtrasSenas { get; set; }
    }

    public class TelefonoType
    {
        public string CodigoPais { get; set; }
        public string NumTelefono { get; set; }
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

    
    public class ExoneracionType
    {
        /// <summary>
        /// Tipo de documento de exoneraciÃ³n o autorizaciÃ³n. 
        /// 01 Compras Autorizadas, 02 Ventas exentas a diplomÃ¡ticos, 
        /// 03 Orden de compra (instituciones pÃºblicas y otros organismos), 
        /// 04 Exenciones DirecciÃ³n General de Hacienda, 
        /// 05 Zonas Francas, 
        /// 99 Otros
        /// </summary>
        public string TipoDocumento { get; set; }

        /// <summary>
        /// NÃºmero de documento de exoneraciÃ³n o autorizaciÃ³n
        /// </summary>
        public string NumeroDocumento { get; set; }

        /// <summary>
        /// Nombre de la instituciÃ³n o dependencia que emitiÃ³ la exoneraciÃ³n
        /// </summary>
        public string NombreInstitucion { get; set; }

        /// <summary>
        /// Fecha y hora de la emisiÃ³n del documento de exoneraciÃ³n o autorizaciÃ³n.
        /// </summary>
        public string FechaEmision { get; set; }

        /// <summary>
        /// Monto del impuesto exonerado o autorizado sin impuestos
        /// </summary>
        public decimal MontoImpuesto { get; set; }

        /// <summary>
        /// Porcentaje de la compra autorizada o exonerada
        /// </summary>
        public int PorcentajeCompra { get; set; }
    }
}
