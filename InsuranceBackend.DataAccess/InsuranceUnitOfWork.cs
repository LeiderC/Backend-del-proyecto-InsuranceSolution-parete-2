using InsuranceBackend.Repositories;
using InsuranceBackend.UnitOfWork;

namespace InsuranceBackend.DataAccess
{
    public class InsuranceUnitOfWork : IUnitOfWork
    {
        public InsuranceUnitOfWork(string connectionString)
        {
            City = new CityRepository(connectionString);
            Country = new CountryRepository(connectionString);
            Customer = new CustomerRepository(connectionString);
            CustomerType = new CustomerTypeRepository(connectionString);
            EPS = new EPSRepository(connectionString);
            Gender = new GenderRepository(connectionString);
            IdentificationType = new IdentificationTypeRepository(connectionString);
            MaritalStatus = new MaritalStatusRepository(connectionString);
            Occupation = new OccupationRepository(connectionString);
            Prefix = new PrefixRepository(connectionString);
            State = new StateRepository(connectionString);
            User = new UserRepository(connectionString);
            Insurance = new InsuranceRepository(connectionString);
            InsuranceLine = new InsuranceLineRepository(connectionString);
            Intermediary = new IntermediaryRepository(connectionString);
            Technician = new TechnicianRepository(connectionString);
            Salesman = new SalesmanRepository(connectionString);
        }
        public ICityRepository City { get; private set; }
        public ICountryRepository Country { get; private set; }
        public ICustomerRepository Customer { get; private set; }
        public ICustomerTypeRepository CustomerType { get; private set; }
        public IEPSRepository EPS { get; private set; }
        public IGenderRepository Gender { get; private set; }
        public IIdentificationTypeRepository IdentificationType { get; private set; }
        public IMaritalStatusRepository MaritalStatus { get; private set; }
        public IOccupationRepository Occupation { get; private set; }
        public IPrefixRepository Prefix { get; private set; }
        public IStateRepository State { get; private set; }
        public IUserRepository User { get; private set; }
        public IInsuranceRepository Insurance { get; private set; }
        public IInsuranceLineRepository InsuranceLine { get; private set; }
        public IIntermediaryRepository Intermediary { get; private set; }
        public ITechnicianRepository Technician { get; private set; }
        public ISalesmanRepository Salesman { get; private set; }
    }
}
