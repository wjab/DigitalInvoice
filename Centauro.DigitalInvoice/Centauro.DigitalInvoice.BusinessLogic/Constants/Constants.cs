using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Centauro.DigitalInvoice.BusinessLogic.Constants
{
    public class Constants
    {
        public const string access_token = "access_token";
        public const string grant_type = "grant_type";
        public const string client_id = "client_id";
        public const string apiStag = "api-stag";
        public const string client_secret = "client_secret";
        public const string scope = "scope";
        public const string username = "username";
        public const string password = "password";
        public const string tokenEndpoint = "tokenEndpoint";
        public const string userATV = "userATV";
        public const string passwordATV = "passwordATV";
        public const string certificatePIN = "certificatePIN";
        public const string serverDomain = "serverDomain";
        public const string urlS3Invoice = "urlS3Invoice";
        public const string xmlExtension = ".xml";


        public const string mhEndpoint = "mhEndpoint";
        public const string mhActionRecepcion = "mhActionRecepcion";
        public const string mhActionComprobantes = "mhActionComprobantes";



        public const string SignIdentifier = "SignIdentifier";
        public const string CallBackUrl = "CallBackUrl";



        public const string application_json = "application/json";


        /* 
         * Hacienda
         */
        public const string RequestApiFormat_2 = "{0}{1}";
        public const string RequestApiFormat_3 = "{0}{1}{2}";
        public const string auth_header_bearer = "bearer";
        public const string encoding_UTF_8 = "utf-8";

        public const string miliseconds = "miliseconds";
        public const string tokenExpiration = "tokenExpiration";


        #region Headers de respuesta de Hacienda

        public const string header_groupId = "Jmsxgroupid";
        public const string header_errorCause = "X-Error-Cause";
        public const string header_rateLimit = "X-Ratelimit-Limit";
        public const string header_reteRemaining = "X-Ratelimit-Remaining";
        public const string header_rateReset = "X-Ratelimit-Reset";
        public const string header_location = "Location";
        

        #endregion
                        
        #region Rutas XSD y templates

        public const string signTemplate = "signTemplate";
        public const string invoiceTemplate = "InvoiceTemplate";
        public const string invoiceTemplateCss = "InvoiceTemplateCss";

        public const string sampleXSD = "sampleXSD";
        public const string aceptaRechazaXSD = "aceptaRechazaXSD";
        public const string facturaElectronicaXSD = "facturaElectronicaXSD";
        public const string resumentPeriodoComprasXSD = "resumentPeriodoComprasXSD";
        public const string resumentPeriodoCompraVentasXSD = "resumentPeriodoCompraVentasXSD";
        public const string resumenPeriodoVentasXSD = "resumenPeriodoVentasXSD";

        #endregion

        #region Html 

        public const string lineaDetalle = @"<td id=""item"">{0}</td> <td class=""desc"">{1}</td> <td class=""unit"">{2}</td> <td class=""qty"">{3}</td> <td id=""item"">{4}</td>";
        public const string lineaTotales = @"<tr> <td colspan=""2""></td> <td colspan=""2"">Total servicios grabados</td> <td>{0}</td> </tr> <tr> <td colspan=""2""></td> <td colspan=""2"">Total servicios exentos</td> <td>{1}</td> </tr> <tr> <td colspan=""2""></td> <td colspan=""2"">Total mercanc&iacuteas grabadas</td> <td>{2}</td> </tr> <tr> <td colspan=""2""></td> <td colspan=""2"">Total mercanc&iacuteas exentas</td> <td>{3}</td> </tr> <tr> <td colspan=""2""></td> <td colspan=""2"">Total venta</td> <td>{4}</td> </tr> <tr> <td colspan=""2""></td> <td colspan=""2"">Total descuento</td> <td>{5}</td> </tr> <tr> <td colspan=""2""></td> <td colspan=""2"">Total impuesto</td> <td>{6}</td> </tr> <tr> <td colspan=""2""></td> <td colspan=""2"">Total comprobante</td> <td>{7}</td> </tr>";

        #endregion


        #region Api-Messages
        public const string fail_Get_account_data = "Fallo al obtener información de la cuenta";
        public const string fail_send_electronic_invoice = "Fallo al enviar la factura electrónica";
        public const string fail_get_document_from_hacienda = "Fallo al optener el Documento en el servidor del Ministerio de Hacienda";
        public const string fail_communication_oauth_hacienda = "Fallo comunicación con el servidor de autenticación OAuth del Ministerio de Hacienda";
        public const string fail_while_authenticate_against_oauth = "La autenticación con el Oauth de Hacienda falló";
        #endregion


    }
}
