using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Centauro.DigitalInvoice.BusinessLogic.Model
{
    public class CredentialData
    {
        public string certificate { get; set; }
        public string certificatePassword { get; set; }
        public string atvUser { get; set; }
        public string atvPassword { get; set; }
    }
}
