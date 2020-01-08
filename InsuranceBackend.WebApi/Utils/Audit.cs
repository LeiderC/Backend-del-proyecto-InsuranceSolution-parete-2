using InsuranceBackend.Models;
using InsuranceBackend.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceBackend.WebApi.Utils
{
    public static class Audit
    {
        public static void InsertAudit(IUnitOfWork unitOfWork, string process, int user, string action, string detail)
        {
            SystemAudit audit = new SystemAudit
            {
                Date = DateTime.Now,
                Process = process,
                IdUser = user,
                IdAction = action,
                Detail = detail
            };
            unitOfWork.SystemAudit.Insert(audit);
        }
    }
}
