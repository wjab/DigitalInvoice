using Centauro.DigitalInvoice.BusinessLogic;
using Centauro.DigitalInvoice.BusinessLogic.Auth;
using Centauro.DigitalInvoice.BusinessLogic.Constants;
using Centauro.DigitalInvoice.BusinessLogic.Enums;
using Centauro.DigitalInvoice.BusinessLogic.Model;
using Centauro.DigitalInvoice.BusinessLogic.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Centauro.DigitalInvoice.WebApi.Controllers
{
    public class AuthController : ApiController
    {

        [HttpGet]
        public IHttpActionResult OatuBearer(string accountId)
        {           
            return Ok(Authentication.Instance().AuthenticationMHById(accountId));
        }         
    }
}
