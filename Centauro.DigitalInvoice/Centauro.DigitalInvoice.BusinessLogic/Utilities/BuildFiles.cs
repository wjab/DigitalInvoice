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

                htmlInvoice = File.ReadAllText(string.Format(Constants.Constants.RequestApiFormat_2, AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings[Constants.Constants.invoiceTemplate]));

                lineasDetalle = new StringBuilder();
                argsDocument = new List<object>();

                #region Datos Emisor
                argsDocument.Add(payload.Emisor.Nombre);
                argsDocument.Add(payload.Emisor.Ubicacion.OtrasSenas);
                argsDocument.Add(string.Format(Constants.Constants.RequestApiFormat_2, payload.Emisor.Telefono.CodigoPais, payload.Emisor.Telefono.NumTelefono));
                argsDocument.Add(payload.Emisor.CorreoElectronico);
                #endregion

                #region Datos Receptor
                argsDocument.Add(payload.Receptor.Nombre);
                argsDocument.Add(payload.Receptor.Ubicacion.OtrasSenas);
                argsDocument.Add(payload.Receptor.CorreoElectronico);
                #endregion

                argsDocument.Add(payload.Clave);
                argsDocument.Add(payload.FechaEmision);

                #region Footer Otros - Notas
                argsDocument.Add(payload.Otros.OtroContenido.codigo);
                argsDocument.Add(payload.Otros.OtroTexto.text);
                #endregion

                #region Lineas Detalle
                foreach (var item in payload.DetalleServicio.LineaDetalle)
                {
                    argsLinea = new List<object>();
                    argsLinea.Add(item.Codigo[0].Codigo);
                    argsLinea.Add(item.Detalle);
                    argsLinea.Add(item.PrecioUnitario);
                    argsLinea.Add(item.Cantidad);
                    argsLinea.Add(item.MontoTotal);

                    lineasDetalle.Append(string.Format(Constants.Constants.lineaDetalle, argsLinea.ToArray()));
                }
                argsDocument.Add(lineasDetalle.ToString());

                #endregion

                #region Resumen - Totales
                argsLinea = new List<object>();
                argsLinea.Add(payload.ResumenFactura.TotalServGravados);
                argsLinea.Add(payload.ResumenFactura.TotalServExentos);
                argsLinea.Add(payload.ResumenFactura.TotalMercanciasGravadas);
                argsLinea.Add(payload.ResumenFactura.TotalMercanciasExentas);
                argsLinea.Add(payload.ResumenFactura.TotalVenta);
                argsLinea.Add(payload.ResumenFactura.TotalDescuentos);
                argsLinea.Add(payload.ResumenFactura.TotalImpuesto);
                argsLinea.Add(payload.ResumenFactura.TotalComprobante);

                argsDocument.Add(string.Format(Constants.Constants.lineaTotales, argsLinea.ToArray()));
                #endregion

                argsDocument.Add(base64QrImage);
                argsDocument.Add(base64LogoImage);

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
                                            UserStyleSheet = string.Format( Constants.Constants.RequestApiFormat_2,
                                                                            AppDomain.CurrentDomain.BaseDirectory,
                                                                            ConfigurationManager.AppSettings[Constants.Constants.invoiceTemplateCss])
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
                    imageQR = Utils.BuildLinkAndQRCode(payload.Clave);
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
