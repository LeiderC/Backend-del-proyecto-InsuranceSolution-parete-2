using InsuranceBackend.Repositories;

namespace InsuranceBackend.UnitOfWork
{
    public interface IUnitOfWork
    {
        ICityRepository City { get; }
        ICountryRepository Country { get; }
        ICustomerRepository Customer { get; }
        ICustomerTypeRepository CustomerType { get; }
        IEPSRepository EPS { get; }
        IGenderRepository Gender { get; }
        IIdentificationTypeRepository IdentificationType { get; }
        IMaritalStatusRepository MaritalStatus { get; }
        IOccupationRepository Occupation { get; }
        IPrefixRepository Prefix { get; }
        IStateRepository State { get; }
        IUserRepository User { get; }
        IInsuranceRepository Insurance { get; }
        IInsuranceLineRepository InsuranceLine { get; }
        IIntermediaryRepository Intermediary { get; }
        ITechnicianRepository Technician { get; }
        ISalesmanRepository Salesman { get; }
    }
}
