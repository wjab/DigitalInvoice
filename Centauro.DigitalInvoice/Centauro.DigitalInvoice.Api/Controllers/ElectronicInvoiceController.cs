using Centauro.DigitalInvoice.Api.Models;
using Centauro.DigitalInvoice.BusinessLogic;
using Centauro.DigitalInvoice.BusinessLogic.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Centauro.DigitalInvoice.Api.Controllers
{
    public class ElectronicInvoiceController : ApiController
    {
        /*
         * Global variables
         */
        //private GenericResponse response;


        /// <summary>
        /// Variables definition
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult ValidateXSD()
        {
            string xml = @"<?xml version=""1.0""?>
                            <books>
	                            <book id=""bk01"">                                     
		                            <author>walter</author>
		                            <title>programacion</title>
		                            <price>10.30</price>
                                    <TipoDoc>01</TipoDoc>
                                </book>
	                            <book id=""bk02"">
		                            <author>roberto</author>
		                            <title>ciclismmo</title>
		                            <price>11.90</price>
                                    <TipoDoc>01</TipoDoc>  
	                            </book>
                            </books>";

            //response = new GenericResponse();
            string myString = "esta es mi respuesta";
            
            //response.errorList = validator.ValidateXML(xml);


            return Ok(JsonConvert.SerializeObject(myString));
        }
    }
}
