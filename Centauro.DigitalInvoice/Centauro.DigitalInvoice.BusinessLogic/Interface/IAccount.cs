using Centauro.DigitalInvoice.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Centauro.DigitalInvoice.BusinessLogic.Interface
{
    public interface IAccount
    {
        Account GetAccountById(string id);
        Account UpsertAccount(Account account);
        bool DeleteAccount(string id);
    }
}
