using Centauro.DigitalInvoice.BusinessLogic.Constants;
using Centauro.DigitalInvoice.BusinessLogic.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Centauro.DigitalInvoice.BusinessLogic.Auth
{    
    public class Authentication
    {
        private static Authentication instance;
        private static AuthenticationResponse authenticationResponse;
        private static DateTime datetime;

        public static Authentication Instance()
        {
            if (instance == null)
            {
                instance = new Authentication();
                authenticationResponse = new AuthenticationResponse();
                datetime = DateTime.Now;
            }

            return instance;
        }

        public async Task<string> AuthenticationMH_Custom()
        {
            string respuesta = string.Empty;

            HttpCustomClient client = new HttpCustomClient();
            object newObject = new
            {
                access_token = ConfigurationManager.AppSettings[Constants.Constants.tokenEndpoint],
                grant_type = Constants.Constants.password,
                client_id = Constants.Constants.apiStag,
                client_secret = string.Empty,
                scope = string.Empty,
                username = ConfigurationManager.AppSettings[Constants.Constants.userATV],
                password = ConfigurationManager.AppSettings[Constants.Constants.passwordATV]
            };

            return await client.Post(newObject, Constants.Constants.tokenEndpoint);
        }

        public async Task<AuthenticationResponse> AuthenticationMH()
        {
            HttpCustomClient client;
            string respuesta = string.Empty;

            if(authenticationResponse.expires_in == 0 || DateTime.Now > datetime.AddSeconds(authenticationResponse.expires_in))
            {
                #region Gets tokenObject
                try
                {
                    client = new HttpCustomClient();
                    Dictionary<string, string> authenticationDictionary = new Dictionary<string, string>
                    {
                        {Constants.Constants.access_token, ConfigurationManager.AppSettings[Constants.Constants.tokenEndpoint]},
                        {Constants.Constants.grant_type, Constants.Constants.password},
                        {Constants.Constants.client_id, Constants.Constants.apiStag},
                        {Constants.Constants.client_secret, string.Empty},
                        {Constants.Constants.scope, string.Empty},
                        {Constants.Constants.username, ConfigurationManager.AppSettings[Constants.Constants.userATV]},
                        {Constants.Constants.password, ConfigurationManager.AppSettings[Constants.Constants.passwordATV]}
                    };

                    respuesta = await client.SendRequest(authenticationDictionary, Constants.Constants.tokenEndpoint);
                    authenticationResponse = JsonConvert.DeserializeObject<AuthenticationResponse>(respuesta);
                }
                catch (Exception ex)
                {
                    ex.Message.ToString();
                }
                #endregion
            }

            return authenticationResponse;
        }



    }
}
