using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.DataAccess
{
    public class PetitionStateRepository : Repository<PetitionState>, IPetitionStateRepository
    {
        public PetitionStateRepository(string connectionString) : base(connectionString)
        {
        }
    }
}