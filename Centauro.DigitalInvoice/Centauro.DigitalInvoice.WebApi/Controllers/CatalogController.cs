using Centauro.DigitalInvoice.BusinessLogic;
using Centauro.DigitalInvoice.BusinessLogic.Enums;
using Centauro.DigitalInvoice.BusinessLogic.Interface;
using Centauro.DigitalInvoice.BusinessLogic.Model;
using Centauro.DigitalInvoice.BusinessLogic.Utilities;
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
    public class CatalogController : ApiController
    {
        private GenericResponse response;
        private ICatalogs catalogInterface;
        private Dictionary<string, object> input;

        [HttpGet]
        public IHttpActionResult GetLocation(int Provincia = 0, int Canton = 0, int Distrito = 0)
        {
            response = new GenericResponse();
            input = new Dictionary<string, object>();

            try
            {
                input.Add(nameof(Provincia), (Provincia == 0 ? 1 : Provincia) );
                if (Canton > 0) { input.Add(nameof(Canton), Canton); }
                if (Distrito > 0) { input.Add(nameof(Distrito), Distrito); }
                response.input = input;

                LocationType(response, Provincia, Canton, Distrito);
                response.status = HttpStatusCode.OK;
            }
            catch(Exception ex)
            {
                Utils.SetExceptionToResponse(ref response, ex);
            }          

            return Ok(JObject.Parse(JsonConvert.SerializeObject(response)));
        }

        [HttpGet]
        public IHttpActionResult GetDocumentType()
        {
            response = new GenericResponse();
            catalogInterface = new Catalogs();

            try
            {
                response.results = catalogInterface.GetDocumentTypes();
                response.status = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                Utils.SetExceptionToResponse(ref response, ex);
            }

            return Ok(JObject.Parse(JsonConvert.SerializeObject(response)));
        }

        [HttpGet]
        public IHttpActionResult GetSaleCondition()
        {
            response = new GenericResponse();
            catalogInterface = new Catalogs();

            try
            {
                response.results = catalogInterface.GetSaleCondition();
                response.status = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                Utils.SetExceptionToResponse(ref response, ex);
            }

            return Ok(JObject.Parse(JsonConvert.SerializeObject(response)));
        }

        [HttpGet]
        public IHttpActionResult GetPaymentMethods()
        {
            response = new GenericResponse();
            catalogInterface = new Catalogs();

            try
            {
                response.results = catalogInterface.GetPaymentMethods();
                response.status = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                Utils.SetExceptionToResponse(ref response, ex);
            }

            return Ok(JObject.Parse(JsonConvert.SerializeObject(response)));
        }

        [HttpGet]
        public IHttpActionResult GetCodeProductService()
        {
            response = new GenericResponse();
            catalogInterface = new Catalogs();

            try
            {
                response.results = catalogInterface.GetCodeProductService();
                response.status = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                Utils.SetExceptionToResponse(ref response, ex);
            }

            return Ok(JObject.Parse(JsonConvert.SerializeObject(response)));
        }

        [HttpGet]
        public IHttpActionResult GetIdentificationType()
        {
            response = new GenericResponse();
            catalogInterface = new Catalogs();

            try
            {
                response.results = catalogInterface.GetIdentificationType();
                response.status = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                Utils.SetExceptionToResponse(ref response, ex);
            }

            return Ok(JObject.Parse(JsonConvert.SerializeObject(response)));
        }


        [HttpGet]
        public IHttpActionResult GetMeasureUnit()
        {
            response = new GenericResponse();
            catalogInterface = new Catalogs();

            try
            {
                response.results = catalogInterface.GetMeasureUnit();
                response.status = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                Utils.SetExceptionToResponse(ref response, ex);
            }

            return Ok(JObject.Parse(JsonConvert.SerializeObject(response)));
        }


        private void LocationType(GenericResponse response, int Provincia, int Canton, int Distrito)
        {
            catalogInterface = new Catalogs();

            if (Provincia == 0 && Canton == 0 && Distrito == 0)
            {
                response.results = catalogInterface.GetLocation(LocationDistribution.Provincia);
            }
            else if (Provincia > 0 && Canton == 0 && Distrito == 0)
            {
                response.results = catalogInterface.GetLocation(LocationDistribution.Canton, Provincia);
            }
            else if (Provincia > 0 && Canton > 0 && Distrito == 0)
            {
                response.results = catalogInterface.GetLocation(LocationDistribution.Distrito, Provincia, Canton);
            }
            else if (Provincia > 0 && Canton > 0 && Distrito > 0)
            {
                response.results = catalogInterface.GetLocation(LocationDistribution.Barrio, Provincia, Canton, Distrito);
            }
            else
            {
                response.results = catalogInterface.GetLocation(LocationDistribution.Provincia);
            }
        }


    }
}
