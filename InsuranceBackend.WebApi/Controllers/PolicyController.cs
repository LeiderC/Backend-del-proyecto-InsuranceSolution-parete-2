using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using AutoMapper;
using InsuranceBackend.Models;
using InsuranceBackend.UnitOfWork;
using InsuranceBackend.WebApi.Models;
using InsuranceBackend.WebApi.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceBackend.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/policy")]
    [Authorize]
    public class PolicyController : Controller
    {
        Notification.EmailUtils emailUtils = new Notification.EmailUtils();
        private readonly IUnitOfWork _unitOfWork;
        private readonly NotificationMetadata _notificationMetadata;
        private readonly List<MovementType> movementTypes;
        private readonly IMapper _mapper;
        public PolicyController(IUnitOfWork unitOfWork, NotificationMetadata notificationMetadata, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _notificationMetadata = notificationMetadata;
            movementTypes = _unitOfWork.MovementType.GetList().ToList();
            _mapper = mapper;
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetById(int id)
        {
            try
            {
                PolicyList policy = _unitOfWork.Policy.PolicyListById(id);
                return Ok(policy);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("GetPolicyAttCol/{id:int}")]
        public IActionResult GetPolicyAttCol(int id)
        {
            try
            {
                PolicyList policy = _unitOfWork.Policy.PolicyAttColListById(id);
                return Ok(policy);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("GetPolicyAttColByPolicyOrder/{id:int}")]
        public IActionResult GetPolicyAttColByPolicyOrder(int id)
        {
            try
            {
                PolicyList policy = _unitOfWork.Policy.PolicyAttColListByIdPolicyOrder(id);
                return Ok(policy);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("GetPolicyHeader")]
        public IActionResult GetAllPolicyHeader()
        {
            try
            {
                return Ok(_unitOfWork.Policy.PolicyHeader());
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("GetPolicyHeaderByCustomer/{idCustomer:int}")]
        public IActionResult GetPolicyHeaderByCustomer(int idCustomer)
        {
            try
            {
                return Ok(_unitOfWork.Policy.PolicyHeaderByIdCustomer(idCustomer));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("GetPolicyExternalUserByCustomer/{idCustomer:int}")]
        public IActionResult GetPolicyExternalUserByCustomer(int idCustomer)
        {
            try
            {
                return Ok(_unitOfWork.Policy.PolicyExternalUserByCustomer(idCustomer));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("GetPolicyAttached/{idPolicy:int}")]
        public IActionResult GetAllPolicyAttached(int idPolicy)
        {
            try
            {
                return Ok(_unitOfWork.Policy.PolicyAttached(idPolicy));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("GetPolicyAttachedByPolicyAttLast/{idPolicy:int}")]
        public IActionResult GetPolicyAttachedByPolicyAttLast(int idPolicy)
        {
            try
            {
                return Ok(_unitOfWork.Policy.PolicyAttachedByPolicyAttLast(idPolicy));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("GetPolicyCancelByPolicyParent/{idPolicy:int}")]
        public IActionResult GetPolicyCancelByPolicyParent(int idPolicy)
        {
            try
            {
                return Ok(_unitOfWork.Policy.PolicyCancelByPolicyParent(idPolicy));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("GetPolicyHeader")]
        public IActionResult GetPolicyHeader([FromBody] GetPolicyHeader request)
        {
            try
            {
                return Ok(_unitOfWork.Policy.PolicyHeader(request.IdInsurance, request.IdInsuranceLine, request.IdInsuranceSubline, request.Number));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("GetPolicyBySearchTerms")]
        public IActionResult GetPolicyBySearchTerms([FromBody] GetPaginatedPolicySearchTerm request)
        {
            try
            {
                int idUser = 0;
                if (request.FindByUserPolicyOrder)
                    idUser = int.Parse(User.Claims.Where(c => c.Type.Equals(ClaimTypes.PrimarySid)).FirstOrDefault().Value);
                if (request.IdUser > 0)
                    idUser = request.IdUser;
                return Ok(_unitOfWork.Policy.PolicyPagedListSearchTerms(request.Identification, request.Name, request.Number, request.IdCustomer, idUser, request.IsOrder, request.Page, request.Rows, request.StateOrder, request.IdPolicyState));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("GetPolicyOrderReport")]
        public IActionResult GetPolicyOrderReport([FromBody] GetPolicyOrderReport request)
        {
            try
            {
                return Ok(_unitOfWork.Policy.PolicyOrderReport(request.Page, request.Rows, request.IdUser, request.StartDate, request.EndDate, request.All));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("GetPolicyOrderReportConsolidated")]
        public IActionResult GetPolicyOrderReportConsolidated([FromBody] GetPolicyOrderReport request)
        {
            try
            {
                return Ok(_unitOfWork.Policy.PolicyOrderReportConsolidated(request.StartDate.Value, request.EndDate.Value));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("GetPolicyCustomerBySearchTerms")]
        public IActionResult GetPolicyCustomerBySearchTerms([FromBody] GetPaginatedSearchTermType request)
        {
            try
            {
                int idUser = 0;
                idUser = int.Parse(User.Claims.Where(c => c.Type.Equals(ClaimTypes.PrimarySid)).FirstOrDefault().Value);
                SystemUser systemUser = _unitOfWork.User.GetById(idUser);
                UserProfile userProfile = _unitOfWork.UserProfile.UserProfileByUser(idUser);
                SystemProfile systemProfile = _unitOfWork.SystemProfile.GetById(userProfile.IdProfile);
                int idSalesman = 0;
                if (systemProfile.ValidateCustomer)
                    idSalesman = systemUser.IdSalesman;
                return Ok(_unitOfWork.Policy.PolicyCustomerPagedListSearchTerms(request.Type, request.SearchTerm, request.Page, request.Rows, idSalesman));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("GetPolicyCustomerBySearchTermsOnlyPolicy")]
        public IActionResult GetPolicyCustomerBySearchTermsOnlyPolicy([FromBody] GetPaginatedSearchTermType request)
        {
            try
            {
                return Ok(_unitOfWork.Policy.PolicyCustomerPagedListSearchTermsOnlyPolicy(request.Type, request.SearchTerm, request.Page, request.Rows));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("GetPolicyCustomerBySearchTermsOnlyOrder")]
        public IActionResult GetPolicyCustomerBySearchTermsOnlyOrder([FromBody] GetPaginatedSearchTermType request)
        {
            try
            {
                return Ok(_unitOfWork.Policy.PolicyCustomerPagedListSearchTermsOnlyOrder(request.Type, request.SearchTerm, request.Page, request.Rows));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("GetPolicyByIdPolicyOrder")]
        public IActionResult GetCustomerByIdentification([FromBody] GetSearchTerm request)
        {
            try
            {
                return Ok(_unitOfWork.Policy.PolicyByIdPolicyOrder(int.Parse(request.SearchTerm), request.IsOrder));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("GetPolicyPromisoryNotePaged")]
        public IActionResult GetPolicyPromisoryNotePaged([FromBody] GetPaginatedPolicyPromisoryNote request)
        {
            try
            {
                return Ok(_unitOfWork.Policy.PolicyPromisoryNotePagedList(request.StartDate.Date, request.EndDate.Date, request.Page, request.Rows, request.IdFinancial));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("GetPolicyOutlayPaged")]
        public IActionResult GetPolicyOutlayPaged([FromBody] GetPaginatedPolicyPromisoryNote request)
        {
            try
            {
                return Ok(_unitOfWork.Policy.PolicyOutlayPagedList(request.StartDate.Date, request.EndDate.Date, request.Page, request.Rows));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("GetPolicyCommissionPaged")]
        public IActionResult GetPolicyCommissionPaged([FromBody] GetPaginatedPolicyCommission request)
        {
            try
            {
                return Ok(_unitOfWork.Policy.PolicyCommissionPagedList(request.InsuranceId, request.StartDate.Date, request.EndDate.Date, request.Page, request.Rows));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("GetPolicyCommission")]
        public IActionResult GetPolicyCommission([FromBody] GetPaginatedPolicyCommission request)
        {
            try
            {
                return Ok(_unitOfWork.Policy.PolicyCommissionList(request.InsuranceId, request.StartDate.Date, request.EndDate.Date));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("GetPolicyCommissionSalesman")]
        public IActionResult GetPolicyCommissionSalesman([FromBody] GetPolicyCommissionSalesman request)
        {
            try
            {
                return Ok(_unitOfWork.Policy.PolicyCommissionSalesmanList(request.IdSalesman, request.StartDate, request.EndDate));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("GetPolicyCommissionExternalSalesman")]
        public IActionResult GetPolicyCommissionExternalSalesman([FromBody] GetPolicyCommissionSalesman request)
        {
            try
            {
                return Ok(_unitOfWork.Policy.PolicyCommissionExternalSalesmanList(request.IdSalesman, request.StartDate, request.EndDate));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("GetPolicyPortfolioReport")]
        public IActionResult GetPolicyPortfolioReport([FromBody] GetPolicyPortfolio request)
        {
            try
            {
                return Ok(_unitOfWork.Policy.PolicyPortfolioReportList(request.StartDate, request.EndDate, request.IdInsurance, request.IdCustomer, request.License));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("GetPolicyPaymentThirdParties")]
        public IActionResult GetPolicyPaymentThirdParties([FromBody] GetPolicyPaymentThirdParties request)
        {
            try
            {
                List<PolicyList> lst = _unitOfWork.Policy.PolicyPaymentThirdParties(request.StartDate, request.EndDate, request.IdInsurance,
                    request.IdFinancial, request.Type, request.Paid, request.IdPaymentMethodThird, request.IdAccountThird).ToList();
                if (request.Type.Equals("A"))
                {
                    lst.RemoveAll(P => P.IdPaymentMethod.Equals("4") && P.IdPaymentType != "L3");
                }
                return Ok(lst);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("GetPolicyVehicleInspected")]
        public IActionResult GetPolicyVehicleInspected([FromBody] GetPolicyPaymentThirdParties request)
        {
            try
            {
                List<PolicyList> lst = _unitOfWork.Policy.PolicyVehicleInspected(request.StartDate, request.EndDate, request.Inspected).ToList();
                return Ok(lst);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("GetPolicyVehiclePendingRegistration")]
        public IActionResult GetPolicyVehiclePendingRegistration([FromBody] GetPolicyPaymentThirdParties request)
        {
            try
            {
                List<PolicyList> lst = _unitOfWork.Policy.PolicyVehiclePendingRegistration(request.StartDate, request.EndDate, request.Register).ToList();
                return Ok(lst);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        [HttpPost]
        [Route("GetPolicyPaymentIncome")]
        public IActionResult GetPolicyPaymentIncome([FromBody] GetPolicyPaymentIncome request)
        {
            try
            {
                List<PolicyList> lst = _unitOfWork.Policy.PolicyPaymentIncome(request.StartDate, request.EndDate).ToList();
                return Ok(lst);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("GetPolicyPaymentAccountReceivable")]
        public IActionResult GetPolicyPaymentAccountReceivable([FromBody] GetPolicyPaymentIncome request)
        {
            try
            {
                List<PolicyList> lst = _unitOfWork.Policy.PolicyPaymentAccountReceivable(request.StartDate, request.EndDate).ToList();
                return Ok(lst);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("SetPolicySettlement")]
        public IActionResult SetPolicySettlement([FromBody] PolicySettlementSave request)
        {
            Settlement settlement = new Settlement();
            if (!ModelState.IsValid)
                return BadRequest();
            using (var transaction = new TransactionScope())
            {
                try
                {
                    Settings settings = _unitOfWork.Settings.GetList().FirstOrDefault();
                    settings.SettlementNumber += 1;
                    _unitOfWork.Settings.Update(settings);
                    //Primero debemos crear la liquidación (settlement)
                    string idUser = User.Claims.Where(c => c.Type.Equals(ClaimTypes.PrimarySid)).FirstOrDefault().Value;
                    request.Settlement.Number = settings.SettlementNumber;
                    request.Settlement.IdUserSettle = int.Parse(idUser);
                    request.Settlement.CreationDate = DateTime.Now;
                    request.Settlement.DateSettle = DateTime.Now;
                    double total = request.Policies.Sum(p => p.Commission);
                    request.Settlement.Total = total;
                    int idSettle = _unitOfWork.Settlement.Insert(request.Settlement);
                    settlement = request.Settlement;
                    settlement.Id = idSettle;
                    //Debemos recorrer las pólizas e insertar en PolicySettlement
                    foreach (var item in request.Policies)
                    {
                        PolicySettlement policySettlement = new PolicySettlement
                        {
                            IdPolicy = item.Id,
                            IdSettle = idSettle
                        };
                        _unitOfWork.PolicySettlement.Insert(policySettlement);
                    }
                    transaction.Complete();
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    return StatusCode(500, "Internal server error: " + ex.Message);
                }
            }
            return Ok(settlement);
        }

        [HttpPost]
        [Route("SetPolicyPaymentThird")]
        public IActionResult SetPolicyPaymentThird([FromBody] PolicyPaymentThird policyPaymentThird)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            try
            {
                int idUser = 0;
                idUser = int.Parse(User.Claims.Where(c => c.Type.Equals(ClaimTypes.PrimarySid)).FirstOrDefault().Value);
                policyPaymentThird.CreationDate = DateTime.Now;
                policyPaymentThird.PaymentDate = DateTime.Now;
                policyPaymentThird.ThirdPayDate = DateTime.Now;
                policyPaymentThird.IdUser = idUser;
                return Ok(_unitOfWork.PolicyPaymentThird.Insert(policyPaymentThird));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("RevokePromisoryNote")]
        public IActionResult RevokePromisoryNote([FromBody] Policy request)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            try
            {
                Policy policy = _unitOfWork.Policy.GetById(request.Id);
                if (policy == null)
                    return BadRequest("No se encuentra la póliza enviada");
                if (request.RevokePromisoryNote)
                    policy.RevokePromisoryNote = true;
                else
                    policy.RevokePromisoryNote = false;
                _unitOfWork.Policy.Update(policy);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] PolicySave policy)
        {
            int idPolicy = 0;
            if (!ModelState.IsValid)
                return BadRequest();
            using (var transaction = new TransactionScope())
            {
                try
                {
                    //Datos de vehículo
                    if (policy.Vehicle != null && !string.IsNullOrEmpty(policy.Vehicle.License))
                        policy.Policy.IdVehicle = vehicle(policy.Vehicle.License, policy.Policy.IdMovementType, policy.Vehicle);
                    else
                        policy.Policy.IdVehicle = null;
                    string idUser = User.Claims.Where(c => c.Type.Equals(ClaimTypes.PrimarySid)).FirstOrDefault().Value;
                    policy.Policy.IdUser = int.Parse(idUser);
                    if (policy.Policy.IsOrder)
                    {
                        if (policy.Policy.IsAttached)
                            policy.Policy.Number = "ORDEN-COL-" + policy.PolicyOrderId;
                        else
                            policy.Policy.Number = "ORDEN-" + policy.PolicyOrderId;
                    }
                    //Debemos bloquear si la placa ya existe en el mismo ramo
                    if (policy.Policy.IdMovementType != "4" && policy.Policy.IdMovementType != "5")
                    {
                        if (policy.Policy.IdInsuranceLine != null && !policy.Policy.IsHeader)
                        {
                            bool exist = _unitOfWork.Policy.PolicyDuplicate(0, policy.Policy.IdInsuranceLine.Value, policy.Policy.License, policy.Policy.IsOrder);
                            if (exist)
                            {
                                transaction.Dispose();
                                return StatusCode(400, "No se puede asegurar la misma placa más de una vez");
                            }
                        }
                    }
                    if (policy.Policy.IsAttached && !policy.Policy.IsAttachedOrder)
                    {
                        policy.Policy.IdInsurance = null;
                        policy.Policy.IdInsuranceLine = null;
                        policy.Policy.IdInsuranceSubline = null;
                        policy.Policy.Number = null;
                    }
                    idPolicy = _unitOfWork.Policy.Insert(policy.Policy);
                    //Productos propios
                    if (policy.Policy.IdMovementType != "4" && policy.Policy.IdMovementType != "5")
                        ownProducts(policy.PolicyProducts, idPolicy, false);
                    string insuredNames = "";
                    //Asegurados
                    if (policy.Policy.IdMovementType != "4" && policy.Policy.IdMovementType != "5")
                        insuredNames = insured(policy.PolicyInsured, idPolicy, false);
                    string beneficariesNames = "";
                    //Beneficiarios
                    if (policy.Policy.IdMovementType != "4" && policy.Policy.IdMovementType != "5")
                        beneficariesNames = beneficiaries(policy.PolicyBeneficiaries, idPolicy, false);
                    //Cuotas
                    if (policy.Policy.IdMovementType != "4" && policy.Policy.IdMovementType != "5")
                    {
                        fee(policy.PolicyFees, policy.Policy.IdPaymentMethod, policy.Policy.IsHeader, idPolicy,
                        policy.Policy.IsAttached, policy.Policy.TotalValue, policy.Policy.ExpiditionDate,
                        policy.Policy.StartDate);
                    }
                    //Cuota incial
                    if (policy.Policy.IdMovementType != "4" && policy.Policy.IdMovementType != "5")
                    {
                        int policyFeesProductCount = policy.PolicyFeesProduct != null ? policy.PolicyFeesProduct.Count : 0;
                        initialFee(policy.Policy.TotalInitialFee, policyFeesProductCount, idPolicy, policy.Policy.InitialFee,
                        policy.Policy.OwnProducts, policy.Policy.ExpiditionDate, policy.Policy.StartDate);
                    }
                    //Referencias
                    if (policy.Policy.IdMovementType != "4" && policy.Policy.IdMovementType != "5")
                        references(policy.PolicyReferences, idPolicy, false);
                    //Cuotas financiamiento p. propios
                    if (policy.Policy.IdMovementType != "4" && policy.Policy.IdMovementType != "5")
                        policyFeesProduct(policy.PolicyFeesProduct, idPolicy);
                    //Si es una orden se debe guardar
                    if (policy.Policy.IsOrder && !policy.Policy.IsAttached)
                    {
                        int idCustomer = policy.PolicyInsured != null && policy.PolicyInsured.Count > 0 ? policy.PolicyInsured[0].Id : 0;
                        string vehicleBrand = policy.Vehicle != null ? policy.Vehicle.Brand : "";
                        string vehicleClass = policy.Vehicle != null ? policy.Vehicle.Class : "";
                        policyManagement(policy.PolicyOrderId, idPolicy, policy.Policy, insuredNames, idUser, idCustomer,
                        vehicleBrand, vehicleClass, beneficariesNames, false);
                    }
                    else
                    {
                        if (policy.PolicyOrderId > 0 && !policy.Policy.IsAttached && !policy.Policy.IsHeader || (policy.Policy.IsAttached && policy.Policy.IsAttachedOrder))
                        {
                            int idCustomer = policy.PolicyInsured != null && policy.PolicyInsured.Count > 0 ? policy.PolicyInsured[0].Id : 0;
                            string vehicleBrand = policy.Vehicle != null ? policy.Vehicle.Brand : "";
                            string vehicleClass = policy.Vehicle != null ? policy.Vehicle.Class : "";
                            policyManagement(policy.PolicyOrderId, idPolicy, policy.Policy, insuredNames, idUser, idCustomer,
                            vehicleBrand, vehicleClass, beneficariesNames, false);
                        }
                    }
                    if (policy.Policy.IsAttached && policy.PolicyOrderId > 0 && !policy.Policy.IsAttachedOrder)
                    {
                        int idCustomer = policy.Policy.IdPolicyHolder != null ? policy.Policy.IdPolicyHolder.Value : 0;
                        string vehicleBrand = policy.Vehicle != null ? policy.Vehicle.Brand : "";
                        string vehicleClass = policy.Vehicle != null ? policy.Vehicle.Class : "";
                        policyManagement(policy.PolicyOrderId, idPolicy, policy.Policy, insuredNames, idUser, idCustomer,
                        vehicleBrand, vehicleClass, beneficariesNames, false);
                    }
                    //Documentos digitalizados
                    if (policy.PolicyOrderId > 0 && policy.Policy.IsHeader)
                    {
                        _unitOfWork.PolicyOrderDetail.Insert(new PolicyOrderDetail
                        {
                            IdPolicyOrder = policy.PolicyOrderId,

                            IdPolicy = idPolicy,
                            CreationDate = DateTime.Now,
                            State = "A"
                        });
                    }
                    if (policy.DigitalizedFiles != null && policy.DigitalizedFiles.Count > 0)
                    { //Nueva versión documentos digitales
                        foreach (var df in policy.DigitalizedFiles)
                        {
                            df.IdPolicyOrder = policy.PolicyOrderId;
                            _unitOfWork.DigitalizedFile.Insert(df);
                        }
                    }
                    //Debemos guardar la última transacción de certificado de póliza
                    if (policy.Policy.IsAttached)
                    {
                        PolicyAttachedLast policyAttachedLast = policy.Policy.IdPolicyParent.HasValue ? _unitOfWork.PolicyAttachedLast.PolicyAttachedLastByPolicy(policy.Policy.IdPolicyParent.Value) : null;
                        if (policyAttachedLast == null)
                        {
                            policyAttachedLast = new PolicyAttachedLast();
                            policyAttachedLast.EndDate = policy.Policy.EndDate;
                            policyAttachedLast.ExpiditionDate = policy.Policy.ExpiditionDate;
                            policyAttachedLast.IdExternalSalesMan = policy.Policy.IdExternalSalesMan;
                            policyAttachedLast.IdExternalUser = policy.Policy.IdExternalUser;
                            policyAttachedLast.IdPolicyHeader = policy.Policy.IdPolicyHeader;
                            policyAttachedLast.IdSalesMan = policy.Policy.IdSalesMan;
                            policyAttachedLast.OwnerIdentification = policy.Policy.OwnerIdentification;
                            policyAttachedLast.OwnerName = policy.Policy.OwnerName;
                            policyAttachedLast.TotalInitialFee = policy.Policy.TotalInitialFee;
                            policyAttachedLast.Id = 0;
                        }
                        policyAttachedLast.IdPolicyParent = idPolicy;
                        policyAttachedLast.Certificate = policy.Policy.Certificate;
                        policyAttachedLast.Contribution = policy.Policy.Contribution;

                        policyAttachedLast.FeeNumbers = policy.Policy.FeeNumbers;
                        policyAttachedLast.FeeValue = policy.Policy.FeeValue;

                        policyAttachedLast.IdFinancial = policy.Policy.IdFinancial;
                        policyAttachedLast.IdFinancialOption = policy.Policy.IdFinancialOption;
                        policyAttachedLast.IdMovementType = policy.Policy.IdMovementType;
                        policyAttachedLast.IdOnerous = policy.Policy.IdOnerous;
                        policyAttachedLast.IdPaymentMethod = policy.Policy.IdPaymentMethod;

                        policyAttachedLast.IdPolicyState = policy.Policy.IdPolicyState;
                        policyAttachedLast.IdUser = int.Parse(idUser);
                        policyAttachedLast.IdVehicle = policy.Policy.IdVehicle;
                        policyAttachedLast.InitialFee = policy.Policy.InitialFee;
                        policyAttachedLast.Inspected = policy.Policy.Inspected;
                        policyAttachedLast.InvoiceNumber = policy.Policy.InvoiceNumber;
                        policyAttachedLast.License = policy.Policy.License;
                        policyAttachedLast.Observation = policy.Policy.Observation;
                        policyAttachedLast.OwnProducts = policy.Policy.OwnProducts;
                        policyAttachedLast.Payday = policy.Policy.Payday;
                        policyAttachedLast.PendingRegistration = policy.Policy.PendingRegistration;
                        policyAttachedLast.ReqAuthorization = policy.Policy.ReqAuthorization;
                        policyAttachedLast.ReqAuthorizationFinancOwnProduct = policy.Policy.ReqAuthorizationFinancOwnProduct;
                        policyAttachedLast.StartDate = policy.Policy.StartDate;
                        policyAttachedLast.UpdateDate = DateTime.Now;

                        //Valores
                        switch (policy.Policy.IdMovementType)
                        {
                            case "1":
                            case "2":
                                policyAttachedLast.Iva = policy.Policy.Iva;
                                policyAttachedLast.NetValue = policy.Policy.NetValue;
                                policyAttachedLast.PremiumExtra = policy.Policy.PremiumExtra;
                                policyAttachedLast.PremiumValue = policy.Policy.PremiumValue;
                                policyAttachedLast.Runt = policy.Policy.Runt;
                                policyAttachedLast.TotalValue = policy.Policy.TotalValue;
                                break;
                            case "4":
                                policyAttachedLast.Iva = policyAttachedLast.Iva - policy.Policy.Iva;
                                policyAttachedLast.NetValue = policyAttachedLast.NetValue - policy.Policy.NetValue;
                                policyAttachedLast.PremiumExtra = policyAttachedLast.PremiumExtra - policy.Policy.PremiumExtra;
                                policyAttachedLast.PremiumValue = policyAttachedLast.PremiumValue - policy.Policy.PremiumValue;
                                policyAttachedLast.Runt = policyAttachedLast.Runt - policy.Policy.Runt;
                                policyAttachedLast.TotalValue = policyAttachedLast.TotalValue - policy.Policy.TotalValue;
                                break;
                            case "5":
                                policyAttachedLast.Iva = policyAttachedLast.Iva + policy.Policy.Iva;
                                policyAttachedLast.NetValue = policyAttachedLast.NetValue + policy.Policy.NetValue;
                                policyAttachedLast.PremiumExtra = policyAttachedLast.PremiumExtra + policy.Policy.PremiumExtra;
                                policyAttachedLast.PremiumValue = policyAttachedLast.PremiumValue + policy.Policy.PremiumValue;
                                policyAttachedLast.Runt = policyAttachedLast.Runt + policy.Policy.Runt;
                                policyAttachedLast.TotalValue = policyAttachedLast.TotalValue + policy.Policy.TotalValue;
                                break;
                        }
                        int idPolicyAttachedLast = policyAttachedLast.Id;
                        if (idPolicyAttachedLast > 0)
                            _unitOfWork.PolicyAttachedLast.Update(policyAttachedLast);
                        else
                            idPolicyAttachedLast = _unitOfWork.PolicyAttachedLast.Insert(policyAttachedLast);

                        policy.Policy.Id = idPolicy;
                        policy.Policy.IdPolicyAttachedLast = idPolicyAttachedLast;
                        _unitOfWork.Policy.Update(policy.Policy);
                        //Asegurados
                        insured(policy.PolicyInsured, idPolicyAttachedLast, policy.Policy.IsAttached);
                        //Beneficiarios
                        beneficiaries(policy.PolicyBeneficiaries, idPolicyAttachedLast, policy.Policy.IsAttached);
                        //Productos propios
                        ownProducts(policy.PolicyProducts, idPolicyAttachedLast, policy.Policy.IsAttached);
                        //Referencias
                        references(policy.PolicyReferences, idPolicy, policy.Policy.IsAttached);
                        //Cuotas
                        feeAttached(policy.PolicyFees, policy.Policy.IdPaymentMethod, idPolicy, policy.Policy.StartDate.Value, policy.Policy.TotalValue);
                        if (policy.Policy.IdMovementType.Equals("1") || policy.Policy.IdMovementType.Equals("2"))
                        {
                            _unitOfWork.PolicyOrderDetail.Insert(new PolicyOrderDetail
                            {
                                IdPolicyOrder = policy.PolicyOrderId,
                                IdPolicy = idPolicyAttachedLast,
                                CreationDate = DateTime.Now,
                                State = "A"
                            });
                        }
                    }

                    transaction.Complete();
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    return StatusCode(500, "Internal server error: " + ex.Message);
                }
            }

            return Ok(idPolicy);
        }

        private int vehicle(string license, string idMovementType, Vehicle vehicle)
        {
            //Validamos primero si existe, de ser así se debe actualizar la info
            Vehicle _vehicle = _unitOfWork.Vehicle.VehicleByLicense(license);
            int idVehicle = _vehicle != null ? _vehicle.Id : 0;
            if (idMovementType != "4" && idMovementType != "5")
            {
                if (_vehicle != null)
                {
                    _vehicle.Chassis = vehicle.Chassis;
                    _vehicle.CommercialValue = vehicle.CommercialValue;
                    _vehicle.Cylinder = vehicle.Cylinder;
                    _vehicle.Fasecolda = vehicle.Fasecolda;
                    _vehicle.IdVehicleBodywork = vehicle.IdVehicleBodywork;
                    _vehicle.IdVehicleBrand = vehicle.IdVehicleBrand;
                    _vehicle.IdVehicleClass = vehicle.IdVehicleClass;
                    _vehicle.IdVehicleReference = vehicle.IdVehicleReference;
                    _vehicle.IdVehicleService = vehicle.IdVehicleService;
                    _vehicle.Model = vehicle.Model;
                    _vehicle.PassengersNumber = vehicle.PassengersNumber;
                    _vehicle.Weight = vehicle.Weight;
                    _unitOfWork.Vehicle.Update(_vehicle);
                }
                else
                    idVehicle = _unitOfWork.Vehicle.Insert(vehicle);
            }
            return idVehicle;
        }

        private void ownProducts(List<PolicyProduct> policyProducts, int idPolicy, bool isAttached)
        {
            if (policyProducts != null && policyProducts.Count > 0)
            {
                if (isAttached)
                {
                    //Primero debemos eliminar los productos propios
                    _unitOfWork.PolicyAttachedLastProduct.DeletePolicyProductByPolicy(idPolicy);
                    foreach (var item in policyProducts)
                    {
                        PolicyAttachedLastProduct product = new PolicyAttachedLastProduct
                        {
                            Authorization = item.Authorization,
                            ExtraValue = item.ExtraValue,
                            FeeNumber = item.FeeNumber,
                            FeeValue = item.FeeValue,
                            IdPolicy = idPolicy,
                            IdProduct = item.IdProduct,
                            IVA = item.IVA,
                            TotalValue = item.TotalValue,
                            Value = item.Value
                        };
                        if (product.FeeValue > 0)
                        {
                            product.Value = 0;
                        }
                        _unitOfWork.PolicyAttachedLastProduct.Insert(product);
                    }
                }
                else
                {
                    //Primero debemos eliminar los productos propios
                    _unitOfWork.PolicyProduct.DeletePolicyProductByPolicy(idPolicy);
                    foreach (var item in policyProducts)
                    {
                        PolicyProduct product = item as PolicyProduct;
                        product.IdPolicy = idPolicy;
                        if (product.FeeValue > 0)
                        {
                            product.Value = 0;
                        }
                        _unitOfWork.PolicyProduct.Insert(product);
                    }
                }
            }
        }

        private string insured(List<Customer> policyInsured, int idPolicy, bool isAttached)
        {
            StringBuilder insuredNames = new StringBuilder();
            if (policyInsured != null && policyInsured.Count > 0)
            {
                if (isAttached)
                    _unitOfWork.PolicyAttachedLastInsured.DeletePolicyInsuredByPolicy(idPolicy);
                else
                    _unitOfWork.PolicyInsured.DeletePolicyInsuredByPolicy(idPolicy);
                foreach (var item in policyInsured)
                {
                    Customer i = _unitOfWork.Customer.GetById(item.Id);
                    string n = i.FirstName + (string.IsNullOrEmpty(i.MiddleName) ? "" : " " + i.MiddleName) + i.LastName + (string.IsNullOrEmpty(i.MiddleLastName) ? "" : " " + i.MiddleLastName);
                    if (insuredNames.Length > 0)
                        insuredNames.Append(", " + n);
                    else
                        insuredNames.Append(n);
                    if (isAttached)
                    {
                        PolicyAttachedLastInsured insured = new PolicyAttachedLastInsured
                        {
                            IdInsured = item.Id,
                            IdPolicy = idPolicy
                        };
                        _unitOfWork.PolicyAttachedLastInsured.Insert(insured);
                    }
                    else
                    {
                        PolicyInsured insured = new PolicyInsured
                        {
                            IdInsured = item.Id,
                            IdPolicy = idPolicy
                        };
                        _unitOfWork.PolicyInsured.Insert(insured);
                    }
                }
            }
            return insuredNames.ToString();
        }

        private string beneficiaries(List<BeneficiaryList> policyBeneficiaries, int idPolicy, bool isAttached)
        {
            StringBuilder beneficariesNames = new StringBuilder();
            if (policyBeneficiaries != null && policyBeneficiaries.Count > 0)
            {
                if (isAttached)
                    _unitOfWork.PolicyAttachedLastBeneficiary.DeletePolicyBeneficiaryByPolicy(idPolicy);
                else
                    _unitOfWork.PolicyBeneficiary.DeletePolicyBeneficiaryByPolicy(idPolicy);
                foreach (var item in policyBeneficiaries)
                {
                    string n = item.FirstName + (string.IsNullOrEmpty(item.MiddleName) ? "" : " " + item.MiddleName) + item.LastName + (string.IsNullOrEmpty(item.MiddleLastName) ? "" : " " + item.MiddleLastName);
                    if (beneficariesNames.Length > 0)
                        beneficariesNames.Append(", " + n);
                    else
                        beneficariesNames.Append(n);
                    //Debemos crear priemero el beneficiario si no existe
                    int idBeneficiary = 0;
                    Beneficiary ben = _unitOfWork.Beneficiary.BeneficiaryByIdentification(item.IdentificationNumber, item.IdIdentificationType);
                    if (ben != null)
                        idBeneficiary = ben.Id;
                    else
                    {
                        ben = item as Beneficiary;
                        idBeneficiary = _unitOfWork.Beneficiary.Insert(ben);
                    }
                    if (isAttached)
                    {
                        PolicyAttachedLastBeneficiary beneficiary = new PolicyAttachedLastBeneficiary
                        {
                            IdBeneficiary = idBeneficiary,
                            IdPolicy = idPolicy,
                            Percentage = item.Percentage
                        };
                        _unitOfWork.PolicyAttachedLastBeneficiary.Insert(beneficiary);
                    }
                    else
                    {
                        PolicyBeneficiary beneficiary = new PolicyBeneficiary
                        {
                            IdBeneficiary = idBeneficiary,
                            IdPolicy = idPolicy,
                            Percentage = item.Percentage
                        };
                        _unitOfWork.PolicyBeneficiary.Insert(beneficiary);
                    }
                }
            }
            return beneficariesNames.ToString();
        }

        private void references(List<PolicyReferences> policyReferences, int idPolicy, bool isAttached)
        {
            if (policyReferences != null && policyReferences.Count > 0)
            {
                if (isAttached)
                {
                    //Primero se debe eliminar las existentes
                    _unitOfWork.PolicyAttachedLastReferences.DeletePolicyReferenciesByPolicy(idPolicy);
                    foreach (var item in policyReferences)
                    {
                        PolicyAttachedLastReferences policyReference = new PolicyAttachedLastReferences
                        {
                            Name = item.Name,
                            Phone = item.Phone,
                            Mobile = item.Mobile,
                            Address = item.Address,
                            IdRelationship = item.IdRelationship,
                            IdPolicy = idPolicy
                        };
                        _unitOfWork.PolicyAttachedLastReferences.Insert(policyReference);
                    }
                }
                else
                {
                    //Primero se debe eliminar las existentes
                    _unitOfWork.PolicyReferences.DeletePolicyReferenciesByPolicy(idPolicy);
                    foreach (var item in policyReferences)
                    {
                        PolicyReferences policyReference = new PolicyReferences
                        {
                            Name = item.Name,
                            Phone = item.Phone,
                            Mobile = item.Mobile,
                            Address = item.Address,
                            IdRelationship = item.IdRelationship,
                            IdPolicy = idPolicy
                        };
                        _unitOfWork.PolicyReferences.Insert(policyReference);
                    }
                }
            }
        }

        private void fee(List<PolicyFee> policyFees, string idPaymentMethod, bool isHeader, int idPolicy,
        bool isAttached, double totalValue, DateTime expeditionDate, DateTime? startDate)
        {
            if (policyFees != null && policyFees.Count > 0 && idPaymentMethod != "4" && idPaymentMethod != "2" && !isHeader)
            {
                //Primero se debe eliminar las existentes
                _unitOfWork.PolicyFee.DeleteFeeByPolicy(idPolicy);
                foreach (var item in policyFees)
                {
                    PolicyFee policyFee = new PolicyFee
                    {
                        Number = item.Number,
                        IdPolicy = idPolicy,
                        Date = item.Date,
                        Value = item.Value,
                        DateInsurance = item.DateInsurance,
                        DatePayment = item.DatePayment
                    };
                    _unitOfWork.PolicyFee.Insert(policyFee);
                }
            }
            else
            {
                if (!isAttached)
                {
                    if (idPaymentMethod != "3") // Si no es financiado debemos crear por lo menos una cuota para poder hacer los pagos
                    {
                        _unitOfWork.PolicyFee.DeleteFeeByPolicy(idPolicy);
                        if (startDate.HasValue)
                        {
                            PolicyFee policyFee = new PolicyFee
                            {
                                Number = 1,
                                IdPolicy = idPolicy,
                                Date = startDate.Value,
                                Value = totalValue,
                                DatePayment = startDate.Value,
                            };
                            _unitOfWork.PolicyFee.Insert(policyFee);
                        }
                        else
                        {
                            PolicyFee policyFee = new PolicyFee
                            {
                                Number = 1,
                                IdPolicy = idPolicy,
                                Date = expeditionDate,
                                Value = totalValue,
                                DatePayment = expeditionDate,
                            };
                            _unitOfWork.PolicyFee.Insert(policyFee);
                        }
                    }
                    if (idPaymentMethod == "4" || idPaymentMethod == "2")
                    {
                        //Primero se debe eliminar las existentes
                        _unitOfWork.PolicyFeeFinancial.DeleteFeeByPolicy(idPolicy);
                        foreach (var item in policyFees)
                        {
                            PolicyFeeFinancial policyFeeFinancial = new PolicyFeeFinancial
                            {
                                Number = item.Number,
                                IdPolicy = idPolicy,
                                Date = item.Date,
                                Value = item.Value,
                                DateInsurance = item.DateInsurance,
                                DatePayment = item.DatePayment
                            };
                            _unitOfWork.PolicyFeeFinancial.Insert(policyFeeFinancial);
                        }
                    }
                }
            }
        }

        private void feeAttached(List<PolicyFee> policyFees, string idPaymentMethod, int idPolicy, DateTime startDate, double totalValue)
        {
            if (policyFees != null && policyFees.Count > 0 && idPaymentMethod != "4" && idPaymentMethod != "2")
            {
                //Primero se debe eliminar las existentes
                _unitOfWork.PolicyAttachedLastFee.DeleteFeeByPolicy(idPolicy);
                foreach (var item in policyFees)
                {
                    PolicyAttachedLastFee policyFee = new PolicyAttachedLastFee
                    {
                        Number = item.Number,
                        IdPolicy = idPolicy,
                        Date = item.Date,
                        Value = item.Value,
                        DateInsurance = item.DateInsurance,
                        DatePayment = item.DatePayment
                    };
                    _unitOfWork.PolicyAttachedLastFee.Insert(policyFee);
                }
            }
            else
            {
                if (idPaymentMethod != "3") // Si no es financiado debemos crear por lo menos una cuota para poder hacer los pagos
                {
                    PolicyFee policyFee = new PolicyFee
                    {
                        Number = 1,
                        IdPolicy = idPolicy,
                        Date = startDate,
                        Value = totalValue,
                        DatePayment = startDate,
                    };
                    _unitOfWork.PolicyFee.Insert(policyFee);
                }
                if (idPaymentMethod == "4" || idPaymentMethod == "2")
                {
                    //Primero se debe eliminar las existentes
                    _unitOfWork.PolicyAttachedLastFeeFinancial.DeleteFeeByPolicy(idPolicy);
                    foreach (var item in policyFees)
                    {
                        PolicyAttachedLastFeeFinancial policyFeeFinancial = new PolicyAttachedLastFeeFinancial
                        {
                            Number = item.Number,
                            IdPolicy = idPolicy,
                            Date = item.Date,
                            Value = item.Value,
                            DateInsurance = item.DateInsurance,
                            DatePayment = item.DatePayment
                        };
                        _unitOfWork.PolicyAttachedLastFeeFinancial.Insert(policyFeeFinancial);
                    }
                }
            }
        }
        private void initialFee(double totalInitialFee, int policyFeesProductCount, int idPolicy,
        double initialFee, double ownProducts, DateTime expeditionDate, DateTime? startDate)
        {
            if (totalInitialFee > 0 && policyFeesProductCount == 0)
            {
                _unitOfWork.PolicyFee.DeleteFeeByPolicyFeeNumber(idPolicy, 0);
                if (startDate.HasValue)
                {
                    PolicyFee policyFee = new PolicyFee
                    {
                        Number = 0,
                        IdPolicy = idPolicy,
                        Date = startDate.Value,
                        Value = initialFee,
                        ValueOwnProduct = ownProducts,
                        DatePayment = startDate.Value,
                    };
                    _unitOfWork.PolicyFee.Insert(policyFee);
                }
                else
                {
                    PolicyFee policyFee = new PolicyFee
                    {
                        Number = 0,
                        IdPolicy = idPolicy,
                        Date = expeditionDate,
                        Value = initialFee,
                        ValueOwnProduct = ownProducts,
                        DatePayment = expeditionDate,
                    };
                    _unitOfWork.PolicyFee.Insert(policyFee);
                }
            }
        }

        private void policyFeesProduct(List<PolicyFeeProduct> policyFeesProduct, int idPolicy)
        {
            if (policyFeesProduct != null && policyFeesProduct.Count > 0)
            {
                //Primero eliminamos las existentes
                _unitOfWork.PolicyFeeProduct.DeleteFeeProductByPolicy(idPolicy);
                foreach (var item in policyFeesProduct)
                {
                    PolicyFeeProduct policyFeeProduct = new PolicyFeeProduct
                    {
                        Date = item.Date,
                        DatePayment = item.DatePayment,
                        IdPolicy = idPolicy,
                        Number = item.Number,
                        Value = item.Value
                    };
                    _unitOfWork.PolicyFeeProduct.Insert(policyFeeProduct);
                }
            }
        }

        private void policyManagement(int idPolicyOrder, int idPolicy, Policy policy, string insuredNames, string idUser,
        int idCustomer, string vehicleBrand, string vehicleClass, string beneficariesNames, bool isEdit)
        {
            if (!isEdit)
            {
                if (idPolicyOrder > 0 && !policy.IsAttached && !policy.IsHeader || (policy.IsAttached && policy.IsAttachedOrder))
                {
                    _unitOfWork.PolicyOrderDetail.UpdateState(idPolicyOrder, "I");
                    Management taskR = _unitOfWork.Management.ManagementByPolicyOrder(idPolicyOrder, "T");
                    if (taskR != null)
                    {
                        taskR.State = "R";
                        _unitOfWork.Management.Update(taskR);
                    }
                }
                if (!policy.IsAttached)
                {
                    _unitOfWork.PolicyOrderDetail.Insert(new PolicyOrderDetail
                    {
                        IdPolicyOrder = idPolicyOrder,
                        IdPolicy = idPolicy,
                        CreationDate = DateTime.Now,
                        State = "A"
                    });
                }
            }
            //Debemos generar una tarea a un usuario para sistematizar (técnico)
            //Primero la gestión
            string policyHolder = "";
            string identification = "";
            MovementType mt = movementTypes.Where(m => m.Id.Equals(policy.IdMovementType)).FirstOrDefault();
            string movto = mt != null ? mt.Alias : "";
            Insurance ins = policy.IdInsurance != null ? _unitOfWork.Insurance.GetById(policy.IdInsurance.Value) : null;
            string insurance = ins != null ? ins.Description : "";
            InsuranceLine insl = policy.IdInsuranceLine != null ? _unitOfWork.InsuranceLine.GetById(policy.IdInsuranceLine.Value) : null;
            string insuranceLine = insl != null ? insl.Description : "";
            InsuranceSubline inssl = policy.IdInsuranceSubline != null ? _unitOfWork.InsuranceSubline.GetById(policy.IdInsuranceSubline.Value) : null;
            string insuranceSubline = inssl != null ? inssl.Description : "";
            string text = string.Empty;
            string subject = string.Empty;
            if (policy.IsAttached)
            {
                Policy polHeader = _unitOfWork.Policy.GetById(policy.IdPolicyHeader);
                ins = _unitOfWork.Insurance.GetById(polHeader.IdInsurance.Value);
                insurance = ins != null ? ins.Description : "";
                insl = _unitOfWork.InsuranceLine.GetById(polHeader.IdInsuranceLine.Value);
                insuranceLine = insl != null ? insl.Description : "";
                inssl = _unitOfWork.InsuranceSubline.GetById(polHeader.IdInsuranceSubline.Value);
                insuranceSubline = inssl != null ? inssl.Description : "";
            }
            if (idPolicyOrder > 0 && !policy.IsHeader)
            {
                string initText = "Creación ";
                if (isEdit)
                    initText = "Modificación ";
                if (string.IsNullOrEmpty(policy.License))
                {
                    text = initText + "Póliza #{0} {1} {2} {3} {4} , Tomador: {5} , Asegurado/s: {6}, Beneficiarios: {7}, Valor Prima: {8}, Total:{9} ";
                    subject = string.Format(text, policy.Number, movto, insurance, insuranceLine, insuranceSubline, policyHolder, insuredNames, beneficariesNames, String.Format("{0:0,0.0}", policy.PremiumValue), String.Format("{0:0,0.0}", policy.TotalValue));
                }
                else
                {
                    text = initText + "Póliza #{0} {1} {2} {3} {4} , Tomador: {5} , Asegurado/s: {6}, Beneficiarios: {7}, Placa: {8}, Valor Prima: {9}, Total:{10} ";
                    subject = string.Format(text, policy.Number, movto, insurance, insuranceLine, insuranceSubline, policyHolder, insuredNames, beneficariesNames, policy.License, String.Format("{0:0,0.0}", policy.PremiumValue), String.Format("{0:0,0.0}", policy.TotalValue));
                }
            }
            if (policy.IsOrder && !policy.IsAttached)
            {
                Customer h = _unitOfWork.Customer.GetById(policy.IdPolicyHolder.Value);
                policyHolder = h.FirstName + (string.IsNullOrEmpty(h.MiddleName) ? "" : " " + h.MiddleName) + h.LastName + (string.IsNullOrEmpty(h.MiddleLastName) ? "" : " " + h.MiddleLastName);
                string initText = "Creación ";
                if (isEdit)
                    initText = "Modificación ";
                if (string.IsNullOrEmpty(policy.License))
                {
                    text = initText + "Orden de Póliza #{0} {1} {2} {3} {4} , Tomador: {5} , Asegurado/s: {6} ";
                    subject = string.Format(text, idPolicyOrder, movto, insurance, insuranceLine, insuranceSubline, policyHolder, insuredNames);
                }
                else
                {
                    text = initText + "Orden de Póliza #{0} {1} {2} {3} {4} , Tomador: {5} , Asegurado/s: {6}, Placa: {7} ";
                    subject = string.Format(text, idPolicyOrder, movto, insurance, insuranceLine, insuranceSubline, policyHolder, insuredNames, policy.License);
                }
            }
            if (policy.IsOrder && policy.IsAttached && idPolicyOrder > 0 && !policy.IsAttachedOrder)
            {
                policyHolder = policy.OwnerName;
                identification = policy.OwnerIdentification;
                Policy polHeader = _unitOfWork.Policy.GetById(policy.IdPolicyHeader);
                ins = _unitOfWork.Insurance.GetById(polHeader.IdInsurance.Value);
                insurance = ins != null ? ins.Description : "";
                insl = _unitOfWork.InsuranceLine.GetById(polHeader.IdInsuranceLine.Value);
                insuranceLine = insl != null ? insl.Description : "";
                inssl = _unitOfWork.InsuranceSubline.GetById(polHeader.IdInsuranceSubline.Value);
                insuranceSubline = inssl != null ? inssl.Description : "";
                string initText = "Creación ";
                if (isEdit)
                    initText = "Modificación ";
                if (string.IsNullOrEmpty(policy.License))
                {
                    text = initText + "Orden de Póliza Colectiva #{0} {1} {2} {3} {4} , Propietario: {5} - {6} , Observación: {7} ";
                    subject = string.Format(text, idPolicyOrder, movto, insurance, insuranceLine, insuranceSubline, identification, policyHolder, policy.Observation);
                }
                else
                {
                    text = initText + "Orden de Póliza Colectiva #{0} {1} {2} {3} {4} , Propietario: {5} - {6} , Placa: {7} Marca: {8} Clase: {9} , Observación: {10} ";
                    subject = string.Format(text, idPolicyOrder, movto, insurance, insuranceLine, insuranceSubline, identification, policyHolder, policy.License, vehicleBrand, vehicleClass, policy.Observation);
                }
            }
            Management management = new Management
            {
                ManagementType = "G",
                IdPolicyOrder = idPolicyOrder,
                CreationUser = int.Parse(idUser),
                DelegatedUser = int.Parse(idUser),
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                State = "R",
                Subject = subject,
                ManagementPartner = "O",
                IdCustomer = idCustomer,
                IsExtra = false,
            };
            int idManagement = _unitOfWork.Management.Insert(management);
            if (policy.IsOrder && !isEdit)
            {
                int delegatedTechnical = 0;
                //Creamos la tarea para sistematizar
                Settings s = _unitOfWork.Settings.GetList().FirstOrDefault();
                //Traemos el listado de tecnicos para asignar la tarea
                List<TechnicalAsign> lst = _unitOfWork.TechnicalAsign.GetList().ToList();
                if (lst.Count > 0)
                {
                    if (s.LastTechnicalUserId > 0)
                    {
                        TechnicalAsign lastTechnical = lst.Where(t => t.IdUser.Equals(s.LastTechnicalUserId)).FirstOrDefault();
                        TechnicalAsign nextTechnicalAsign = lst.Where(t => t.OrderAssign.Equals(lastTechnical.OrderAssign + 1)).FirstOrDefault();
                        if (nextTechnicalAsign == null)
                        {
                            TechnicalAsign technicalAsign = lst.Where(t => t.OrderAssign.Equals(1)).FirstOrDefault();
                            delegatedTechnical = technicalAsign.IdUser;
                        }
                        else
                        {
                            delegatedTechnical = nextTechnicalAsign.IdUser;
                        }
                    }
                    else
                    {
                        TechnicalAsign technicalAsign = lst.Where(t => t.OrderAssign.Equals(1)).FirstOrDefault();
                        delegatedTechnical = technicalAsign.IdUser;
                    }
                }
                string textTask = string.Empty;
                string subjectTask = string.Empty;
                if (policy.IsOrder && !policy.IsAttached)
                {
                    if (string.IsNullOrEmpty(policy.License))
                    {
                        textTask = "Sistematizar Orden de Póliza #{0} {1} {2} {3} {4} , Tomador: {5} , Asegurado/s: {6}";
                        subjectTask = string.Format(textTask, idPolicyOrder, movto, insurance, insuranceLine, insuranceSubline, policyHolder, insuredNames);
                    }
                    else
                    {
                        textTask = "Sistematizar Orden de Póliza #{0} {1} {2} {3} {4} , Tomador: {5} , Asegurado/s: {6}, Placa: {7} ";
                        subjectTask = string.Format(textTask, idPolicyOrder, movto, insurance, insuranceLine, insuranceSubline, policyHolder, insuredNames, policy.License);
                    }
                }
                if (policy.IsOrder && policy.IsAttached && idPolicyOrder > 0 && !policy.IsAttachedOrder)
                {
                    if (string.IsNullOrEmpty(policy.License))
                    {
                        textTask = "Sistematizar Orden de Póliza Colectiva #{0} {1} {2} {3} {4} , Propietario: {2} - {3} , Observación: {4}";
                        subjectTask = string.Format(textTask, idPolicyOrder, movto, insurance, insuranceLine, insuranceSubline, identification, policyHolder, policy.Observation);
                    }
                    else
                    {
                        textTask = "Sistematizar Orden de Póliza Colectiva #{0} {1} {2} {3} {4} , Propietario: {2} - {3} , Placa: {4} Marca: {5} Clase: {6} , Observación: {7} ";
                        subjectTask = string.Format(textTask, idPolicyOrder, movto, insurance, insuranceLine, insuranceSubline, identification, policyHolder, policy.License, vehicleBrand, vehicleClass, policy.Observation);
                    }
                }
                Management task = new Management
                {
                    ManagementType = "T",
                    IdPolicyOrder = idPolicyOrder,
                    CreationUser = int.Parse(idUser),
                    DelegatedUser = delegatedTechnical,
                    StartDate = DateTime.Now,
                    State = "P",
                    Subject = subjectTask,
                    ManagementPartner = "O",
                    IdCustomer = idCustomer,
                    IsExtra = true,
                    Assignable = true
                };
                int idTask = _unitOfWork.Management.Insert(task);
                _unitOfWork.ManagementExtra.Insert(new ManagementExtra
                {
                    IdManagement = idManagement,
                    IdManagementExtra = idTask
                });
                s.LastTechnicalUserId = delegatedTechnical;
                _unitOfWork.Settings.Update(s);
            }
        }

        [HttpPost]
        [Route("SavePolicyCol")]
        public IActionResult PostCol([FromBody] PolicySave policy)
        {
            int idPolicy = 0;
            if (!ModelState.IsValid)
                return BadRequest();
            using (var transaction = new TransactionScope())
            {
                try
                {
                    List<DigitalizedFileType> digitalizedFileTypes = _unitOfWork.DigitalizedFileType.GetList().ToList();
                    string idUser = User.Claims.Where(c => c.Type.Equals(ClaimTypes.PrimarySid)).FirstOrDefault().Value;
                    ExternalUser externalUser = _unitOfWork.ExternalUser.GetById(int.Parse(idUser));
                    List<PolicyList> policyLists = _unitOfWork.Policy.PolicyExternalUserByCustomer(externalUser.IdCustomer.Value).ToList();
                    int idPolicyOrder = policy.PolicyOrderId;
                    //Datos de vehículo
                    if (policy.Vehicle != null && !string.IsNullOrEmpty(policy.Vehicle.License))
                    {
                        int idVehicle = 0;
                        //Validamos primero si existe, de ser así se debe actualizar la info
                        Vehicle vehicle = _unitOfWork.Vehicle.VehicleByLicense(policy.Vehicle.License);
                        if (vehicle != null)
                        {
                            vehicle.Chassis = policy.Vehicle.Chassis;
                            vehicle.CommercialValue = policy.Vehicle.CommercialValue;
                            vehicle.Cylinder = policy.Vehicle.Cylinder;
                            vehicle.Fasecolda = policy.Vehicle.Fasecolda;
                            vehicle.IdVehicleBodywork = policy.Vehicle.IdVehicleBodywork;
                            vehicle.IdVehicleBrand = policy.Vehicle.IdVehicleBrand;
                            vehicle.IdVehicleClass = policy.Vehicle.IdVehicleClass;
                            vehicle.IdVehicleReference = policy.Vehicle.IdVehicleReference;
                            vehicle.IdVehicleService = policy.Vehicle.IdVehicleService;
                            vehicle.Model = policy.Vehicle.Model;
                            vehicle.PassengersNumber = policy.Vehicle.PassengersNumber;
                            vehicle.Weight = policy.Vehicle.Weight;
                            _unitOfWork.Vehicle.Update(vehicle);
                            idVehicle = vehicle.Id;
                        }
                        else
                            idVehicle = _unitOfWork.Vehicle.Insert(policy.Vehicle);
                        policy.Policy.IdVehicle = idVehicle;
                    }
                    else
                        policy.Policy.IdVehicle = null;
                    int i = 1;
                    foreach (var item in policyLists)
                    {
                        if (i > 1)
                        {
                            PolicyOrder policyOrder = new PolicyOrder
                            {
                                CreationDate = DateTime.Now,
                                IdExternalUser = int.Parse(idUser),
                                State = "A",
                                StateOrder = "A",
                                IdUser = 1
                            };
                            idPolicyOrder = _unitOfWork.PolicyOrder.Insert(policyOrder);
                        }
                        //Póliza
                        policy.Policy.IdExternalUser = int.Parse(idUser);
                        policy.Policy.Number = "ORDEN-COL-" + idPolicyOrder;
                        policy.Policy.IdInsurance = item.IdInsurance;
                        policy.Policy.IdInsuranceLine = item.IdInsuranceLine;
                        policy.Policy.IdInsuranceSubline = item.IdInsuranceSubline;
                        policy.Policy.IdPolicyHolder = item.IdPolicyHolder;
                        policy.Policy.IdPolicyHeader = item.Id;
                        idPolicy = _unitOfWork.Policy.Insert(policy.Policy);
                        //Tarea
                        _unitOfWork.PolicyOrderDetail.Insert(new PolicyOrderDetail { IdPolicyOrder = idPolicyOrder, IdPolicy = idPolicy, CreationDate = DateTime.Now, State = "A" });
                        //Debemos generar una tarea a un usuario para sistematizar (técnico)
                        //Primero la gestión
                        string policyHolder = policy.Policy.OwnerName;
                        string identification = policy.Policy.OwnerIdentification;
                        string movto = _unitOfWork.MovementType.GetList().Where(m => m.Id.Equals(policy.Policy.IdMovementType)).FirstOrDefault().Alias;
                        string text = string.Empty;
                        string subject = string.Empty;
                        if (string.IsNullOrEmpty(policy.Policy.License))
                        {
                            text = "Creación Orden de Póliza Colectiva #{0} {1} {2} {3} {4} , Propietario: {5} - {6} , Observación: {7} ";
                            subject = string.Format(text, idPolicyOrder, movto, item.InsuranceDesc, item.InsuranceLineDesc, item.InsuranceSublineDesc, identification, policyHolder, policy.Policy.Observation);
                        }
                        else
                        {
                            text = "Creación Orden de Póliza Colectiva #{0} {1} {2} {3} {4} , Propietario: {5} - {6} , Placa: {7} Marca: {8} Clase: {9} , Observación: {10} ";
                            subject = string.Format(text, idPolicyOrder, movto, item.InsuranceDesc, item.InsuranceLineDesc, item.InsuranceSublineDesc, identification, policyHolder, policy.Policy.License, policy.Vehicle.Brand, policy.Vehicle.Class, policy.Policy.Observation);
                        }
                        Management management = new Management
                        {
                            ManagementType = "G",
                            IdPolicyOrder = idPolicyOrder,
                            CreationUser = 1,
                            StartDate = DateTime.Now,
                            EndDate = DateTime.Now,
                            State = "R",
                            Subject = subject,
                            ManagementPartner = "O",
                            IdCustomer = policy.Policy.IdPolicyHolder,
                            IsExtra = false,
                        };
                        int idManagement = _unitOfWork.Management.Insert(management);
                        int delegatedTechnical = 0;
                        //Creamos la tarea para sistematizar
                        Settings s = _unitOfWork.Settings.GetList().FirstOrDefault();
                        //Traemos el listado de tecnicos para asignar la tarea
                        List<TechnicalAsign> lst = _unitOfWork.TechnicalAsign.GetList().ToList();
                        if (lst.Count > 0)
                        {
                            if (s.LastTechnicalUserId > 0)
                            {
                                TechnicalAsign lastTechnical = lst.Where(t => t.IdUser.Equals(s.LastTechnicalUserId)).FirstOrDefault();
                                TechnicalAsign nextTechnicalAsign = lst.Where(t => t.OrderAssign.Equals(lastTechnical.OrderAssign + 1)).FirstOrDefault();
                                if (nextTechnicalAsign == null)
                                {
                                    TechnicalAsign technicalAsign = lst.Where(t => t.OrderAssign.Equals(1)).FirstOrDefault();
                                    delegatedTechnical = technicalAsign.IdUser;
                                }
                                else
                                {
                                    delegatedTechnical = nextTechnicalAsign.IdUser;
                                }
                            }
                            else
                            {
                                TechnicalAsign technicalAsign = lst.Where(t => t.OrderAssign.Equals(1)).FirstOrDefault();
                                delegatedTechnical = technicalAsign.IdUser;
                            }
                        }
                        string textTask = string.Empty;
                        string subjectTask = string.Empty;
                        if (string.IsNullOrEmpty(policy.Policy.License))
                        {
                            textTask = "Sistematizar Orden de Póliza Colectiva #{0} {1} {2} {3} {4} , Propietario: {5} - {6} , Observación: {7}";
                            subjectTask = string.Format(textTask, idPolicyOrder, movto, item.InsuranceDesc, item.InsuranceLineDesc, item.InsuranceSublineDesc, identification, policyHolder, policy.Policy.Observation);
                        }
                        else
                        {
                            textTask = "Sistematizar Orden de Póliza Colectiva #{0} {1} {2} {3} {4} , Propietario: {5} - {6} , Placa: {7} Marca: {8} Clase: {9} , Observación: {10} ";
                            subjectTask = string.Format(textTask, idPolicyOrder, movto, item.InsuranceDesc, item.InsuranceLineDesc, item.InsuranceSublineDesc, identification, policyHolder, policy.Policy.License, policy.Vehicle.Brand, policy.Vehicle.Class, policy.Policy.Observation);
                        }
                        Management task = new Management
                        {
                            ManagementType = "T",
                            IdPolicyOrder = idPolicyOrder,
                            CreationUser = 1,
                            //DelegatedUser = delegatedTechnical,
                            DelegatedUser = 1,
                            StartDate = DateTime.Now,
                            State = "P",
                            Subject = subjectTask,
                            ManagementPartner = "O",
                            IdCustomer = policy.Policy.IdPolicyHolder,
                            IsExtra = true,
                            Assignable = true
                        };
                        int idTask = _unitOfWork.Management.Insert(task);
                        _unitOfWork.ManagementExtra.Insert(new ManagementExtra { IdManagement = idManagement, IdManagementExtra = idTask });
                        s.LastTechnicalUserId = delegatedTechnical;
                        _unitOfWork.Settings.Update(s);
                        //Documentos digitalizados
                        foreach (var df in policy.DigitalizedFiles)
                        {
                            if (string.IsNullOrEmpty(df.Description))
                            {
                                DigitalizedFileType digitalizedFileType = digitalizedFileTypes.Where(dt => dt.Id.Equals(df.IdDigitalizedFileType)).FirstOrDefault();
                                if (digitalizedFileType != null)
                                    df.Description = digitalizedFileType.Description;
                            }
                            df.IdPolicyOrder = idPolicyOrder;
                            _unitOfWork.DigitalizedFile.Insert(df);
                        }
                        i += 1;
                    }
                    //Enviamos notificación con la orden generada
                    string content = "<h2><strong>Estimado: $firstName $lastName</strong></h2><h2><strong>Tomador/Empresa: $name</strong></h2><h3>Se acaba de registrar la siguiente orden:</h3><table style=\"height: 57px;\" width=\"529\"><tbody><tr><td style=\"width: 82.4667px;\"><strong>Placa</strong></td><td style=\"width: 82.5167px;\"><strong>Motor</strong></td><td style=\"width: 82.9333px;\"><strong>Chasis</strong></td><td style=\"width: 80.3667px;\"><strong>Modelo</strong></td><td style=\"width: 80.3667px;\"><strong>Capacidad</strong></td></tr><tr><td style=\"width: 82.4667px;\">$license</td><td style=\"width: 82.5167px;\">$motor</td><td style=\"width: 82.9333px;\">$chasis</td><td style=\"width: 80.3667px;\">$model</td><td style=\"width: 80.3667px;\">$capacity</td></tr></tbody></table><p>&nbsp;</p><table style=\"height: 57px;\" width=\"529\"><tbody><tr><td style=\"width: 82.4667px;\"><strong>Marca</strong></td><td style=\"width: 82.5167px;\"><strong>Clase</strong></td><td style=\"width: 82.9333px;\"><strong>Fasecolda</strong></td></tr><tr><td style=\"width: 82.4667px;\">$brand</td><td style=\"width: 82.5167px;\">$class</td><td style=\"width: 82.9333px;\">$fasecolda</td></tr></tbody></table><br><h3><strong>P&oacute;lizas:</strong></h3>";
                    StringBuilder content_pol = new StringBuilder("<table border=\"1\"><tbody><tr><td><strong>Aseguradora</strong></td><td><strong>Ramo</strong></td><td><strong>Subramo</strong></td><td><strong>N&uacute;mero</strong></td><td><strong>Vigencia</strong></td></tr>");
                    foreach (var item in policyLists)
                    {
                        content_pol.Append("<tr>");
                        content_pol.Append("<td>");
                        content_pol.Append(item.InsuranceDesc);
                        content_pol.Append("</td>");
                        content_pol.Append("<td>");
                        content_pol.Append(item.InsuranceLineDesc);
                        content_pol.Append("</td>");
                        content_pol.Append("<td>");
                        content_pol.Append(item.InsuranceSublineDesc);
                        content_pol.Append("</td>");
                        content_pol.Append("<td>");
                        content_pol.Append(item.Number);
                        content_pol.Append("</td>");
                        string vigencia = "";
                        if (item.StartDate != null && item.EndDate != null)
                            vigencia = item.StartDate.Value.ToString("dd/MM/yyyy") + "-" + item.EndDate.Value.ToString("dd/MM/yyyy");
                        content_pol.Append("<td>");
                        content_pol.Append(vigencia);
                        content_pol.Append("</td>");
                        content_pol.Append("</tr>");
                    }
                    content_pol.Append("</tbody></table>");
                    //firstName
                    content = content.Replace("$firstName", externalUser.FirstName);
                    //lastName
                    content = content.Replace("$lastName", externalUser.LastName);
                    //name
                    if (externalUser.IdCustomer != null)
                    {
                        Customer tomador = _unitOfWork.Customer.GetById(externalUser.IdCustomer.Value);
                        string name = tomador.FirstName + " " + tomador.LastName;
                        content = content.Replace("$name", name);
                    }
                    //placa
                    content = content.Replace("$license", policy.Policy.License);
                    //motor
                    content = content.Replace("$motor", policy.Vehicle.Engine);
                    //chasis
                    content = content.Replace("$chasis", policy.Vehicle.Chassis);
                    //modelo
                    content = content.Replace("$model", policy.Vehicle.Model.ToString());
                    //capacidad
                    content = content.Replace("$capacity", policy.Vehicle.PassengersNumber.ToString());
                    //marca
                    content = content.Replace("$brand", policy.Vehicle.Brand);
                    //clase
                    content = content.Replace("$class", policy.Vehicle.Class);
                    //fasecolda
                    content = content.Replace("$fasecolda", policy.Vehicle.Fasecolda);
                    content = $"{content}{content_pol.ToString()}";
                    string fullName = externalUser.FirstName + " " + externalUser.LastName;
                    string ccName = "Técnico";
                    string cc = "tecnico@wfe.com.co";
                    emailUtils.SendMail(_notificationMetadata, externalUser.Email, fullName,
                    "Orden Colectivas", MimeKit.Text.TextFormat.Html, content, ccName, cc, null);
                    transaction.Complete();
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    return StatusCode(500, "Internal server error: " + ex.Message);
                }
            }
            return Ok(idPolicy);
        }

        [HttpPut]
        public IActionResult Put([FromBody] PolicySave policy)
        {
            int idPolicy = 0;
            if (!ModelState.IsValid)
                return BadRequest();
            using (var transaction = new TransactionScope())
            {
                try
                {
                    //Datos de vehículo
                    if (policy.Vehicle != null && !string.IsNullOrEmpty(policy.Vehicle.License))
                        policy.Policy.IdVehicle = vehicle(policy.Vehicle.License, policy.Policy.IdMovementType, policy.Vehicle);
                    else
                        policy.Policy.IdVehicle = null;

                    string idUser = User.Claims.Where(c => c.Type.Equals(ClaimTypes.PrimarySid)).FirstOrDefault().Value;
                    policy.Policy.IdUser = int.Parse(idUser);
                    idPolicy = policy.Policy.Id;
                    //Debemos bloquear si la placa ya existe en el mismo ramo
                    if (policy.Policy.IdInsuranceLine != null)
                    {
                        bool exist = _unitOfWork.Policy.PolicyDuplicate(idPolicy, policy.Policy.IdInsuranceLine.Value, policy.Policy.License, policy.Policy.IsOrder);
                        if (exist)
                        {
                            transaction.Dispose();
                            return StatusCode(400, "No se puede asegurar la misma placa más de una vez");
                        }
                    }
                    if (policy.Policy.IsAttached)
                    {
                        if (!policy.Policy.Number.Contains("ORDEN-COL-") && !policy.Policy.IsAttachedOrder)
                        {
                            policy.Policy.IdInsurance = null;
                            policy.Policy.IdInsuranceLine = null;
                            policy.Policy.IdInsuranceSubline = null;
                            policy.Policy.Number = null;
                        }
                    }
                    if (_unitOfWork.Policy.Update(policy.Policy))
                    {
                        //Productos propios
                        if (policy.Policy.IdMovementType != "4" && policy.Policy.IdMovementType != "5")
                            ownProducts(policy.PolicyProducts, idPolicy, false);
                        //Asegurados
                        string insuredNames = "";
                        if (policy.Policy.IdMovementType != "4" && policy.Policy.IdMovementType != "5")
                            insuredNames = insured(policy.PolicyInsured, idPolicy, false);
                        //Beneficiarios
                        string beneficariesNames = "";
                        if (policy.Policy.IdMovementType != "4" && policy.Policy.IdMovementType != "5")
                            beneficariesNames = beneficiaries(policy.PolicyBeneficiaries, idPolicy, false);
                        //Cuotas
                        if (policy.Policy.IdMovementType != "4" && policy.Policy.IdMovementType != "5")
                        {
                            fee(policy.PolicyFees, policy.Policy.IdPaymentMethod, policy.Policy.IsHeader, idPolicy,
                                                    policy.Policy.IsAttached, policy.Policy.TotalValue, policy.Policy.ExpiditionDate,
                                                    policy.Policy.StartDate);
                        }
                        //Cuota incial
                        if (policy.Policy.IdMovementType != "4" && policy.Policy.IdMovementType != "5")
                        {
                            int policyFeesProductCount = policy.PolicyFeesProduct != null ? policy.PolicyFeesProduct.Count : 0;
                            initialFee(policy.Policy.TotalInitialFee, policyFeesProductCount, idPolicy, policy.Policy.InitialFee,
                            policy.Policy.OwnProducts, policy.Policy.ExpiditionDate, policy.Policy.StartDate);
                        }
                        //Referencias
                        if (policy.Policy.IdMovementType != "4" && policy.Policy.IdMovementType != "5")
                            references(policy.PolicyReferences, idPolicy, false);
                        //Cuotas financiamiento p. propios
                        if (policy.Policy.IdMovementType != "4" && policy.Policy.IdMovementType != "5")
                            policyFeesProduct(policy.PolicyFeesProduct, idPolicy);
                        //Gestiones-Tareas
                        int idCustomer = policy.PolicyInsured != null && policy.PolicyInsured.Count > 0 ? policy.PolicyInsured[0].Id : 0;
                        string vehicleBrand = policy.Vehicle != null ? policy.Vehicle.Brand : "";
                        string vehicleClass = policy.Vehicle != null ? policy.Vehicle.Class : "";
                        policyManagement(policy.PolicyOrderId, idPolicy, policy.Policy, insuredNames, idUser, idCustomer,
                        vehicleBrand, vehicleClass, beneficariesNames, true);
                    }
                    transaction.Complete();
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    return StatusCode(500, "Internal server error: " + ex.Message);
                }
            }

            return Ok(new { Message = "La Póliza se ha actualizado" });
        }

        [HttpPost]
        [Route("PolicyCancel")]
        public IActionResult PolicyCancel([FromBody] PolicySave policy)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            using (var transaction = new TransactionScope())
            {
                try
                {
                    string idUser = User.Claims.Where(c => c.Type.Equals(ClaimTypes.PrimarySid)).FirstOrDefault().Value;
                    //Dejamos el backup de la póliza inicial
                    PolicyBck policyBck = _mapper.Map<PolicyBck>(policy.Policy);
                    _unitOfWork.PolicyBck.Insert(policyBck);
                    int idPolicyParent = policy.Policy.Id;
                    Policy policyOld = _unitOfWork.Policy.GetById(idPolicyParent);
                    policyOld.IdPolicyState = "2";
                    _unitOfWork.Policy.Update(policyOld);
                    policy.Policy.IdPolicyParent = idPolicyParent;
                    policy.Policy.Id = 0;
                    policy.Policy.PremiumValue = policy.Policy.PremiumValueCan.Value;
                    policy.Policy.Iva = policy.Policy.IvaCan.Value;
                    policy.Policy.NetValue = policy.Policy.NetValueCan.Value;
                    policy.Policy.PremiumExtra = policy.Policy.PremiumExtraCan.Value;
                    policy.Policy.Runt = policy.Policy.RuntCan.Value;
                    policy.Policy.TotalValue = policy.Policy.TotalValueCan.Value;
                    policy.Policy.IdPolicyState = "2";
                    int idPolicy = _unitOfWork.Policy.Insert(policy.Policy);
                    //Productos propios
                    if (policy.Policy.IdMovementType != "4" && policy.Policy.IdMovementType != "5")
                        ownProducts(policy.PolicyProducts, idPolicy, false);
                    string insuredNames = "";
                    //Asegurados
                    if (policy.Policy.IdMovementType != "4" && policy.Policy.IdMovementType != "5")
                        insuredNames = insured(policy.PolicyInsured, idPolicy, false);
                    string beneficariesNames = "";
                    //Beneficiarios
                    if (policy.Policy.IdMovementType != "4" && policy.Policy.IdMovementType != "5")
                        beneficariesNames = beneficiaries(policy.PolicyBeneficiaries, idPolicy, false);
                    //Cuotas
                    if (policy.Policy.IdMovementType != "4" && policy.Policy.IdMovementType != "5")
                    {
                        fee(policy.PolicyFees, policy.Policy.IdPaymentMethod, policy.Policy.IsHeader, idPolicy,
                        policy.Policy.IsAttached, policy.Policy.TotalValue, policy.Policy.ExpiditionDate,
                        policy.Policy.StartDate);
                    }
                    //Cuota incial
                    if (policy.Policy.IdMovementType != "4" && policy.Policy.IdMovementType != "5")
                    {
                        int policyFeesProductCount = policy.PolicyFeesProduct != null ? policy.PolicyFeesProduct.Count : 0;
                        initialFee(policy.Policy.TotalInitialFee, policyFeesProductCount, idPolicy, policy.Policy.InitialFee,
                        policy.Policy.OwnProducts, policy.Policy.ExpiditionDate, policy.Policy.StartDate);
                    }
                    //Referencias
                    if (policy.Policy.IdMovementType != "4" && policy.Policy.IdMovementType != "5")
                        references(policy.PolicyReferences, idPolicy, false);
                    //Cuotas financiamiento p. propios
                    if (policy.Policy.IdMovementType != "4" && policy.Policy.IdMovementType != "5")
                        policyFeesProduct(policy.PolicyFeesProduct, idPolicy);
                    //Gestiones-Tareas
                    int idCustomer = policy.PolicyInsured != null && policy.PolicyInsured.Count > 0 ? policy.PolicyInsured[0].Id : 0;
                    string vehicleBrand = policy.Vehicle != null ? policy.Vehicle.Brand : "";
                    string vehicleClass = policy.Vehicle != null ? policy.Vehicle.Class : "";
                    policyManagement(policy.PolicyOrderId, idPolicy, policy.Policy, insuredNames, idUser, idCustomer,
                    vehicleBrand, vehicleClass, beneficariesNames, true);
                    transaction.Complete();
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    return StatusCode(500, "Internal server error: " + ex.Message);
                }
            }
            return Ok(new { Message = "La Póliza se ha cancelado" });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var policy = _unitOfWork.Policy.GetById(id);
                if (policy == null)
                    return NotFound();
                if (_unitOfWork.Policy.Delete(policy))
                    return Ok(new { Message = "Politica se ha eliminado" });
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("GetPolicyPendingAuthorization")]
        public IActionResult GetPolicyPendingAuthorization()
        {
            try
            {
                return Ok(_unitOfWork.Policy.PolicyPendingAuthorizationList());
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("GetPolicyPendingAuthorizationDisc")]
        public IActionResult GetPolicyPendingAuthorizationDisc()
        {
            try
            {
                return Ok(_unitOfWork.Policy.PolicyPendingAuthorizationDiscList());
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("GetPolicyReportProduction")]
        public IActionResult GetPolicyReportProduction([FromBody] GetPolicyReportProduction request)
        {
            try
            {
                string idUser = User.Claims.Where(c => c.Type.Equals(ClaimTypes.PrimarySid)).FirstOrDefault().Value;
                if (request.CurrentUser)
                    request.IdUser = int.Parse(idUser);
                var result = _unitOfWork.Policy.PolicyReportProduction(request.IdUser, request.StartDate, request.EndDate);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("GetPolicyColReportProduction")]
        public IActionResult GetPolicyColReportProduction([FromBody] GetPolicyReportProduction request)
        {
            try
            {
                return Ok(_unitOfWork.Policy.PolicyColReportProduction(request.StartDate, request.EndDate, request.IdPolicyHolder));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        [HttpPost]
        [Route("GetPolicyReportProductionConsolidated")]
        public IActionResult GetPolicyReportProductionConsolidated([FromBody] GetPolicyReportProduction request)
        {
            try
            {
                return Ok(_unitOfWork.Policy.PolicyReportProductionConsolidated(request.IdUser, request.StartDate, request.EndDate));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}
