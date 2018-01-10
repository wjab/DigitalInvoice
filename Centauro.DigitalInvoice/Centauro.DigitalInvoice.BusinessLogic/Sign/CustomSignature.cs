using Centauro.DigitalInvoice.BusinessLogic.Interface;
using Centauro.DigitalInvoice.BusinessLogic.InterfaceImp;
using Centauro.DigitalInvoice.DataBase;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Centauro.DigitalInvoice.BusinessLogic.Sign
{
    public class CustomSignature
    {
        public static void SignXML(string fechaEmision, ref XmlDocument xml, string accountId = "",  X509Certificate2 certificate = null)
        {
            XmlDocument xmlDocSignature;
            DataBase.Account accountData;
            X509Certificate2 cert;
            IAccount accountImp;
            string singIdentifier, xmlSignature;
            SHA256 sha256;
            SHA1 sha1;

            try
            {
                accountImp = new AccountImp();
                xmlDocSignature = new XmlDocument();
                sha256 = new SHA256CryptoServiceProvider();
                sha1 = new SHA1CryptoServiceProvider();

                singIdentifier = ConfigurationManager.AppSettings[Constants.Constants.SignIdentifier];

                if (certificate == null)
                {
                    if (!string.IsNullOrEmpty(accountId))
                    {
                        accountData = accountImp.GetAccountById(accountId);
                        if (accountData != null)
                        {
                            cert = new X509Certificate2(Convert.FromBase64String(accountData.certificate), accountData.certificatePIN.ToString());
                        }
                        else
                        {
                            throw new Exception(Constants.Constants.fail_Get_account_data);
                        }
                    }
                    else
                    {
                        throw new Exception(Constants.Constants.fail_CertificateInfo_incomplete);
                    }
                }
                else
                {
                    cert = certificate;
                }

                xmlDocSignature.Load(string.Format(Constants.Constants.RequestApiFormat_2, AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings[Constants.Constants.signTemplate]));

                #region Estructura para firma digital

                /************************************************
                 * 0 : id
                 * 1 : XML document base64
                 * 2 : (no se que es) Properties
                 * 3 : current datetime
                 * 4 : certificate data base64
                 * 5 : X509IssuerName
                 * 6 : X509SerialNumber
                 * 7 : Identifier
                 * 8 : Identifier sha256
                 ***********************************************/
                xmlSignature = xmlDocSignature.OuterXml;
                List<string> parameters = new List<string>();
                parameters.Add(Guid.NewGuid().ToString("N")); // 0
                parameters.Add(Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(xmlDocSignature.OuterXml)))); // 1
                parameters.Add("5JVZPTwN5Lj0sGTfFzaUeMKCo/xbCAj7fw6TLUFtZIk="); // 2
                parameters.Add(fechaEmision); // 3
                parameters.Add(Convert.ToBase64String(sha1.ComputeHash(Encoding.ASCII.GetBytes(cert.ToString(true))))); // 4
                parameters.Add(cert.IssuerName.ToString()); // 5
                parameters.Add(cert.SerialNumber); // 6
                parameters.Add(singIdentifier); // 7
                parameters.Add(Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(singIdentifier)))); // 8

                xmlSignature = string.Format(xmlSignature, parameters.ToArray());

                #endregion

                XmlDocumentFragment xFrag = xml.CreateDocumentFragment();
                xFrag.InnerXml = xmlSignature;
                xml.ChildNodes[0].AppendChild(xFrag);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
