using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.DataAccess
{
    public class InsuranceLineTypeRepository: Repository<InsuranceLineType>, IInsuranceLineTypeRepository
    {
        public InsuranceLineTypeRepository(string connectionString) : base(connectionString)
        {
        }
    }
}