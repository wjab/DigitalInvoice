using Centauro.DigitalInvoice.BusinessLogic.Enums;
using Centauro.DigitalInvoice.BusinessLogic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Centauro.DigitalInvoice.BusinessLogic.Interface
{
    public interface ICatalogs
    {
        List<GeneralObject> GetLocation(LocationDistribution locationType, int Provincia = 1, int Canton = 1, int Distrito = 1);

        List<GeneralObject> GetDocumentTypes();

        List<GeneralObject> GetSaleCondition();

        List<GeneralObject> GetPaymentMethods();

        List<GeneralObject> GetCodeProductService();

        List<GeneralObject> GetIdentificationType();

        List<GeneralResponseObject> GetMeasureUnit();
    }
}
