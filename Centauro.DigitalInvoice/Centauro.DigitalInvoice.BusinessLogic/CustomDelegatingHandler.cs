using Centauro.DigitalInvoice.BusinessLogic.Auth;
using Centauro.DigitalInvoice.BusinessLogic.Constants;
using Centauro.DigitalInvoice.BusinessLogic.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
                AuthenticationResponse authenticationResponse = await Authentication.Instance().AuthenticationMH();
                request.Headers.Authorization = new AuthenticationHeaderValue(Constants.Constants.auth_header_bearer, authenticationResponse.access_token);
                
                response = await base.SendAsync(request, cancellationToken);

                if(response.StatusCode == HttpStatusCode.Forbidden || response.StatusCode == HttpStatusCode.NotFound || response.StatusCode == HttpStatusCode.BadRequest)
                {
                    throw new Exception(response.StatusCode.ToString());
                }
            }
            catch(Exception ex)
            {
                ex.Message.ToString();
                throw ex;
            }

            return response;
        }
    }

    public class HttpCustomClient
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


                response = await client.GetAsync(string.Format(Constants.Constants.RequestApiFormat_2,
                                                        ConfigurationManager.AppSettings[Constants.Constants.mhEndpoint].ToString(),
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

        public async Task<string> Get(string parameter, string functionToCall)
        {
            HttpResponseMessage response;
            string responseString = string.Empty;

            try
            {
                customHandler = new CustomDelegatingHandler();
                HttpClient client = HttpClientFactory.Create(customHandler);

                response = await client.GetAsync(string.Format(Constants.Constants.RequestApiFormat_3,
                                                        ConfigurationManager.AppSettings[Constants.Constants.mhEndpoint].ToString(),
                                                        ConfigurationManager.AppSettings[functionToCall],
                                                        parameter));

                if (response != null)
                {
                    responseString = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                responseString = ex.Message.ToString();
                throw ex;
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
                throw ex;
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

                response = await client.PutAsJsonAsync(ConfigurationManager.AppSettings[Constants.Constants.mhEndpoint].ToString(), request);

                if (response != null)
                {
                    responseString = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                responseString = ex.Message.ToString();
                throw ex;
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

                response = await client.DeleteAsync(string.Format(Constants.Constants.RequestApiFormat_2,
                                                        ConfigurationManager.AppSettings[Constants.Constants.mhEndpoint].ToString(),
                                                        stringBuilder.ToString()));
                if (response != null)
                {
                    responseString = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                responseString = ex.Message.ToString();
                throw ex;
            }
            return responseString;
        }

        public async Task<string> SendRequest(Dictionary<string, string> requestObject, string functionToCall)
        {
            HttpResponseMessage response;
            string responseString = string.Empty;

            try
            {
                customHandler = new CustomDelegatingHandler();

                HttpClient client = new HttpClient(); //HttpClientFactory.Create(customHandler);
                StringBuilder stringBuilder = new StringBuilder(ConfigurationManager.AppSettings[functionToCall].ToString());
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, ConfigurationManager.AppSettings[Constants.Constants.tokenEndpoint]);
                request.Content = new FormUrlEncodedContent(requestObject);

                response = await client.SendAsync(request);

                if (response != null)
                {
                    responseString = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                responseString = ex.Message.ToString();
                throw ex;
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
