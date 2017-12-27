using Centauro.DigitalInvoice.BusinessLogic.Interface;
using Centauro.DigitalInvoice.BusinessLogic.InterfaceImp;
using Centauro.DigitalInvoice.BusinessLogic.Model;
using Centauro.DigitalInvoice.BusinessLogic.Utilities;
using Centauro.DigitalInvoice.DataBase;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Centauro.DigitalInvoice.WebApi.Controllers
{
    public class AccountController : ApiController
    {
        private GenericResponse response;
        private IAccount accountInterface;

        [HttpGet]
        public IHttpActionResult GetAccountById(string id)
        {
            response = new GenericResponse();
            accountInterface = new AccountImp();

            try
            {
                response.input = id;
                response.results = accountInterface.GetAccountById(id);

                if (response.results == null)
                {
                    response.status = HttpStatusCode.NotFound;
                }
                else
                {
                    response.status = HttpStatusCode.OK;
                }
            }
            catch (Exception ex)
            {
                Utils.SetExceptionToResponse(ref response, ex);
            }

            return Ok(JObject.Parse(JsonConvert.SerializeObject(response)));
        }

        [HttpPost]
        public IHttpActionResult UpsertAccount([FromBody] Account request)
        {
            response = new GenericResponse();
            accountInterface = new AccountImp();            

            try
            {
                response.input = request;
                response.results = accountInterface.UpsertAccount(request);
                response.status = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                Utils.SetExceptionToResponse(ref response, ex);
            }

            return Ok(JObject.Parse(JsonConvert.SerializeObject(response)));
        }

        [HttpDelete]
        public IHttpActionResult DeleteAccount(string id)
        {
            response = new GenericResponse();
            accountInterface = new AccountImp();

            try
            {
                response.input = id;
                response.results = accountInterface.DeleteAccount(id);
                response.status = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                Utils.SetExceptionToResponse(ref response, ex);
            }

            return Ok(JObject.Parse(JsonConvert.SerializeObject(response)));
        }
    }
}
