﻿using InsuranceBackend.Models;
using System.Collections.Generic;


namespace InsuranceBackend.Repositories
{
    public interface IRenewalRepository : IRepository<Renewal>
    {
        DashboardRenewal DashboardRenewal(int idRenewal);
        IEnumerable<Renewal> RenewalByUser(int idUser);
    }
}
