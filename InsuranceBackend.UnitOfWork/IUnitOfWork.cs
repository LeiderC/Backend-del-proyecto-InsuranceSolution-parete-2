﻿using InsuranceBackend.Repositories;

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
        IInsuranceLineCommissionRepository InsuranceLineCommission { get; }
        IInsuranceLineTypeRepository InsuranceLineType { get; }
        IInsuranceLineClassRepository InsuranceLineClass { get; }
        IRecordStatusRepository RecordStatus { get; }
        IPolicyStateRepository PolicyState { get; }
        IPolicyTypeRepository PolicyType { get; }
        IIntermediaryRepository Intermediary { get; }
        ITechnicianRepository Technician { get; }
        ISalesmanRepository Salesman { get; }
        IPolicyRepository Policy { get; }
        ITaskAboutRepository TaskAbout { get; }
        IPriorityRepository Priority { get; }
        ITaskRepository Task { get; }
        IExternalSalesmanRepository ExternalSalesman { get; }
        ITypeDigitalizedFileRepository TypeDigitalizedFile { get; }
        IDigitalizedFileRepository DigitalizedFile { get; }
        IBranchOfficeRepository BranchOffice { get; }
        IManagementTypeRepository ManagementType { get; }
        IManagementRepository Management { get; }
        IManagementTaskRepository ManagementTask { get; }
    }
}
