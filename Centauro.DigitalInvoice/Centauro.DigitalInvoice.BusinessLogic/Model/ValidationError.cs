﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Centauro.DigitalInvoice.BusinessLogic.Model
{
    public class ValidationError : IError
    {
        public string errorMessage { get; set; }
    }
    
}
