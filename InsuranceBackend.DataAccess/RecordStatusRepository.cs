using InsuranceBackend.Models;
using InsuranceBackend.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceBackend.DataAccess
{
    public class RecordStatusRepository : Repository<RecordStatus>, IRecordStatusRepository
    {
        public RecordStatusRepository(string connectionString) : base(connectionString)
        {
        }
    }
}