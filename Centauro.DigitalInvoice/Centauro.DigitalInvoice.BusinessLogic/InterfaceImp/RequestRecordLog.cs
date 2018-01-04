using Centauro.DigitalInvoice.BusinessLogic.Interface;
using Centauro.DigitalInvoice.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Centauro.DigitalInvoice.BusinessLogic.InterfaceImp
{
    public class RequestRecordLog : IRequestRecordLog
    {
        public void RegisterRequestRecord(string clave, string accountId, string state)
        {
            try
            {
                RequestRecord newRecord = new RequestRecord()
                {
                    clave = clave,
                    accountId = accountId,
                    requestState = state,
                    sentDatetime = DateTime.Now
                };

                using (DigitalInvoiceEntities context = new DigitalInvoiceEntities())
                {
                    context.RequestRecord.Add(newRecord);
                    context.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                ex.Message.ToString();
            }
        }

        public void UpdateRequestRecord(RequestRecord requestRecord)
        {
            RequestRecord record;
            try
            {
                using (DigitalInvoiceEntities context = new DigitalInvoiceEntities())
                {
                    record = (from rr in context.RequestRecord
                              where rr.clave.Equals(requestRecord.clave) && rr.callBackReceived == false
                              select rr).FirstOrDefault();

                    if(record != null)
                    {
                        record.docDatetime = requestRecord.docDatetime;
                        record.indState = requestRecord.indState;
                        record.responseXML = requestRecord.responseXML;
                        record.callBacakDatetime = requestRecord.callBacakDatetime;
                        record.callBackReceived = true;
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
