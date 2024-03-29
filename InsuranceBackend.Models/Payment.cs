﻿using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public string IdPaymentType { get; set; }
        public int Number { get; set; }
        public int IdUser { get; set; }
        public int IdCustomer { get; set; }
        public DateTime DatePayment { get; set; }
        public float TotalValue { get; set; }
        public string State { get; set; }
        public string IdWaytoPay { get; set; }
        public float AdditionalCharge { get; set; }
        public float Total { get; set; }
        //public string PaidDestination { get; set; }
        public string Observation { get; set; }
        public string ObservationRevoke { get; set; }
        public DateTime DateCreated { get; set; }
        public float TotalReceived { get; set; }
        public DateTime? DateRevoke { get; set; }
    }
}
