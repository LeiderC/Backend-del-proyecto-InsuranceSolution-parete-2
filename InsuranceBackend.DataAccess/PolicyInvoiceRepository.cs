using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.DataAccess
{
    public class PolicyInvoiceRepository : Repository<PolicyInvoice>, IPolicyInvoiceRepository
    {
        public PolicyInvoiceRepository(string connectionString) : base(connectionString)
        {
        }
    }
}