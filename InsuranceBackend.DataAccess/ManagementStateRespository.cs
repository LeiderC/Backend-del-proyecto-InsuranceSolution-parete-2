using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.DataAccess
{
    public class ManagementStateRespository : Repository<ManagementState>, IManagementStateRepository
    {
        public ManagementStateRespository(string connectionString) : base(connectionString)
        {
        }
    }
}