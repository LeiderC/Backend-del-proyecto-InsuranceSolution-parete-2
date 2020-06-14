using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.Models
{
    public class PaymentList : Payment
    {
        public int TotalRecords { get; set; }
        public string PaymentTypeDesc { get; set; }
        public string CustomerName { get; set; }
        public string UserName { get; set; }
        public string StateDesc { get; set; }
        public string WaytoPayDesc { get; set; }
        public List<PaymentDetailList> PaymentDetailLists { get; set; }
        public List<PaymentDetailFinancialList> PaymentDetailFinancialLists { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerCity { get; set; }
        public string CustomerIdentification { get; set; }
        public string SalesmanName { get; set; }
        public float Diference { get; set; }
    }
}