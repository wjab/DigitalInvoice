using Centauro.DigitalInvoice.BusinessLogic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Centauro.DigitalInvoice.Api.Models
{
    public class GenericResponse
    {
        public int status { get; set; }
        public object input { get; set; }
        public string message { get; set; }
        public IList<IError> errorList { get; set; }
        public object results { get; set; }
    }
}