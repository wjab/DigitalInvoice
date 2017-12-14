using Centauro.DigitalInvoice.BusinessLogic.Constants;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Centauro.DigitalInvoice.BusinessLogic
{
    public class CustomDelegatingHandler : DelegatingHandler
    {
        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = null;

            try
            {
                response = await base.SendAsync(request, cancellationToken);
            }
            catch(Exception ex)
            {
                ex.Message.ToString();
            }

            return response;
        }
    }

    public class HttpCutomClient
    {
        private CustomDelegatingHandler customHandler;        

        public async Task<string> Get(Dictionary<string, string> requestObject, string functionToCall)
        {
            HttpResponseMessage response;
            string responseString = string.Empty;

            try
            {
                customHandler = new CustomDelegatingHandler();
                HttpClient client = HttpClientFactory.Create(customHandler);
                StringBuilder stringBuilder = UriParametresBuilder(requestObject, functionToCall);


                response = await client.GetAsync(String.Format(Contants.RequestApiFormat,
                                                        ConfigurationManager.AppSettings["mhEndpointRecepcion"].ToString(),
                                                        stringBuilder.ToString()));

                if(response != null)
                {
                    responseString = await response.Content.ReadAsStringAsync();
                }
            }
            catch(Exception ex)
            {
                responseString = ex.Message.ToString();
            }

            return responseString;
        }

        public async Task<string> Post(object request, string functionToCall)
        {
            HttpResponseMessage response;
            string responseString = string.Empty;

            try
            {
                customHandler = new CustomDelegatingHandler();

                HttpClient client = HttpClientFactory.Create(customHandler);
                StringBuilder stringBuilder = new StringBuilder(ConfigurationManager.AppSettings[functionToCall].ToString());

                response = await client.PostAsJsonAsync(stringBuilder.ToString(), request);

                if (response != null)
                {
                    responseString = await response.Content.ReadAsStringAsync();
                }
            }
            catch(Exception ex)
            {
                responseString = ex.Message.ToString();
            }

            return responseString;
        }

        public async Task<string> Put(object request, string functionToCall)
        {
            HttpResponseMessage response;
            string responseString = string.Empty;

            try
            {
                customHandler = new CustomDelegatingHandler();

                HttpClient client = HttpClientFactory.Create(customHandler);
                StringBuilder stringBuilder = new StringBuilder(ConfigurationManager.AppSettings[functionToCall].ToString());

                response = await client.PutAsJsonAsync(ConfigurationManager.AppSettings["mhEndpointRecepcion"].ToString(), request);

                if (response != null)
                {
                    responseString = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                responseString = ex.Message.ToString();
            }

            return responseString;
        }

        public async Task<string> Delete(Dictionary<string, string> requestObject, string functionToCall)
        {
            HttpResponseMessage response;
            string responseString = string.Empty;

            try
            {
                customHandler = new CustomDelegatingHandler();
                HttpClient client = HttpClientFactory.Create(customHandler);
                StringBuilder stringBuilder = UriParametresBuilder(requestObject, functionToCall);

                response = await client.DeleteAsync(String.Format(Contants.RequestApiFormat,
                                                        ConfigurationManager.AppSettings["mhEndpointRecepcion"].ToString(),
                                                        stringBuilder.ToString()));
                if (response != null)
                {
                    responseString = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                responseString = ex.Message.ToString();
            }
            return responseString;
        }

        private static StringBuilder UriParametresBuilder(Dictionary<string, string> requestObject, string functionToCall)
        {
            StringBuilder stringBuilder = new StringBuilder(ConfigurationManager.AppSettings[functionToCall].ToString());
            if (null != requestObject)
            {
                stringBuilder.Append("?");
                for (int index = 0; index < requestObject.Count; index++)
                {
                    KeyValuePair<string, string> pair = requestObject.ElementAt(index);

                    if (index < requestObject.Count - 1)
                    {
                        stringBuilder.Append(pair.Key).Append("=").Append(pair.Value).Append("&");
                    }
                    else
                    {
                        stringBuilder.Append(pair.Key).Append("=").Append(pair.Value);
                    }
                }
            }

            return stringBuilder;
        }
    }

}
