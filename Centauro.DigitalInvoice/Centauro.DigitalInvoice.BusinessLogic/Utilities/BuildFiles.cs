using Centauro.DigitalInvoice.BusinessLogic.Interface;
using Centauro.DigitalInvoice.BusinessLogic.InterfaceImp;
using Centauro.DigitalInvoice.BusinessLogic.Model;
using Centauro.DigitalInvoice.DataBase;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuesPechkin;

namespace Centauro.DigitalInvoice.BusinessLogic.Utilities
{
    public class BuildFiles
    {
        #region HTML to PDF Converter
        static IConverter pdfConverter =
            new ThreadSafeConverter(
                new RemotingToolset<PdfToolset>(
                    new Win32EmbeddedDeployment(new TempFolderDeployment())
                )
            );
        #endregion

        /// <summary>
        /// Genera el hmtl que se utilizará para formar la factura
        /// </summary>
        /// <param name="payload"></param>
        /// <param name="base64QrImage"></param>
        /// <param name="base64LogoImage"></param>
        /// <returns></returns>
        public string BuildHtmlInvoice(FacturaElectronica payload, string base64QrImage, string base64LogoImage)
        {
            StringBuilder lineasDetalle;
            List<object> argsLinea, argsDocument;
            string htmlInvoice;

            try
            {
                #region Crear HTML
                                                
                argsDocument = new List<object>();
                argsDocument.Add(payload.NumeroConsecutivo);

                #region Datos Emisor
                lineasDetalle = new StringBuilder();
                argsLinea = new List<object>();
                htmlInvoice = File.ReadAllText(string.Format(Constants.Constants.RequestApiFormat_2, AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings[Constants.Constants.invoiceEmisor]));

                argsLinea.Add(payload.Emisor.Nombre);
                argsLinea.Add(payload.Emisor.Identificacion.Numero);
                argsLinea.Add(payload.Emisor.Ubicacion.Provincia + payload.Emisor.Ubicacion.Canton + payload.Emisor.Ubicacion.Distrito + payload.Emisor.Ubicacion.OtrasSenas);
                argsLinea.Add(string.Format(Constants.Constants.RequestApiFormat_2, payload.Emisor.Telefono.CodigoPais, payload.Emisor.Telefono.NumTelefono));
                argsLinea.Add(payload.Emisor.CorreoElectronico);

                lineasDetalle.Append(string.Format(htmlInvoice, argsLinea.ToArray()));

                #endregion

                #region Datos Factura

                argsLinea = new List<object>();
                htmlInvoice = File.ReadAllText(string.Format(Constants.Constants.RequestApiFormat_2, AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings[Constants.Constants.invoiceInfo]));

                argsLinea.Add(payload.NumeroConsecutivo);
                if(!base64LogoImage.Contains("data:image"))
                {
                    base64LogoImage = "data:image/png;base64," + base64LogoImage;
                }
                argsLinea.Add(base64LogoImage);
                argsLinea.Add(Convert.ToDateTime(payload.FechaEmision).ToString("dd/mm/yyyy"));
                argsLinea.Add(Convert.ToDateTime(payload.FechaEmision).ToString("hh:mm:ss tt"));
                argsLinea.Add(lineasDetalle.ToString());

                argsDocument.Add(string.Format(htmlInvoice, argsLinea.ToArray()));

                #endregion
                
                #region Datos Receptor

                argsLinea = new List<object>();
                htmlInvoice = File.ReadAllText(string.Format(Constants.Constants.RequestApiFormat_2, AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings[Constants.Constants.invoiceReceptor]));

                argsLinea.Add(payload.Receptor.Nombre);
                argsLinea.Add(string.IsNullOrEmpty(payload.Receptor.Identificacion.Numero) ? string.Empty : payload.Receptor.Identificacion.Numero);
                argsLinea.Add(string.Format(Constants.Constants.RequestApiFormat_2,
                                               (string.IsNullOrEmpty(payload.Receptor.Telefono.CodigoPais) ? string.Empty : payload.Receptor.Telefono.CodigoPais),
                                               (string.IsNullOrEmpty(payload.Receptor.Telefono.NumTelefono) ? string.Empty : payload.Receptor.Telefono.NumTelefono)));

                argsLinea.Add(payload.Receptor.CorreoElectronico);
                argsLinea.Add(payload.Receptor.Ubicacion.Provincia + payload.Receptor.Ubicacion.Canton + payload.Receptor.Ubicacion.Distrito + payload.Receptor.Ubicacion.OtrasSenas);                
                argsDocument.Add(string.Format(htmlInvoice, argsLinea.ToArray()));

                #endregion

                #region Lineas Detalle

                lineasDetalle = new StringBuilder();
                htmlInvoice = File.ReadAllText(string.Format(Constants.Constants.RequestApiFormat_2, AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings[Constants.Constants.invoiceLines]));

                foreach (var item in payload.DetalleServicio.LineaDetalle)
                {
                    argsLinea = new List<object>();
                    argsLinea.Add(item.Codigo.Count == 0 ? string.Empty : item.Codigo[0].Codigo);
                    argsLinea.Add(item.Detalle);
                    argsLinea.Add(item.PrecioUnitario);
                    argsLinea.Add(item.Cantidad);
                    argsLinea.Add(item.MontoTotal);

                    lineasDetalle.Append(string.Format(htmlInvoice, argsLinea.ToArray()));
                }
                argsDocument.Add(lineasDetalle.ToString());

                #endregion                                
                
                #region Totales

                lineasDetalle = new StringBuilder();
                htmlInvoice = File.ReadAllText(string.Format(Constants.Constants.RequestApiFormat_2, AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings[Constants.Constants.invoiceTotals]));

                lineasDetalle.Append(Utils.BuildInvoiceTotalLine(nameof(payload.ResumenFactura.TotalServGravados), payload.ResumenFactura.TotalServGravados, htmlInvoice));
                lineasDetalle.Append(Utils.BuildInvoiceTotalLine(nameof(payload.ResumenFactura.TotalServExentos), payload.ResumenFactura.TotalServExentos, htmlInvoice));
                lineasDetalle.Append(Utils.BuildInvoiceTotalLine(nameof(payload.ResumenFactura.TotalMercanciasGravadas), payload.ResumenFactura.TotalMercanciasGravadas, htmlInvoice));
                lineasDetalle.Append(Utils.BuildInvoiceTotalLine(nameof(payload.ResumenFactura.TotalMercanciasExentas), payload.ResumenFactura.TotalMercanciasExentas, htmlInvoice));
                lineasDetalle.Append(Utils.BuildInvoiceTotalLine(nameof(payload.ResumenFactura.TotalGravado), payload.ResumenFactura.TotalGravado, htmlInvoice));
                lineasDetalle.Append(Utils.BuildInvoiceTotalLine(nameof(payload.ResumenFactura.TotalExento), payload.ResumenFactura.TotalExento, htmlInvoice));
                lineasDetalle.Append(Utils.BuildInvoiceTotalLine(nameof(payload.ResumenFactura.TotalVenta), payload.ResumenFactura.TotalVenta, htmlInvoice));
                lineasDetalle.Append(Utils.BuildInvoiceTotalLine(nameof(payload.ResumenFactura.TotalDescuentos), payload.ResumenFactura.TotalDescuentos, htmlInvoice));
                lineasDetalle.Append(Utils.BuildInvoiceTotalLine(nameof(payload.ResumenFactura.TotalVentaNeta), payload.ResumenFactura.TotalVentaNeta, htmlInvoice));
                lineasDetalle.Append(Utils.BuildInvoiceTotalLine(nameof(payload.ResumenFactura.TotalImpuesto), payload.ResumenFactura.TotalImpuesto, htmlInvoice));
                lineasDetalle.Append(Utils.BuildInvoiceTotalLine(nameof(payload.ResumenFactura.TotalComprobante), payload.ResumenFactura.TotalComprobante, htmlInvoice));

                argsDocument.Add(lineasDetalle.ToString());
                #endregion

                #region Notas

                argsLinea = new List<object>();
                htmlInvoice = File.ReadAllText(string.Format(Constants.Constants.RequestApiFormat_2, AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings[Constants.Constants.invoiceNotes]));

                argsLinea.Add(string.IsNullOrEmpty(payload.Otros.OtroContenido.codigo) ? string.Empty : payload.Otros.OtroContenido.codigo);
                argsLinea.Add(string.IsNullOrEmpty(payload.Otros.OtroTexto.text) ? string.Empty : payload.Otros.OtroTexto.text);
                argsLinea.Add(base64QrImage);

                argsDocument.Add(string.Format(htmlInvoice, argsLinea.ToArray()));

                argsDocument.Add(string.Format(htmlInvoice, argsDocument.ToArray()));
                #endregion

                htmlInvoice = File.ReadAllText(string.Format(Constants.Constants.RequestApiFormat_2, AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings[Constants.Constants.invoiceTemplate]));
                htmlInvoice = string.Format(htmlInvoice, argsDocument.ToArray());

                #endregion
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return htmlInvoice;
        }

        /// <summary>
        /// COnvierte el HTML de la factura y genera el PDF
        /// </summary>
        /// <returns></returns>
        public string BuildPDFInvoice(string htmlInvoice)
        {
            string base64Pdf;

            try
            {
                #region Crear PDF

                IDocument htmlDocToPdf = new HtmlToPdfDocument
                {
                    Objects =
                                {
                                    new ObjectSettings
                                    {
                                        HtmlText = htmlInvoice,                                        
                                        ProduceExternalLinks = true,
                                        ProduceLocalLinks = true,
                                        WebSettings =
                                        {                                            
                                            PrintBackground = true,
                                            PrintMediaType = true,
                                            LoadImages = true,
                                            DefaultEncoding = Constants.Constants.encoding_UTF_8,
                                            UserStyleSheet = AppDomain.CurrentDomain.BaseDirectory + ConfigurationManager.AppSettings[Constants.Constants.invoiceTemplateCss]
                                        }
                                    }
                                }
                };


                base64Pdf = Convert.ToBase64String(pdfConverter.Convert(htmlDocToPdf));

                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return base64Pdf;
        }

        public string GetPDFInvoice(FacturaElectronica payload, string accountId)
        {
            DataBase.Account accountData;
            IAccount accountImp;
            string imageQR, htmlInvoice, pdfInvoice;

            try
            {
                accountImp = new AccountImp();
                accountData = accountImp.GetAccountById(accountId);
                if (accountData != null)
                {
                    imageQR = Utils.BuildLinkAndQRCode(payload.Clave, payload.Emisor.Identificacion.Numero);
                    htmlInvoice = BuildHtmlInvoice(payload, imageQR, accountData.logoImage);
                    pdfInvoice = BuildPDFInvoice(htmlInvoice);
                }
                else
                {
                    throw new Exception(Constants.Constants.fail_Get_account_data);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return pdfInvoice;
        }

    }
}
