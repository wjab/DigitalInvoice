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
    public class HttpCustomClient
    {
        public async Task<string> Get(Dictionary<string, string> requestObject, string functionToCall)
        {
            HttpResponseMessage response;
            string responseString = string.Empty;

            try
            {
                HttpClient client = new HttpClient();
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
                HttpClient client = new HttpClient();

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

        public async Task<ReceivedDigitalDocument> Post(object request, string functionToCall, string domain = "", string accountId = "", string oAuthToken = "")
        {
            ReceivedDigitalDocument InvoiceResponse = new ReceivedDigitalDocument();
            HttpResponseMessage response;

            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Constants.Constants.auth_header_bearer, 
                    string.IsNullOrEmpty(oAuthToken) ? await Authentication.Instance().AuthenticationMHById(accountId) : oAuthToken);

                StringBuilder stringBuilder = new StringBuilder();

                stringBuilder.Append(string.Format(Constants.Constants.RequestApiFormat_2,
                        (string.IsNullOrEmpty(domain) ? domain : ConfigurationManager.AppSettings[domain]),
                        ConfigurationManager.AppSettings[functionToCall].ToString()));            

                response = await client.PostAsJsonAsync(stringBuilder.ToString(), request);

                #region Evaluate response
                if (response != null)
                {
                    InvoiceResponse.reasonPhrase = response.ReasonPhrase;
                    InvoiceResponse.statusCode = Convert.ToInt32(response.StatusCode);

                    if (response.StatusCode != HttpStatusCode.Accepted)
                    {
                        InvoiceResponse.x_Error_Cause = Convert.ToString(response.Headers.GetValues(Constants.Constants.header_errorCause).FirstOrDefault());
                        #region Otros Datos Header - de momento no se usan
                        //headerContainer.headerResponse.Breadcrumbid = Convert.ToString(responseMessage.Headers.GetValues(Constants.Constants.header_groupId).FirstOrDefault());
                        //headerContainer.headerResponse.X_Ratelimit_Limit = Convert.ToInt32(responseMessage.Headers.GetValues(Constants.Constants.header_rateLimit).FirstOrDefault());
                        //headerContainer.headerResponse.X_Ratelimit_Remaining = Convert.ToInt32(responseMessage.Headers.GetValues(Constants.Constants.header_reteRemaining).FirstOrDefault());
                        //headerContainer.headerResponse.X_Ratelimit_Reset = Convert.ToInt32(responseMessage.Headers.GetValues(Constants.Constants.header_rateReset).FirstOrDefault());
                        #endregion
                    }
                    else
                    {
                        InvoiceResponse.fileLocation = response.Headers.GetValues(Constants.Constants.header_location).FirstOrDefault();                       
                    }
                }
                else
                {
                    throw new Exception(Constants.Constants.fail_send_electronic_invoice);
                }
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return InvoiceResponse;
        }

        public async Task<string> Put(object request, string functionToCall)
        {
            HttpResponseMessage response;
            string responseString = string.Empty;

            try
            {
                HttpClient client = new HttpClient();
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
                HttpClient client = new HttpClient();
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

                HttpClient client = new HttpClient();
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
