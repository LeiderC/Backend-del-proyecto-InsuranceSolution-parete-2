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
        IInsuranceSublineRepository InsuranceSubline { get; }
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
        //IManagementRepository Management { get; }
        //IManagementTaskRepository ManagementTask { get; }
        IPaymentMethodRepository PaymentMethod { get; }
        IFinancialRepository Financial { get; }
        IManagementRepository Management { get; }
        IManagementExtraRepository ManagementExtra { get; }
        IManagementPartnerRepository ManagementPartner { get; }
        IManagementStateRepository ManagementState { get; }
        IFinancialOptionRepository FinancialOption { get; }
        IVehicleBodyworkRepository VehicleBodywork { get; }
        IVehicleBrandRepository VehicleBrand { get; }
        IVehicleClassRepository VehicleClass { get; }
        IVehicleReferenceRepository VehicleReference { get; }
        IVehicleRepository Vehicle { get; }
        IVehicleServiceRepository VehicleService { get; }
        ICompanyRepository Company { get; }
        IProductCompanyRepository ProductCompany { get; }
        IPolicyProductRepository PolicyProduct { get; }
        ISalesmanProfileRepository SalesmanProfile { get; }
        IMovementTypeRepository MovementType { get; }
        ISalesmanParamRepository SalesmanParam { get; }
        IExternalSalesmanParamRepository ExternalSalesmanParam { get; }
        ISettingsRepository Settings { get; }
        IBeneficiaryRepository Beneficiary { get; }
        IPolicyBeneficiaryRepository PolicyBeneficiary { get; }
        IPolicyInsuredRepository PolicyInsured { get; }
        IRelationshiprepository Relationship { get; }
        IPolicyOrderRepository PolicyOrder { get; }
        ISystemProfileRepository SystemProfile { get; }
        IUserProfileRepository UserProfile { get; }
        IPolicyOrderDetailRepository PolicyOrderDetail { get; }
        IPolicyFeeRepository PolicyFee { get; }
        IPolicyFeeFinancialRepository PolicyFeeFinancial { get; }
        ISystemAuditRepository SystemAudit { get; }
        IPaymentTypeRepository PaymentType { get; }
        IPaymentRepository Payment { get; }
        IPaymentDetailRepository PaymentDetail { get; }
        IPaymentDetailFinancialRepository PaymentDetailFinancial { get; }
        IPolicyPromisoryNoteRepository PolicyPromisoryNote { get; }
        IManagementReasonRepository ManagementReason { get; }
        IInterestDueRepository InterestDue { get; }
        IPolicyOutlayRepository PolicyOutlay { get; }
        ISettlementRepository Settlement { get; }
        IPolicySettlementRepository PolicySettlement { get; }
        IPetitionRepository Petition { get; }
        IPetitionTraceRepository PetitionTrace { get; }
        IPetitionStateRepository PetitionState { get; }
        ICollectionMethodRepository CollectionMethod { get; }
        IPetitionTypeRepository PetitionType { get; }
        ICancellationReasonRepository CancellationReason { get; }
        IRenewalRepository Renewal { get; }
        ISubMenuProfilePermRepository SubMenuProfilePerm { get; }
        IRestrictedPhonesRepository RestrictedPhones { get; }
        IAllowedDomainsRepository AllowedDomains { get; }
        IOnerousRepository Onerous { get; }
        IPolicyReferencesRepository PolicyReferences { get; }
        ITechnicalAsignRepository TechnicalAsign { get; }
        IDeleteDocumentPermissionRepository DeleteDocumentPermission { get; }
        IPolicyAuthorizationRepository PolicyAuthorization { get; }
        IPolicyAuthorizationDiscRepository PolicyAuthorizationDisc { get; }
        IPolicyAuthorizationFinanOwnProductRepository PolicyAuthorizationFinanOwnProduct { get; }
        IFasecoldaRepository Fasecolda { get; }
        IWaytoPayRepository WaytoPay { get; }
        IPaymentMethodThirdRepository PaymentMethodThird { get; }
        IPaymentThirdAccountRepository PaymentThirdAccount { get; }
        IPolicyPaymentThirdRepository PolicyPaymentThird { get; }
        IInsuranceLineGroupRepository InsuranceLineGroup { get; }
        IBusinessUnitRepository BusinessUnit { get; }
        IBusinessUnitDetailRepository BusinessUnitDetail { get; }
        ICustomerBusinessUnitRepository CustomerBusinessUnit { get; }
        IPolicyFeeProductRepository PolicyFeeProduct { get; }
        IPaymentDetailProductRepository PaymentDetailProduct { get; }
        IPolicyInvoiceRepository PolicyInvoice { get; }
        IExternalUserRepository ExternalUser { get; }
        IPolicyExternalUserRepository PolicyExternalUser { get; }
        IDigitalizedFileTypeRepository DigitalizedFileType { get; }
        IPolicyAttachedLastRepository PolicyAttachedLast { get; }
        IPolicyAttachedLastBeneficiaryRepository PolicyAttachedLastBeneficiary { get; }
        IPolicyAttachedLastInsuredRepository PolicyAttachedLastInsured { get; }
        IPolicyAttachedLastProductRepository PolicyAttachedLastProduct { get; }
        IPolicyAttachedLastReferencesRepository PolicyAttachedLastReferences { get; }
        IPolicyAttachedLastFeeRepository PolicyAttachedLastFee { get; }
        IPolicyAttachedLastFeeFinancialRepository PolicyAttachedLastFeeFinancial { get; }
        IPolicyBckRepository PolicyBck { get; }
        IPolicyInspectedRepository PolicyInspected { get; }
        IPolicyPendingRegistrationRepository PolicyPendingRegistration { get; }
        IVehicleInspectionRepository VehicleInspection { get; }
        IVehicleTypeRepository VehicleType { get; }
    }
}
