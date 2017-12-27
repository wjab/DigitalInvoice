using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Centauro.DigitalInvoice.BusinessLogic.Utilities.Cryptography
{
    public class CryptoHelper
    {
        private CryptoHelper()
        {
        }

        public static string GetBase64SHA256(string inputString)
        {
            var inputBytes = Encoding.Default.GetBytes(inputString.ToCharArray());
            return GetBase64SHA256(inputBytes);
        }
        public static string GetBase64SHA256(byte[] inputBytes)
        {
            byte[] outputBytes = GetBytesSHA256(inputBytes);
            return Convert.ToBase64String(outputBytes);
        }
        public static byte[] GetBytesSHA256(byte[] inputBytes)
        {
            SHA256 cryptoServiceProvider = new SHA256CryptoServiceProvider();
            return cryptoServiceProvider.ComputeHash(inputBytes);
        }
        public static byte[] GetBytesSHA1(string inputString)
        {
            var inputBytes = Encoding.Default.GetBytes(inputString.ToCharArray());
            return GetBytesSHA256(inputBytes);
        }
    }
}
