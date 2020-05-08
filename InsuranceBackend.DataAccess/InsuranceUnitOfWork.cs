using InsuranceBackend.Models;
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
            InsuranceSubline = new InsuranceSublineRepository(connectionString);
            InsuranceLineType = new InsuranceLineTypeRepository(connectionString);
            InsuranceLineClass = new InsuranceLineClassRepository(connectionString);
            InsuranceLineCommission = new InsuranceLineCommissionRepository(connectionString);
            RecordStatus = new RecordStatusRepository(connectionString);
            PolicyState = new PolicyStateRepository(connectionString);
            PolicyType = new PolicyTypeRepository(connectionString);
            Intermediary = new IntermediaryRepository(connectionString);
            Technician = new TechnicianRepository(connectionString);
            Salesman = new SalesmanRepository(connectionString);
            Policy = new PolicyRepository(connectionString);
            TaskAbout = new TaskAboutRepository(connectionString);
            Priority = new PriorityRepository(connectionString);
            Task = new TaskRepository(connectionString);
            ExternalSalesman = new ExternalSalesmanRepository(connectionString);
            TypeDigitalizedFile = new TypeDigitalizedFileRepository(connectionString);
            DigitalizedFile = new DigitalizedFileRepository(connectionString);
            BranchOffice = new BranchOfficeRepository(connectionString);
            ManagementType = new ManagementTypeRepository(connectionString);
            //Management = new ManagementRepository(connectionString);
            //ManagementTask = new ManagementTaskRepository(connectionString);
            PaymentMethod = new PaymentMethodRepository(connectionString);
            Financial = new FinancialRepository(connectionString);
            Management = new ManagementRepository(connectionString);
            ManagementPartner = new ManagementPartnerRespository(connectionString);
            ManagementState = new ManagementStateRespository(connectionString);
            ManagementExtra = new ManagementExtraRepository(connectionString);
            FinancialOption = new FinancialOptionRepository(connectionString);
            VehicleBodywork = new VehicleBodyWorkRepository(connectionString);
            VehicleBrand = new VehicleBrandRepository(connectionString);
            VehicleClass = new VehicleClassRepository(connectionString);
            VehicleReference = new VehicleReferenceRepository(connectionString);
            Vehicle = new VehicleRepository(connectionString);
            VehicleService = new VehicleServiceRepository(connectionString);
            Company = new CompanyRepository(connectionString);
            ProductCompany = new ProductCompanyRepository(connectionString);
            PolicyProduct = new PolicyProductRepository(connectionString);
            SalesmanProfile = new SalesmanProfileRepository(connectionString);
            MovementType = new MovementTypeRepository(connectionString);
            SalesmanParam = new SalesmanParamRepository(connectionString);
            Settings = new SettingsRepository(connectionString);
            Beneficiary = new BeneficiaryRepository(connectionString);
            PolicyBeneficiary = new PolicyBeneficiaryRepository(connectionString);
            PolicyInsured = new PolicyInsuredRepository(connectionString);
            Relationship = new RelationshipRepository(connectionString);
            PolicyOrder = new PolicyOrderRepository(connectionString);
            SystemProfile = new SystemProfileRepository(connectionString);
            UserProfile = new UserProfileRepository(connectionString);
            PolicyOrderDetail = new PolicyOrderDetailRepository(connectionString);
            PolicyFee = new PolicyFeeRepository(connectionString);
            SystemAudit = new SystemAuditRepository(connectionString);
            PaymentType = new PaymentTypeRepository(connectionString);
            Payment = new PaymentRepository(connectionString);
            PaymentDetail = new PaymentDetailRepository(connectionString);
            PolicyPromisoryNote = new PolicyPromisoryNoteRepository(connectionString);
            ManagementReason = new ManagementReasonRepository(connectionString);
            InterestDue = new InterestDueRepository(connectionString);
            PolicyOutlay = new PolicyOutlayRepository(connectionString);
            Settlement = new SettlementRepository(connectionString);
            PolicySettlement = new PolicySettlementRepository(connectionString);
            Petition = new PetitionRepository(connectionString);
            PetitionTrace = new PetitionTraceRepository(connectionString);
            PetitionState = new PetitionStateRepository(connectionString);
            CollectionMethod = new CollectionMethodRepository(connectionString);
            PetitionType = new PetitionTypeRepository(connectionString);
            CancellationReason = new CancellationReasonRepository(connectionString);
            Renewal = new RenewalRepository(connectionString);
            SubMenuProfilePerm = new SubMenuProfilePermRepository(connectionString);
            RestrictedPhones = new RestrictedPhonesRepository(connectionString);
            AllowedDomains = new AllowedDomainsRepository(connectionString);
            Onerous = new OnerousRepository(connectionString);
            PolicyReferences = new PolicyReferencesRepository(connectionString);
            TechnicalAsign = new TechnicalAssignRepository(connectionString);
            DeleteDocumentPermission = new DeleteDocumentPermissionRepository(connectionString);
            PolicyAuthorization = new PolicyAuthorizationRepository(connectionString);
            Fasecolda = new FasecoldaRepository(connectionString);
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
        public IInsuranceSublineRepository InsuranceSubline { get; private set; }
        public IInsuranceLineCommissionRepository InsuranceLineCommission { get; private set; }
        public IInsuranceLineTypeRepository InsuranceLineType { get; private set; }
        public IInsuranceLineClassRepository InsuranceLineClass { get; private set; }
        public IRecordStatusRepository RecordStatus { get; private set; }
        public IPolicyStateRepository PolicyState { get; private set; }
        public IPolicyTypeRepository PolicyType { get; private set; }
        public IIntermediaryRepository Intermediary { get; private set; }
        public ITechnicianRepository Technician { get; private set; }
        public ISalesmanRepository Salesman { get; private set; }
        public IPolicyRepository Policy { get; private set; }
        public ITaskAboutRepository TaskAbout { get; private set; }
        public IPriorityRepository Priority { get; private set; }
        public ITaskRepository Task { get; private set; }
        public IExternalSalesmanRepository ExternalSalesman { get; private set; }
        public ITypeDigitalizedFileRepository TypeDigitalizedFile { get; private set; }
        public IDigitalizedFileRepository DigitalizedFile { get; private set; }
        public IBranchOfficeRepository BranchOffice { get; private set; }
        public IManagementTypeRepository ManagementType { get; private set; }
        //public IManagementRepository Management { get; private set; }
        //public IManagementTaskRepository ManagementTask { get; private set; }
        public IPaymentMethodRepository PaymentMethod { get; private set; }
        public IFinancialRepository Financial { get; private set; }
        public IManagementRepository Management { get; private set; }
        public IManagementPartnerRepository ManagementPartner { get; private set; }
        public IManagementStateRepository ManagementState { get; private set; }
        public IManagementExtraRepository ManagementExtra { get; private set; }
        public IFinancialOptionRepository FinancialOption { get; private set; }
        public IVehicleBodyworkRepository VehicleBodywork { get; private set; }
        public IVehicleBrandRepository VehicleBrand { get; private set; }
        public IVehicleClassRepository VehicleClass { get; private set; }
        public IVehicleReferenceRepository VehicleReference { get; private set; }
        public IVehicleRepository Vehicle { get; private set; }
        public IVehicleServiceRepository VehicleService { get; private set; }
        public ICompanyRepository Company { get; private set; }
        public IProductCompanyRepository ProductCompany { get; private set; }
        public IPolicyProductRepository PolicyProduct { get; private set; }
        public ISalesmanProfileRepository SalesmanProfile { get; private set; }
        public IMovementTypeRepository MovementType { get; private set; }
        public ISalesmanParamRepository SalesmanParam { get; private set; }
        public ISettingsRepository Settings { get; private set; }
        public IBeneficiaryRepository Beneficiary { get; private set; }
        public IPolicyBeneficiaryRepository PolicyBeneficiary { get; private set; }
        public IPolicyInsuredRepository PolicyInsured { get; private set; }
        public IRelationshiprepository Relationship { get; private set; }
        public IPolicyOrderRepository PolicyOrder { get; private set; }
        public ISystemProfileRepository SystemProfile { get; private set; }
        public IUserProfileRepository UserProfile { get; private set; }
        public IPolicyOrderDetailRepository PolicyOrderDetail { get; private set; }
        public IPolicyFeeRepository PolicyFee { get; private set; }
        public ISystemAuditRepository SystemAudit { get; private set; }
        public IPaymentTypeRepository PaymentType { get; private set; }
        public IPaymentRepository Payment { get; private set; }
        public IPaymentDetailRepository PaymentDetail { get; private set; }
        public IPolicyPromisoryNoteRepository PolicyPromisoryNote { get; private set; }
        public IManagementReasonRepository ManagementReason { get; private set; }
        public IInterestDueRepository InterestDue { get; private set; }
        public IPolicyOutlayRepository PolicyOutlay { get; private set; }
        public ISettlementRepository Settlement { get; private set; }
        public IPolicySettlementRepository PolicySettlement { get; private set; }
        public IPetitionRepository Petition { get; private set; }
        public IPetitionTraceRepository PetitionTrace { get; private set; }
        public IPetitionStateRepository PetitionState { get; private set; }
        public ICollectionMethodRepository CollectionMethod { get; private set; }
        public IPetitionTypeRepository PetitionType { get; private set; }
        public ICancellationReasonRepository CancellationReason { get; private set; }
        public IRenewalRepository Renewal { get; private set; }
        public ISubMenuProfilePermRepository SubMenuProfilePerm { get; private set; }
        public IRestrictedPhonesRepository RestrictedPhones { get; private set; }
        public IAllowedDomainsRepository AllowedDomains { get; private set; }
        public IOnerousRepository Onerous { get; private set; }
        public IPolicyReferencesRepository PolicyReferences { get; private set; }
        public ITechnicalAsignRepository TechnicalAsign { get; private set; }
        public IDeleteDocumentPermissionRepository DeleteDocumentPermission { get; private set; }
        public IPolicyAuthorizationRepository PolicyAuthorization { get; private set; }
        public IFasecoldaRepository Fasecolda { get; private set; }
    }
}
