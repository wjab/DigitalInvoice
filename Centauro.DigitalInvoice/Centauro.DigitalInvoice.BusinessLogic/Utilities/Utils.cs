using Centauro.DigitalInvoice.BusinessLogic.Model;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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
                ex = ex.InnerException;
                errorMessage.Append("  " + ex.Message);
            }

            return errorMessage.ToString();
        }

        public static string ConcatElements(IList<IError> errorList)
        {
            StringBuilder value = new StringBuilder();
            foreach (var item in errorList)
            {
                value.Append(item + " | ");
            }

            return value.ToString();
        }

        public static void SetExceptionToResponse(ref GenericResponse response, Exception ex)
        {
            response.results = null;
            response.message = Utils.GetInnerMessage(ex);
            response.status = HttpStatusCode.InternalServerError;
        }

        public static int DifferenceInSeconds(DateTime initialDate, DateTime finalDate)
        {
            double diffInSeconds = (finalDate - initialDate).TotalSeconds;
            int seconds = 100;

            try
            {
                if (diffInSeconds >= 0)
                {
                    seconds = Convert.ToInt32(Math.Round(diffInSeconds, MidpointRounding.AwayFromZero));
                }
            }
            catch (Exception ex)
            { ex.Message.ToString(); }

            return seconds;
        }

        public static string GenerateQRCode(string link)
        {
            try
            {
                QrEncoder qr_encoder = new QrEncoder();
                QrCode code = null;
                qr_encoder.TryEncode(link, out code);
                GraphicsRenderer renderer = new GraphicsRenderer(new FixedCodeSize(350, QuietZoneModules.Zero), Brushes.Black, Brushes.White);
                MemoryStream ms = new MemoryStream();
                renderer.WriteToStream(code.Matrix,ImageFormat.Png, ms);
                return Convert.ToBase64String(ms.GetBuffer());
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return null;
            }
        }
    }
}
