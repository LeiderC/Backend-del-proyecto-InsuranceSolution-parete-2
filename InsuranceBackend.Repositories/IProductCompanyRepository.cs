using System.Collections.Generic;
using InsuranceBackend.Models;

namespace InsuranceBackend.Repositories
{
    public interface IProductCompanyRepository : IRepository<ProductCompany>
    {
        IEnumerable<ProductCompany> ProductByCompany(int idCompany);
    }
}
