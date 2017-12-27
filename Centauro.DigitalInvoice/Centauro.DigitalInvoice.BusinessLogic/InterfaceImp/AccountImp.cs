using Centauro.DigitalInvoice.BusinessLogic.Interface;
using Centauro.DigitalInvoice.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Centauro.DigitalInvoice.BusinessLogic.InterfaceImp
{
    public class AccountImp : IAccount
    {
        public Account GetAccountById(string id)
        {
            Account account = null;

            try
            {
                using (DigitalInvoiceEntities context = new DigitalInvoiceEntities())
                {
                    account = (from a in context.Account
                               where a.accountId.Equals(id) && a.enabled == true
                               select a).FirstOrDefault();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return account;
        }

        public Account UpsertAccount(Account account)
        {
            Account selectedAccount = null;

            try
            {
                using (DigitalInvoiceEntities context = new DigitalInvoiceEntities())
                {
                    selectedAccount = (from a in context.Account
                                       where a.accountId.Equals(account.accountId)
                                       select a).FirstOrDefault();

                    if (selectedAccount == null)
                    {
                        account.accountId = Guid.NewGuid().ToString();
                        account.enabled = true;
                        context.Account.Add(account);
                        selectedAccount = account;

                    }
                    else
                    {
                        selectedAccount.certificate = account.certificate;
                        selectedAccount.certificatePIN = account.certificatePIN;
                        selectedAccount.certificateExt = account.certificateExt;
                        selectedAccount.userATV = account.userATV;
                        selectedAccount.passwordATV = account.passwordATV;
                        selectedAccount.certificateName = account.certificateName;
                    }

                    context.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return selectedAccount;
        }

        public bool DeleteAccount(string id)
        {
            bool response = false;
            Account account = null;

            try
            {
                using (DigitalInvoiceEntities context = new DigitalInvoiceEntities())
                {
                    account = (from a in context.Account
                               where a.accountId.Equals(id)
                               select a).FirstOrDefault();

                    if(account != null)
                    {
                        account.enabled = false;
                        context.SaveChanges();
                    }
                    else
                    {
                        throw new Exception("Account profile can't be deleted");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return response;
        }
    }
}
