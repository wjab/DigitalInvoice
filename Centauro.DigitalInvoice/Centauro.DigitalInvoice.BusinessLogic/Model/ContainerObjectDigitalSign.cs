using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Centauro.DigitalInvoice.BusinessLogic.Model
{
    public class ContainerObjectDigitalSign
    {
        public ContainerObjectDigitalSign()
        {
            FirmaDigital = new FirmaDigital();
        }

        public object Object { get; set; }
        public FirmaDigital FirmaDigital { get; set; }
    }
}
