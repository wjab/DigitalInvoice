//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Centauro.DigitalInvoice.DataBase
{
    using System;
    using System.Collections.Generic;
    
    public partial class RequestRecord
    {
        public long id { get; set; }
        public string clave { get; set; }
        public string accountId { get; set; }
        public string requestState { get; set; }
        public System.DateTime sentDatetime { get; set; }
        public bool callBackReceived { get; set; }
        public Nullable<System.DateTime> docDatetime { get; set; }
        public string indState { get; set; }
        public string responseXML { get; set; }
        public Nullable<System.DateTime> callBacakDatetime { get; set; }
        public bool sentToUI { get; set; }
    }
}