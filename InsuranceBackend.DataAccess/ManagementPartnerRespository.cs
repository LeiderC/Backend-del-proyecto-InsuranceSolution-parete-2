using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.DataAccess
{
    public class ManagementPartnerRespository : Repository<ManagementPartner>, IManagementPartnerRepository
    {
        public ManagementPartnerRespository(string connectionString) : base(connectionString)
        {
        }
    }
}