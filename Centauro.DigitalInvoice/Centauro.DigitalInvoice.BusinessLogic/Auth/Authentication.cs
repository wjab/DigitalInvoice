using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Centauro.DigitalInvoice.BusinessLogic.Auth
{    
    public class Authentication
    {
        private static Authentication instance;

        public static Authentication Instance()
        {
            if(instance == null)
            {
                instance = new Authentication();
            }

            return instance;
        }

        public void AUthenticationMH()
        {

        }



    }
}
