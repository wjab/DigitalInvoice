using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Centauro.DigitalInvoice.BusinessLogic.Model
{
    public class HeaderResponse
    {
        public string X_Error_Cause { get; set; }
        
        public int X_Ratelimit_Limit {get; set;}
        
        public int X_Http_Status { get; set; }
        
        public int X_Ratelimit_Remaining { get; set; }
        
        public int X_Ratelimit_Reset { get; set; }

        public string Jmsxgroupid { get; set; }

        public string Breadcrumbid { get; set; }

        public string Location { get; set; }
    }

    public class HeaderContainer
    {
        public HeaderContainer()
        {
            headerResponse = new HeaderResponse();
        }

        public int StatusCode { get; set; }
        public string ReasonPhrase { get; set; }

        public HeaderResponse headerResponse { get; set; }
    }

}
