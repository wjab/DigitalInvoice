using Centauro.DigitalInvoice.BusinessLogic.Constants;
using Centauro.DigitalInvoice.BusinessLogic.Interface;
using Centauro.DigitalInvoice.BusinessLogic.InterfaceImp;
using Centauro.DigitalInvoice.BusinessLogic.Model;
using Centauro.DigitalInvoice.BusinessLogic.Utilities;
using Centauro.DigitalInvoice.DataBase;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
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
        private static ConcurrentDictionary<string, AuthenticationResponse> oAuthDataDictionary;
        private static object syncLockoAuthData = new object();

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

        public Authentication()
        {
            oAuthDataDictionary = new ConcurrentDictionary<string, AuthenticationResponse>();
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
            string stringResponse = string.Empty;

            if(authenticationResponse.expires_in == 0 || DateTime.Now > datetime.AddSeconds(authenticationResponse.expires_in))
            {
                #region Gets tokenObject
                try
                {
                    client = new HttpCustomClient();
                    Dictionary<string, string> authenticationDictionary = new Dictionary<string, string>
                    {
                        {Constants.Constants.grant_type, Constants.Constants.password},
                        {Constants.Constants.client_id, Constants.Constants.apiStag},
                        {Constants.Constants.client_secret, string.Empty},
                        {Constants.Constants.scope, string.Empty},
                        {Constants.Constants.username, ConfigurationManager.AppSettings[Constants.Constants.userATV]},
                        {Constants.Constants.password, ConfigurationManager.AppSettings[Constants.Constants.passwordATV]}
                    };

                    stringResponse = await client.SendRequest(authenticationDictionary, Constants.Constants.tokenEndpoint);

                    if (!string.IsNullOrEmpty(stringResponse))
                    {
                        authenticationResponse = JsonConvert.DeserializeObject<AuthenticationResponse>(stringResponse);
                    }
                    else
                    {
                        throw new Exception("Fallo comunicación con el servidor de autenticación OAuth del Ministerio de Hacienda");
                    }
                }
                catch (Exception ex)
                {
                    ex.Message.ToString();
                }
                #endregion
            }

            return authenticationResponse;
        }

        

        public async Task<string> AuthenticationMHById(string accountId)
        {
            string valid_token = string.Empty;
            AuthenticationResponse authResponse, newAuthResponse;
            IAccount accountImp = new AccountImp();
            Account accountInfo;
            bool resultget;

            try
            {
                authResponse = new AuthenticationResponse();
                accountInfo = accountImp.GetAccountById(accountId);

                lock (syncLockoAuthData)
                {
                    resultget = oAuthDataDictionary.TryGetValue(accountId, out authResponse);
                }                 

                if (!resultget)
                {
                    #region Agrega el registro por primera vez
                    
                    authResponse = await AuthenticationMH(accountInfo.userATV, accountInfo.passwordATV);
                    if (authResponse != null)
                    {
                        authResponse.currentDateTime = DateTime.Now;
                        lock (syncLockoAuthData)
                        {
                            oAuthDataDictionary.GetOrAdd(accountId, authResponse);
                        }
                        valid_token = authResponse.access_token;
                    }
                    else
                    {
                        throw new Exception("La autenticación con el Oauth de Hacienda falló");
                    }
                    
                    #endregion
                }
                else
                {
                    if(authResponse.expires_in > Utils.DifferenceInSeconds(authResponse.currentDateTime, DateTime.Now))
                    {
                        valid_token = authResponse.access_token;
                    }
                    else
                    {
                        newAuthResponse = await AuthenticationMH(accountInfo.userATV, accountInfo.passwordATV);

                        if (newAuthResponse != null)
                        {
                            lock (syncLockoAuthData)
                            {
                                oAuthDataDictionary.AddOrUpdate(accountId, authResponse, (k, v) => newAuthResponse);
                            }
                            valid_token = newAuthResponse.access_token;
                        }
                        else
                        {
                            throw new Exception("La autenticación con el Oauth de Hacienda falló");
                        }
                    }
                }                
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return valid_token;

        }

        private async Task<AuthenticationResponse> AuthenticationMH(string user, string password)
        {
            HttpCustomClient client;
            string stringResponse = string.Empty;

            if (authenticationResponse.expires_in == 0 || DateTime.Now > datetime.AddSeconds(authenticationResponse.expires_in))
            {
                #region Gets tokenObject
                try
                {
                    client = new HttpCustomClient();
                    Dictionary<string, string> authenticationDictionary = new Dictionary<string, string>
                    {
                        {Constants.Constants.grant_type, Constants.Constants.password},
                        {Constants.Constants.client_id, Constants.Constants.apiStag},
                        {Constants.Constants.client_secret, string.Empty},
                        {Constants.Constants.scope, string.Empty},
                        {Constants.Constants.username, user},
                        {Constants.Constants.password, password}
                    };

                    stringResponse = await client.SendRequest(authenticationDictionary, Constants.Constants.tokenEndpoint);

                    if (!string.IsNullOrEmpty(stringResponse))
                    {
                        authenticationResponse = JsonConvert.DeserializeObject<AuthenticationResponse>(stringResponse);
                    }
                    else
                    {
                        throw new Exception("Fallo comunicación con el servidor de autenticación OAuth del Ministerio de Hacienda");
                    }
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
