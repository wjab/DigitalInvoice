using Centauro.DigitalInvoice.BusinessLogic.Constants;
using Centauro.DigitalInvoice.BusinessLogic.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Centauro.DigitalInvoice.BusinessLogic.Auth
{    
    public class Authentication
    {
        private static Authentication instance;

        public static Authentication Instance()
        {
            if(instance == null)
            {
                instance = new Authentication();
            }

            return instance;
        }

        public static async Task<string> AUthenticationMH()
        {
            string respuesta = string.Empty;

            HttpCutomClient client = new HttpCutomClient();
            var newObject = new
            {
                access_token = ConfigurationManager.AppSettings[Contants.tokenEndpoint],
                grant_type = Contants.password,
                client_id = Contants.apiStag,
                client_secret = string.Empty,
                scope = string.Empty,
                username = ConfigurationManager.AppSettings[Contants.userATV],
                password = ConfigurationManager.AppSettings[Contants.passwordATV]
            };

            var x = await client.Post(newObject, Contants.tokenEndpoint);

            /*HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, ConfigurationManager.AppSettings[Contants.tokenEndpoint]);
            request.Content = new FormUrlEncodedContent(authenticationDictionary);
            request.Method = HttpMethod.Post;

            CustomDelegatingHandler customHandler = new CustomDelegatingHandler();
            HttpClient client = HttpClientFactory.Create(customHandler);

            var rest = client.PostAsJsonAsync(ConfigurationManager.AppSettings["mhEndpointRecepcion"].ToString(), request);

            if(rest != null)
            {
                respuesta = await rest.Result.Content.ReadAsStringAsync();
            }*/

            return respuesta;
        }

        public static async Task<AuthenticationResponse> AUthenticationMH_1()
        {
            string respuesta = string.Empty;
            HttpResponseMessage responseMessage;

            using (HttpClient client = new HttpClient())
            {
                Dictionary<string, string> authenticationDictionary = new Dictionary<string, string>
                {
                    {Contants.access_token, ConfigurationManager.AppSettings[Contants.tokenEndpoint]},
                    {Contants.grant_type, Contants.password},
                    {Contants.client_id, Contants.apiStag},
                    {Contants.client_secret, string.Empty},
                    {Contants.scope, string.Empty},
                    {Contants.username, ConfigurationManager.AppSettings[Contants.userATV]},
                    {Contants.password, ConfigurationManager.AppSettings[Contants.passwordATV]}
                };

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, ConfigurationManager.AppSettings[Contants.tokenEndpoint]);
                request.Content = new FormUrlEncodedContent(authenticationDictionary);

                responseMessage = await client.SendAsync(request);
                respuesta = await responseMessage.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<AuthenticationResponse>(respuesta);

            }


        }



    }
}
