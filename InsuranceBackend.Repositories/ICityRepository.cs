using System.Collections.Generic;
using InsuranceBackend.Models;

namespace InsuranceBackend.Repositories
{
    public interface ICityRepository: IRepository<City>
    {
        IEnumerable<CityList> CityPagedList(int page, int rows);
        IEnumerable<City> CityByState(int idState);
    }
}
