using Centauro.DigitalInvoice.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Centauro.DigitalInvoice.BusinessLogic.Interface
{
    public interface IRequestRecordLog
    {
        void RegisterRequestRecord(string clave, string accoountId, string state);
        void UpdateRequestRecord(RequestRecord requestRecord);
    }
}
