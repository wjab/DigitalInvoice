using Centauro.DigitalInvoice.BusinessLogic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Centauro.DigitalInvoice.BusinessLogic.Utilities
{
    public class Utils
    {
        public static string GetInnerMessage(Exception ex)
        {
            StringBuilder errorMessage = new StringBuilder(ex.Message);

            while (ex.InnerException != null)
            {
                errorMessage.Append("  " + ex.Message.ToString());
                ex = ex.InnerException;
            }
            errorMessage.Append("  " + ex.Message.ToString());

            return errorMessage.ToString();
        }


        public static void SetExceptionToResponse(ref GenericResponse response, Exception ex)
        {
            response.results = null;
            response.message = Utils.GetInnerMessage(ex);
            response.status = HttpStatusCode.InternalServerError;
        }
    }
}
