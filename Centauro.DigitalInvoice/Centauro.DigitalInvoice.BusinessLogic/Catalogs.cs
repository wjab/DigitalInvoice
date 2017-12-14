using Centauro.DigitalInvoice.BusinessLogic.Enums;
using Centauro.DigitalInvoice.BusinessLogic.Model;
using Centauro.DigitalInvoice.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Centauro.DigitalInvoice.BusinessLogic
{
    public class Catalogs : ICatalogs
    {
        public List<GeneralObject> GetLocation(LocationDistribution locationType, int Provincia = 1, int Canton = 1, int Distrito = 1)
        {
            List<GeneralObject> location = new List<GeneralObject>();

            try
            {
                switch (locationType)
                {
                    case LocationDistribution.Provincia:
                        {
                            using (DigitalInvoiceEntities context = new DigitalInvoiceEntities())
                            {
                                location = (from l in context.Location
                                            select new GeneralObject()
                                            {
                                                code = l.IdProvincia,
                                                name = l.Provincia
                                            }).Distinct().ToList();
                            }
                        }
                        break;
                    case LocationDistribution.Canton:
                        {
                            using (DigitalInvoiceEntities context = new DigitalInvoiceEntities())
                            {
                                location = (from l in context.Location
                                            where l.IdProvincia == Provincia
                                            select new GeneralObject()
                                            {
                                                code = l.IdCanton,
                                                name = l.Canton
                                            }).Distinct().ToList();
                            }
                        }
                        break;
                    case LocationDistribution.Distrito:
                        {
                            using (DigitalInvoiceEntities context = new DigitalInvoiceEntities())
                            {
                                location = (from l in context.Location
                                            where l.IdProvincia == Provincia && l.IdCanton == Canton
                                            select new GeneralObject()
                                            {
                                                code = l.IdDistrito,
                                                name = l.Distrito
                                            }).Distinct().ToList();
                            }
                        }
                        break;
                    case LocationDistribution.Barrio:
                        {
                            using (DigitalInvoiceEntities context = new DigitalInvoiceEntities())
                            {
                                location = (from l in context.Location
                                            where l.IdProvincia == Provincia && l.IdCanton == Canton && l.IdDistrito == Distrito
                                            select new GeneralObject()
                                            {
                                                code = l.IdBarrio,
                                                name = l.Barrio
                                            }).Distinct().ToList();
                            }
                        }
                        break;
                }                
            }
            catch(Exception ex)
            {
                ex.Message.ToString();
            }

            return location;
        }
        
        public List<GeneralObject> GetDocumentTypes()
        {
            List<GeneralObject> DocumentTypeList = new List<GeneralObject>();

            try
            {
                using (DigitalInvoiceEntities context = new DigitalInvoiceEntities())
                {
                    DocumentTypeList = (from d in context.DocumentTypeAuthExon
                                        select new GeneralObject()
                                        {
                                            code = d.CodeId,
                                            name = d.DocumentType
                                        }).ToList();
                }
            }
            catch(Exception ex)
            {
                ex.Message.ToString();
            }

            return DocumentTypeList;
        }

        public List<GeneralObject> GetSaleCondition()
        {
            List<GeneralObject> DocumentTypeList = new List<GeneralObject>();

            try
            {
                using (DigitalInvoiceEntities context = new DigitalInvoiceEntities())
                {
                    DocumentTypeList = (from d in context.SaleCondition
                                        select new GeneralObject()
                                        {
                                            code = d.CodeId,
                                            name = d.SaleCondition1
                                        }).ToList();
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }

            return DocumentTypeList;
        }

        public List<GeneralObject> GetPaymentMethods()
        {
            List<GeneralObject> DocumentTypeList = new List<GeneralObject>();

            try
            {
                using (DigitalInvoiceEntities context = new DigitalInvoiceEntities())
                {
                    DocumentTypeList = (from d in context.PaymentMethods
                                        select new GeneralObject()
                                        {
                                            code = d.CodeId,
                                            name = d.PaymentMethods1
                                        }).ToList();
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }

            return DocumentTypeList;
        }

        public List<GeneralObject> GetCodeProductService()
        {
            List<GeneralObject> DocumentTypeList = new List<GeneralObject>();

            try
            {
                using (DigitalInvoiceEntities context = new DigitalInvoiceEntities())
                {
                    DocumentTypeList = (from d in context.CodeProductService
                                        select new GeneralObject()
                                        {
                                            code = d.CodeId,
                                            name = d.CodeProductService1
                                        }).ToList();
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }

            return DocumentTypeList;
        }

        public List<GeneralObject> GetIdentificationType()
        {
            List<GeneralObject> DocumentTypeList = new List<GeneralObject>();

            try
            {
                using (DigitalInvoiceEntities context = new DigitalInvoiceEntities())
                {
                    DocumentTypeList = (from d in context.IdentificationType
                                        select new GeneralObject()
                                        {
                                            code = d.CodeId,
                                            name = d.IdentificationType1
                                        }).ToList();
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }

            return DocumentTypeList;
        }

    }
}
