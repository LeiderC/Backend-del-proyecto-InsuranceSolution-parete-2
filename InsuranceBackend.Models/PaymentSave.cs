﻿using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class PaymentSave
    {
        public Payment Payment { get; set; }
        public List<PaymentDetail> PaymentDetails { get; set; }
    }
}