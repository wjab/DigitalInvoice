using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Centauro.DigitalInvoice.BusinessLogic.Model
{
    public class GeneralObject
    {
        public int code { get; set; }
        public string name { get; set; }
    }

    public class GeneralResponseObject
    {
        public int id { get; set; }
        public string code { get; set; }
        public string description { get; set; }
    }
}
