using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceBackend.WebApi.Models
{
    public class GetPaginatedPaymentSearchTerm
    {
        public int Page { get; set; }
        public int Rows { get; set; }
        public string PaymentType { get; set; }
        public int PaymentNumber { get; set; }
        public int IdCustomer { get; set; }
        public int IdPolicy { get; set; }
    }
}
