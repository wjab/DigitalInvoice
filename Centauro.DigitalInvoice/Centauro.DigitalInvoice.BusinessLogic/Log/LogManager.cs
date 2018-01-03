using Centauro.DigitalInvoice.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Centauro.DigitalInvoice.BusinessLogic.Log
{
    public class LogManager
    {
        public static void RegisterCallBackFailToDataBase(string request)
        {
            try
            {
                using (DigitalInvoiceEntities context = new DigitalInvoiceEntities())
                {
                    LogRequest logRequest = new LogRequest()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Request = request,
                        LogDate = DateTime.Now
                    };

                    context.LogRequest.Add(logRequest);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
