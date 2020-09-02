using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using InsuranceBackend.Models;
using InsuranceBackend.UnitOfWork;
using InsuranceBackend.WebApi.Models;
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
        public PolicyController(IUnitOfWork unitOfWork, NotificationMetadata notificationMetadata)
        {
            _unitOfWork = unitOfWork;
            _notificationMetadata = notificationMetadata;
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
                return Ok(_unitOfWork.Policy.PolicyPagedListSearchTerms(request.Identification, request.Name, request.Number, request.IdCustomer, idUser, request.IsOrder, request.Page, request.Rows, request.StateOrder));
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
                    {
                        int idVehicle = 0;
                        //Validamos primero si existe, de ser así se debe actualizar la info
                        Vehicle vehicle = _unitOfWork.Vehicle.VehicleByLicense(policy.Vehicle.License);
                        idVehicle = vehicle != null ? vehicle.Id : 0;
                        if (policy.Policy.IdMovementType != "4" && policy.Policy.IdMovementType != "5")
                        {
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
                            }
                            else
                                idVehicle = _unitOfWork.Vehicle.Insert(policy.Vehicle);
                        }
                        policy.Policy.IdVehicle = idVehicle;
                    }
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
                    {
                        if (policy.PolicyProducts != null && policy.PolicyProducts.Count > 0)
                        {
                            //Primero debemos eliminar los productos propios
                            _unitOfWork.PolicyProduct.DeletePolicyProductByPolicy(idPolicy);
                            foreach (var item in policy.PolicyProducts)
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
                    StringBuilder insuredNames = new StringBuilder();
                    //Asegurados
                    if (policy.Policy.IdMovementType != "4" && policy.Policy.IdMovementType != "5")
                    {
                        if (policy.PolicyInsured != null && policy.PolicyInsured.Count > 0)
                        {
                            foreach (var item in policy.PolicyInsured)
                            {
                                Customer i = _unitOfWork.Customer.GetById(item.Id);
                                string n = i.FirstName + (string.IsNullOrEmpty(i.MiddleName) ? "" : " " + i.MiddleName) + i.LastName + (string.IsNullOrEmpty(i.MiddleLastName) ? "" : " " + i.MiddleLastName);
                                if (insuredNames.Length > 0)
                                    insuredNames.Append(", " + n);
                                else
                                    insuredNames.Append(n);
                                PolicyInsured insured = new PolicyInsured
                                {
                                    IdInsured = item.Id,
                                    IdPolicy = idPolicy
                                };
                                _unitOfWork.PolicyInsured.Insert(insured);
                            }
                        }
                    }
                    StringBuilder beneficariesNames = new StringBuilder();
                    //Beneficiarios
                    if (policy.Policy.IdMovementType != "4" && policy.Policy.IdMovementType != "5")
                    {
                        if (policy.PolicyBeneficiaries != null && policy.PolicyBeneficiaries.Count > 0)
                        {
                            foreach (var item in policy.PolicyBeneficiaries)
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
                    //Cuotas
                    if (policy.PolicyFees != null && policy.PolicyFees.Count > 0 && policy.Policy.IdPaymentMethod != "4" && policy.Policy.IdPaymentMethod != "2" && !policy.Policy.IsHeader)
                    {
                        //Primero se debe eliminar las existentes
                        _unitOfWork.PolicyFee.DeleteFeeByPolicy(idPolicy);
                        foreach (var item in policy.PolicyFees)
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
                        if (!policy.Policy.IsAttached)
                        {
                            if (policy.Policy.IdPaymentMethod != "3") // Si no es financiado debemos crear por lo menos una cuota para poder hacer los pagos
                            {
                                if (policy.Policy.StartDate.HasValue)
                                {
                                    PolicyFee policyFee = new PolicyFee
                                    {
                                        Number = 1,
                                        IdPolicy = idPolicy,
                                        Date = policy.Policy.StartDate.Value,
                                        Value = policy.Policy.TotalValue,
                                        DatePayment = policy.Policy.StartDate.Value,
                                    };
                                    _unitOfWork.PolicyFee.Insert(policyFee);
                                }
                                else
                                {
                                    PolicyFee policyFee = new PolicyFee
                                    {
                                        Number = 1,
                                        IdPolicy = idPolicy,
                                        Date = policy.Policy.ExpiditionDate,
                                        Value = policy.Policy.TotalValue,
                                        DatePayment = policy.Policy.ExpiditionDate,
                                    };
                                    _unitOfWork.PolicyFee.Insert(policyFee);
                                }
                            }
                            if (policy.Policy.IdPaymentMethod == "4" || policy.Policy.IdPaymentMethod == "2")
                            {
                                //Primero se debe eliminar las existentes
                                _unitOfWork.PolicyFeeFinancial.DeleteFeeByPolicy(idPolicy);
                                foreach (var item in policy.PolicyFees)
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
                    //Cuota incial
                    if (policy.Policy.TotalInitialFee > 0 && policy.PolicyFeesProduct.Count == 0)
                    {
                        _unitOfWork.PolicyFee.DeleteFeeByPolicyFeeNumber(idPolicy, 0);
                        if (policy.Policy.StartDate.HasValue)
                        {
                            PolicyFee policyFee = new PolicyFee
                            {
                                Number = 0,
                                IdPolicy = idPolicy,
                                Date = policy.Policy.StartDate.Value,
                                Value = policy.Policy.InitialFee,
                                ValueOwnProduct = policy.Policy.OwnProducts,
                                DatePayment = policy.Policy.StartDate.Value,
                            };
                            _unitOfWork.PolicyFee.Insert(policyFee);
                        }
                        else
                        {
                            PolicyFee policyFee = new PolicyFee
                            {
                                Number = 0,
                                IdPolicy = idPolicy,
                                Date = policy.Policy.ExpiditionDate,
                                Value = policy.Policy.InitialFee,
                                ValueOwnProduct = policy.Policy.OwnProducts,
                                DatePayment = policy.Policy.ExpiditionDate,
                            };
                            _unitOfWork.PolicyFee.Insert(policyFee);
                        }
                    }
                    //Referencias
                    if (policy.PolicyReferences != null && policy.PolicyReferences.Count > 0)
                    {
                        //Primero se debe eliminar las existentes
                        _unitOfWork.PolicyReferences.DeletePolicyReferenciesByPolicy(idPolicy);
                        foreach (var item in policy.PolicyReferences)
                        {
                            PolicyReferences policyReferences = new PolicyReferences
                            {
                                Name = item.Name,
                                Phone = item.Phone,
                                Mobile = item.Mobile,
                                Address = item.Address,
                                IdRelationship = item.IdRelationship,
                                IdPolicy = idPolicy
                            };
                            _unitOfWork.PolicyReferences.Insert(policyReferences);
                        }
                    }
                    //Cuotas financiamiento p. propios
                    if (policy.PolicyFeesProduct != null && policy.PolicyFeesProduct.Count > 0)
                    {
                        //Primero eliminamos las existentes
                        _unitOfWork.PolicyFeeProduct.DeleteFeeProductByPolicy(idPolicy);
                        foreach (var item in policy.PolicyFeesProduct)
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
                    //Si es una orden se debe guardar
                    if (policy.Policy.IsOrder && !policy.Policy.IsAttached)
                    {
                        _unitOfWork.PolicyOrderDetail.Insert(new PolicyOrderDetail { IdPolicyOrder = policy.PolicyOrderId, IdPolicy = idPolicy, CreationDate = DateTime.Now, State = "A" });
                        //Debemos generar una tarea a un usuario para sistematizar (técnico)
                        //Primero la gestión
                        Customer h = _unitOfWork.Customer.GetById(policy.Policy.IdPolicyHolder.Value);
                        string policyHolder = h.FirstName + (string.IsNullOrEmpty(h.MiddleName) ? "" : " " + h.MiddleName) + h.LastName + (string.IsNullOrEmpty(h.MiddleLastName) ? "" : " " + h.MiddleLastName);
                        string movto = _unitOfWork.MovementType.GetList().Where(m => m.Id.Equals(policy.Policy.IdMovementType)).FirstOrDefault().Alias;
                        string insurance = _unitOfWork.Insurance.GetById(policy.Policy.IdInsurance.Value).Description;
                        string insuranceLine = _unitOfWork.InsuranceLine.GetById(policy.Policy.IdInsuranceLine.Value).Description;
                        string insuranceSubline = _unitOfWork.InsuranceSubline.GetById(policy.Policy.IdInsuranceSubline.Value).Description;
                        string text = string.Empty;
                        string subject = string.Empty;
                        if (string.IsNullOrEmpty(policy.Policy.License))
                        {
                            text = "Modificación Orden de Póliza #{0} {1} {2} {3} {4} , Tomador: {5} , Asegurado/s: {6} ";
                            subject = string.Format(text, policy.PolicyOrderId, movto, insurance, insuranceLine, insuranceSubline, policyHolder, insuredNames.ToString());
                        }
                        else
                        {
                            text = "Modificación Orden de Póliza #{0} {1} {2} {3} {4} , Tomador: {5} , Asegurado/s: {6}, Placa: {7} ";
                            subject = string.Format(text, policy.PolicyOrderId, movto, insurance, insuranceLine, insuranceSubline, policyHolder, insuredNames.ToString(), policy.Policy.License);
                        }
                        Management management = new Management
                        {
                            ManagementType = "G",
                            IdPolicyOrder = policy.PolicyOrderId,
                            CreationUser = int.Parse(idUser),
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
                            textTask = "Sistematizar Orden de Póliza #{0} {1} {2} {3} {4} , Tomador: {5} , Asegurado/s: {6}";
                            subjectTask = string.Format(textTask, policy.PolicyOrderId, movto, insurance, insuranceLine, insuranceSubline, policyHolder, insuredNames.ToString());
                        }
                        else
                        {
                            textTask = "Sistematizar Orden de Póliza #{0} {1} {2} {3} {4} , Tomador: {5} , Asegurado/s: {6}, Placa: {7} ";
                            subjectTask = string.Format(textTask, policy.PolicyOrderId, movto, insurance, insuranceLine, insuranceSubline, policyHolder, insuredNames.ToString(), policy.Policy.License);
                        }
                        Management task = new Management
                        {
                            ManagementType = "T",
                            IdPolicyOrder = policy.PolicyOrderId,
                            CreationUser = int.Parse(idUser),
                            DelegatedUser = delegatedTechnical,
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
                    }
                    else
                    {
                        if (policy.PolicyOrderId > 0 && !policy.Policy.IsAttached && !policy.Policy.IsHeader || (policy.Policy.IsAttached && policy.Policy.IsAttachedOrder))
                        {
                            //Primero deshabilitamos la que existe
                            _unitOfWork.PolicyOrderDetail.UpdateState(policy.PolicyOrderId, "I");
                            _unitOfWork.PolicyOrderDetail.Insert(new PolicyOrderDetail { IdPolicyOrder = policy.PolicyOrderId, IdPolicy = idPolicy, CreationDate = DateTime.Now, State = "A" });
                            //Debemos dar la tarea por terminada y crear una gestión indicando la sistematización que se hizo
                            //tarea
                            Management task = _unitOfWork.Management.ManagementByPolicyOrder(policy.PolicyOrderId, "T");
                            if (task != null)
                            {
                                task.State = "R";
                                _unitOfWork.Management.Update(task);
                            }
                            //gestión
                            Customer h = _unitOfWork.Customer.GetById(policy.Policy.IdPolicyHolder.Value);
                            string policyHolder = h.FirstName + (string.IsNullOrEmpty(h.MiddleName) ? "" : " " + h.MiddleName) + h.LastName + (string.IsNullOrEmpty(h.MiddleLastName) ? "" : " " + h.MiddleLastName);
                            string movto = _unitOfWork.MovementType.GetList().Where(m => m.Id.Equals(policy.Policy.IdMovementType)).FirstOrDefault().Alias;
                            string insurance = "", insuranceLine = "", insuranceSubline = "";
                            insurance = _unitOfWork.Insurance.GetById(policy.Policy.IdInsurance.Value).Description;
                            insuranceLine = _unitOfWork.InsuranceLine.GetById(policy.Policy.IdInsuranceLine.Value).Description;
                            insuranceSubline = _unitOfWork.InsuranceSubline.GetById(policy.Policy.IdInsuranceSubline.Value).Description;
                            string text = string.Empty;
                            string subject = string.Empty;
                            if (string.IsNullOrEmpty(policy.Policy.License))
                            {
                                text = "Póliza #{0} {1} {2} {3} {4} , Tomador: {5} , Asegurado/s: {6}, Beneficiarios: {7}, Valor Prima: {8}, Total:{9} ";
                                subject = string.Format(text, policy.Policy.Number, movto, insurance, insuranceLine, insuranceSubline, policyHolder, insuredNames.ToString(), beneficariesNames.ToString(), String.Format("{0:0,0.0}", policy.Policy.PremiumValue), String.Format("{0:0,0.0}", policy.Policy.TotalValue));
                            }
                            else
                            {
                                text = "Póliza #{0} {1} {2} {3} {4} , Tomador: {5} , Asegurado/s: {6}, Beneficiarios: {7}, Placa: {8}, Valor Prima: {9}, Total:{10} ";
                                subject = string.Format(text, policy.Policy.Number, movto, insurance, insuranceLine, insuranceSubline, policyHolder, insuredNames.ToString(), beneficariesNames.ToString(), policy.Policy.License, String.Format("{0:0,0.0}", policy.Policy.PremiumValue), String.Format("{0:0,0.0}", policy.Policy.TotalValue));
                            }
                            Management management = new Management
                            {
                                ManagementType = "G",
                                IdPolicyOrder = policy.PolicyOrderId,
                                CreationUser = int.Parse(idUser),
                                StartDate = DateTime.Now,
                                EndDate = DateTime.Now,
                                State = "R",
                                Subject = subject,
                                ManagementPartner = "O",
                                IdCustomer = policy.Policy.IdPolicyHolder,
                                IsExtra = false,
                            };
                            _unitOfWork.Management.Insert(management);
                        }
                    }
                    if (policy.Policy.IsAttached && policy.PolicyOrderId > 0 && !policy.Policy.IsAttachedOrder)
                    {
                        _unitOfWork.PolicyOrderDetail.Insert(new PolicyOrderDetail { IdPolicyOrder = policy.PolicyOrderId, IdPolicy = idPolicy, CreationDate = DateTime.Now, State = "A" });
                        //Debemos generar una tarea a un usuario para sistematizar (técnico)
                        //Primero la gestión
                        string policyHolder = policy.Policy.OwnerName;
                        string identification = policy.Policy.OwnerIdentification;
                        string movto = _unitOfWork.MovementType.GetList().Where(m => m.Id.Equals(policy.Policy.IdMovementType)).FirstOrDefault().Alias;
                        string text = string.Empty;
                        string subject = string.Empty;
                        if (string.IsNullOrEmpty(policy.Policy.License))
                        {
                            text = "Creación Orden de Póliza Colectiva #{0} {1} , Propietario: {2} - {3} , Observación: {4} ";
                            subject = string.Format(text, policy.PolicyOrderId, movto, identification, policyHolder, policy.Policy.Observation);
                        }
                        else
                        {
                            text = "Creación Orden de Póliza Colectiva #{0} {1} , Propietario: {2} - {3} , Placa: {4} Marca: {5} Clase: {6} , Observación: {7} ";
                            subject = string.Format(text, policy.PolicyOrderId, movto, identification, policyHolder, policy.Policy.License, policy.Vehicle.Brand, policy.Vehicle.Class, policy.Policy.Observation);
                        }
                        Management management = new Management
                        {
                            ManagementType = "G",
                            IdPolicyOrder = policy.PolicyOrderId,
                            CreationUser = int.Parse(idUser),
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
                            textTask = "Sistematizar Orden de Póliza Colectiva #{0} {1} , Propietario: {2} - {3} , Observación: {4}";
                            subjectTask = string.Format(textTask, policy.PolicyOrderId, movto, identification, policyHolder, policy.Policy.Observation);
                        }
                        else
                        {
                            textTask = "Sistematizar Orden de Póliza Colectiva #{0} {1} , Propietario: {2} - {3} , Placa: {4} Marca: {5} Clase: {6} , Observación: {7} ";
                            subjectTask = string.Format(textTask, policy.PolicyOrderId, movto, identification, policyHolder, policy.Policy.License, policy.Vehicle.Brand, policy.Vehicle.Class, policy.Policy.Observation);
                        }
                        Management task = new Management
                        {
                            ManagementType = "T",
                            IdPolicyOrder = policy.PolicyOrderId,
                            CreationUser = int.Parse(idUser),
                            DelegatedUser = delegatedTechnical,
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
                    }
                    //Documentos digitalizados
                    if (policy.PolicyOrderId > 0 && policy.Policy.IsHeader)
                    {
                        _unitOfWork.PolicyOrderDetail.Insert(new PolicyOrderDetail { IdPolicyOrder = policy.PolicyOrderId, IdPolicy = idPolicy, CreationDate = DateTime.Now, State = "A" });
                    }
                    if (policy.DigitalizedFiles != null && policy.DigitalizedFiles.Count > 0)
                    { //Nueva versión documentos digitales
                        foreach (var df in policy.DigitalizedFiles)
                        {
                            df.IdPolicyOrder = policy.PolicyOrderId;
                            _unitOfWork.DigitalizedFile.Insert(df);
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
                    "Orden Colectivas", MimeKit.Text.TextFormat.Html, content, ccName, cc);
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
                        if (policy.PolicyProducts.Count > 0)
                        {
                            //Primero debemos eliminar los productos propios
                            _unitOfWork.PolicyProduct.DeletePolicyProductByPolicy(idPolicy);
                            foreach (var item in policy.PolicyProducts)
                            {
                                PolicyProduct product = item as PolicyProduct;
                                if (product.FeeValue > 0)
                                {
                                    product.Value = 0;
                                }
                                product.IdPolicy = idPolicy;
                                _unitOfWork.PolicyProduct.Insert(product);
                            }
                        }
                        //Asegurados
                        StringBuilder insuredNames = new StringBuilder();
                        if (policy.PolicyInsured != null && policy.PolicyInsured.Count > 0)
                        {
                            //Primero debemos eliminar los asegurados
                            _unitOfWork.PolicyInsured.DeletePolicyInsuredByPolicy(idPolicy);
                            foreach (var item in policy.PolicyInsured)
                            {
                                Customer i = _unitOfWork.Customer.GetById(item.Id);
                                string n = i.FirstName + (string.IsNullOrEmpty(i.MiddleName) ? "" : " " + i.MiddleName) + i.LastName + (string.IsNullOrEmpty(i.MiddleLastName) ? "" : " " + i.MiddleLastName);
                                if (insuredNames.Length > 0)
                                    insuredNames.Append(", " + n);
                                else
                                    insuredNames.Append(n);
                                PolicyInsured insured = new PolicyInsured
                                {
                                    IdInsured = item.Id,
                                    IdPolicy = idPolicy
                                };
                                _unitOfWork.PolicyInsured.Insert(insured);
                            }
                        }
                        //Beneficiarios
                        StringBuilder beneficariesNames = new StringBuilder();
                        if (policy.PolicyBeneficiaries != null && policy.PolicyBeneficiaries.Count > 0)
                        {
                            foreach (var item in policy.PolicyBeneficiaries)
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
                                //Eliminamos los beneficiarios
                                _unitOfWork.PolicyBeneficiary.DeletePolicyBeneficiaryByPolicy(idPolicy);
                                PolicyBeneficiary beneficiary = new PolicyBeneficiary
                                {
                                    IdBeneficiary = idBeneficiary,
                                    IdPolicy = idPolicy,
                                    Percentage = item.Percentage
                                };
                                _unitOfWork.PolicyBeneficiary.Insert(beneficiary);
                            }
                        }
                        //Cuotas
                        if (policy.PolicyFees != null && policy.PolicyFees.Count > 0 && policy.Policy.IdPaymentMethod != "4" && policy.Policy.IdPaymentMethod != "2")
                        {
                            //Primero se debe eliminar las existentes
                            _unitOfWork.PolicyFee.DeleteFeeByPolicy(idPolicy);
                            foreach (var item in policy.PolicyFees)
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
                            if (policy.Policy.IdPaymentMethod != "3") // Si no es financiado debemos crear por lo menos una cuota para poder hacer los pagos
                            {
                                _unitOfWork.PolicyFee.DeleteFeeByPolicy(idPolicy);
                                if (policy.Policy.StartDate.HasValue)
                                {
                                    PolicyFee policyFee = new PolicyFee
                                    {
                                        Number = 1,
                                        IdPolicy = idPolicy,
                                        Date = policy.Policy.StartDate.Value,
                                        Value = policy.Policy.TotalValue,
                                        DatePayment = policy.Policy.StartDate.Value,
                                    };
                                    _unitOfWork.PolicyFee.Insert(policyFee);
                                }
                                else
                                {
                                    PolicyFee policyFee = new PolicyFee
                                    {
                                        Number = 1,
                                        IdPolicy = idPolicy,
                                        Date = policy.Policy.ExpiditionDate,
                                        Value = policy.Policy.TotalValue,
                                        DatePayment = policy.Policy.ExpiditionDate,
                                    };
                                    _unitOfWork.PolicyFee.Insert(policyFee);
                                }
                            }
                            if (policy.Policy.IdPaymentMethod == "4" || policy.Policy.IdPaymentMethod == "2")
                            {
                                //Primero se debe eliminar las existentes
                                _unitOfWork.PolicyFeeFinancial.DeleteFeeByPolicy(idPolicy);
                                foreach (var item in policy.PolicyFees)
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
                        //Cuota incial
                        if (policy.Policy.TotalInitialFee > 0 && policy.PolicyFeesProduct.Count == 0)
                        {
                            _unitOfWork.PolicyFee.DeleteFeeByPolicyFeeNumber(idPolicy, 0);
                            if (policy.Policy.StartDate.HasValue)
                            {
                                PolicyFee policyFee = new PolicyFee
                                {
                                    Number = 0,
                                    IdPolicy = idPolicy,
                                    Date = policy.Policy.StartDate.Value,
                                    Value = policy.Policy.InitialFee,
                                    ValueOwnProduct = policy.Policy.OwnProducts,
                                    DatePayment = policy.Policy.StartDate.Value,
                                };
                                _unitOfWork.PolicyFee.Insert(policyFee);
                            }
                            else
                            {
                                PolicyFee policyFee = new PolicyFee
                                {
                                    Number = 0,
                                    IdPolicy = idPolicy,
                                    Date = policy.Policy.ExpiditionDate,
                                    Value = policy.Policy.InitialFee,
                                    ValueOwnProduct = policy.Policy.OwnProducts,
                                    DatePayment = policy.Policy.ExpiditionDate,
                                };
                                _unitOfWork.PolicyFee.Insert(policyFee);
                            }
                        }
                        //Referencias
                        if (policy.PolicyReferences != null && policy.PolicyReferences.Count > 0)
                        {
                            //Primero se debe eliminar las existentes
                            _unitOfWork.PolicyReferences.DeletePolicyReferenciesByPolicy(idPolicy);
                            foreach (var item in policy.PolicyReferences)
                            {
                                PolicyReferences policyReferences = new PolicyReferences
                                {
                                    Name = item.Name,
                                    Phone = item.Phone,
                                    Mobile = item.Mobile,
                                    Address = item.Address,
                                    IdRelationship = item.IdRelationship,
                                    IdPolicy = idPolicy
                                };
                                _unitOfWork.PolicyReferences.Insert(policyReferences);
                            }
                        }
                        //Cuotas financiamiento p. propios
                        if (policy.PolicyFeesProduct != null && policy.PolicyFeesProduct.Count > 0)
                        {
                            //Primero eliminamos las existentes
                            _unitOfWork.PolicyFeeProduct.DeleteFeeProductByPolicy(idPolicy);
                            foreach (var item in policy.PolicyFeesProduct)
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
                        if (policy.Policy.IsOrder)
                        {
                            //Creamos una gestión con la modificación realizada
                            Customer h = _unitOfWork.Customer.GetById(policy.Policy.IdPolicyHolder.Value);
                            string policyHolder = h.FirstName + (string.IsNullOrEmpty(h.MiddleName) ? "" : " " + h.MiddleName) + h.LastName + (string.IsNullOrEmpty(h.MiddleLastName) ? "" : " " + h.MiddleLastName);
                            string movto = _unitOfWork.MovementType.GetList().Where(m => m.Id.Equals(policy.Policy.IdMovementType)).FirstOrDefault().Alias;
                            string insurance = _unitOfWork.Insurance.GetById(policy.Policy.IdInsurance.Value).Description;
                            string insuranceLine = _unitOfWork.InsuranceLine.GetById(policy.Policy.IdInsuranceLine.Value).Description;
                            string insuranceSubline = _unitOfWork.InsuranceSubline.GetById(policy.Policy.IdInsuranceSubline.Value).Description;
                            string text = string.Empty;
                            string subject = string.Empty;
                            if (string.IsNullOrEmpty(policy.Policy.License))
                            {
                                text = "Modificación Orden de Póliza #{0} {1} {2} {3} {4} , Tomador: {5} , Asegurado/s: {6} ";
                                subject = string.Format(text, policy.PolicyOrderId, movto, insurance, insuranceLine, insuranceSubline, policyHolder, insuredNames.ToString());
                            }
                            else
                            {
                                text = "Modificación Orden de Póliza #{0} {1} {2} {3} {4} , Tomador: {5} , Asegurado/s: {6}, Placa: {7} ";
                                subject = string.Format(text, policy.PolicyOrderId, movto, insurance, insuranceLine, insuranceSubline, policyHolder, insuredNames.ToString(), policy.Policy.License);
                            }
                            Management management = new Management
                            {
                                ManagementType = "G",
                                IdPolicyOrder = policy.PolicyOrderId,
                                CreationUser = int.Parse(idUser),
                                StartDate = DateTime.Now,
                                EndDate = DateTime.Now,
                                State = "R",
                                Subject = subject,
                                ManagementPartner = "O",
                                IdCustomer = policy.Policy.IdPolicyHolder,
                                IsExtra = false,
                            };
                            _unitOfWork.Management.Insert(management);
                        }
                        else
                        {
                            //Creamos una gestión con la modificación realizada
                            Customer h = _unitOfWork.Customer.GetById(policy.Policy.IdPolicyHolder.Value);
                            string policyHolder = h.FirstName + (string.IsNullOrEmpty(h.MiddleName) ? "" : " " + h.MiddleName) + h.LastName + (string.IsNullOrEmpty(h.MiddleLastName) ? "" : " " + h.MiddleLastName);
                            string movto = _unitOfWork.MovementType.GetList().Where(m => m.Id.Equals(policy.Policy.IdMovementType)).FirstOrDefault().Alias;
                            string insurance = _unitOfWork.Insurance.GetById(policy.Policy.IdInsurance.Value).Description;
                            string insuranceLine = _unitOfWork.InsuranceLine.GetById(policy.Policy.IdInsuranceLine.Value).Description;
                            string insuranceSubline = _unitOfWork.InsuranceSubline.GetById(policy.Policy.IdInsuranceSubline.Value).Description;
                            string text = string.Empty;
                            string subject = string.Empty;
                            if (string.IsNullOrEmpty(policy.Policy.License))
                            {
                                text = "Modificación Póliza #{0} {1} {2} {3} {4} , Tomador: {5} , Asegurado/s: {6}, Beneficiarios: {7}, Valor Prima: {8}, Total:{9} ";
                                subject = string.Format(text, policy.Policy.Number, movto, insurance, insuranceLine, insuranceSubline, policyHolder, insuredNames.ToString(), beneficariesNames.ToString(), String.Format("{0:0,0.0}", policy.Policy.PremiumValue), String.Format("{0:0,0.0}", policy.Policy.TotalValue));
                            }
                            else
                            {
                                text = "Moficicación Póliza #{0} {1} {2} {3} {4} , Tomador: {5} , Asegurado/s: {6}, Beneficiarios: {7}, Placa: {8}, Valor Prima: {9}, Total:{10} ";
                                subject = string.Format(text, policy.Policy.Number, movto, insurance, insuranceLine, insuranceSubline, policyHolder, insuredNames.ToString(), beneficariesNames.ToString(), policy.Policy.License, String.Format("{0:0,0.0}", policy.Policy.PremiumValue), String.Format("{0:0,0.0}", policy.Policy.TotalValue));
                            }
                            Management management = new Management
                            {
                                ManagementType = "G",
                                IdPolicyOrder = policy.PolicyOrderId,
                                CreationUser = int.Parse(idUser),
                                StartDate = DateTime.Now,
                                EndDate = DateTime.Now,
                                State = "R",
                                Subject = subject,
                                ManagementPartner = "O",
                                IdCustomer = policy.Policy.IdPolicyHolder,
                                IsExtra = false,
                            };
                            _unitOfWork.Management.Insert(management);
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

            return Ok(new { Message = "La Póliza se ha actualizado" });
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
