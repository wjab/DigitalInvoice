using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Centauro.DigitalInvoice.BusinessLogic.Model
{
    public class AuthenticationResponse
    {
        public AuthenticationResponse()
        {
            expires_in = 0;
            refresh_expires_in = 0;
            not_before_policy = 0;
        }

        public string access_token { get; set; }
        public int expires_in { get; set; }
        public int refresh_expires_in { get; set; }
        public string refresh_token { get; set; }
        public string token_type { get; set; }
        public string id_token { get; set; }

        [Column("not-before-policy")]
        public int not_before_policy { get; set; }
        public string session_state { get; set; }
        
        /// <summary>
        /// Custom property to manage the currentDateTime
        /// </summary>
        public DateTime currentDateTime { get; set; }
    }
}
