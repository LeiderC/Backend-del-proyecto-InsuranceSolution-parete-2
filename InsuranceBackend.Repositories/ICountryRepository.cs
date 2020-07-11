using System.Collections.Generic;
using InsuranceBackend.Models;

namespace InsuranceBackend.Repositories
{
    public interface ICountryRepository: IRepository<Country>
    {
        IEnumerable<CountryList> CountryPagedList(int page, int rows);
    }
}
