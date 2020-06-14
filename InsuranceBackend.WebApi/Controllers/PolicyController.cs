using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
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
        private readonly IUnitOfWork _unitOfWork;
        public PolicyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
                return Ok(_unitOfWork.Policy.PolicyPagedListSearchTerms(request.Identification, request.Name, request.Number, request.IdCustomer, idUser, request.IsOrder, request.Page, request.Rows));
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
                return Ok(_unitOfWork.Policy.PolicyByIdPolicyOrder(int.Parse(request.SearchTerm)));
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
                if (policy.RevokePromisoryNote)
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
                    if (policy.Policy.IsOrder)
                    {
                        policy.Policy.Number = "ORDEN-" + policy.PolicyOrderId;
                    }
                    idPolicy = _unitOfWork.Policy.Insert(policy.Policy);
                    //Productos propios
                    if (policy.PolicyProducts.Count > 0)
                    {
                        //Primero debemos eliminar los productos propios
                        _unitOfWork.PolicyProduct.DeletePolicyProductByPolicy(idPolicy);
                        foreach (var item in policy.PolicyProducts)
                        {
                            PolicyProduct product = item as PolicyProduct;
                            product.IdPolicy = idPolicy;
                            _unitOfWork.PolicyProduct.Insert(product);
                        }
                    }
                    //Asegurados
                    StringBuilder insuredNames = new StringBuilder();
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
                    if (policy.PolicyFees != null && policy.PolicyFees.Count > 0 && policy.Policy.IdPaymentMethod != "4")
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
                        if(policy.Policy.IdPaymentMethod == "4")
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
                    if (policy.Policy.IdPaymentMethod == "2" && policy.Policy.StartDate.HasValue)
                    {
                        //Primero se debe eliminar las existentes
                        _unitOfWork.PolicyFeeFinancial.DeleteFeeByPolicy(idPolicy);
                        DateTime start = new DateTime(policy.Policy.StartDate.Value.Year, policy.Policy.StartDate.Value.Month, policy.Policy.Payday);
                        for (int i = 1; i <= policy.Policy.FeeNumbers; i++)
                        {
                            PolicyFeeFinancial policyFeeFinancial = new PolicyFeeFinancial
                            {
                                Number = i,
                                IdPolicy = idPolicy,
                                Date = policy.Policy.StartDate.Value.AddMonths(i - 1),
                                Value = policy.Policy.FeeValue,
                                DateInsurance = start.AddMonths(i - 1),
                                DatePayment = start.AddMonths(i - 1)
                            };
                            _unitOfWork.PolicyFeeFinancial.Insert(policyFeeFinancial);
                        }
                    }
                    //Cuota incial
                    if (policy.Policy.TotalInitialFee > 0)
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
                    //Si es una orden se debe guardar
                    if (policy.Policy.IsOrder)
                    {
                        _unitOfWork.PolicyOrderDetail.Insert(new PolicyOrderDetail { IdPolicyOrder = policy.PolicyOrderId, IdPolicy = idPolicy, CreationDate = DateTime.Now, State = "A" });
                        //Debemos generar una tarea a un usuario para sistematizar (técnico)
                        //Primero la gestión
                        Customer h = _unitOfWork.Customer.GetById(policy.Policy.IdPolicyHolder);
                        string policyHolder = h.FirstName + (string.IsNullOrEmpty(h.MiddleName) ? "" : " " + h.MiddleName) + h.LastName + (string.IsNullOrEmpty(h.MiddleLastName) ? "" : " " + h.MiddleLastName);
                        string movto = _unitOfWork.MovementType.GetList().Where(m => m.Id.Equals(policy.Policy.IdMovementType)).FirstOrDefault().Alias;
                        string insurance = _unitOfWork.Insurance.GetById(policy.Policy.IdInsurance).Description;
                        string insuranceLine = _unitOfWork.InsuranceLine.GetById(policy.Policy.IdInsuranceLine).Description;
                        string insuranceSubline = _unitOfWork.InsuranceSubline.GetById(policy.Policy.IdInsuranceSubline).Description;
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
                        if (policy.PolicyOrderId > 0)
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
                            Customer h = _unitOfWork.Customer.GetById(policy.Policy.IdPolicyHolder);
                            string policyHolder = h.FirstName + (string.IsNullOrEmpty(h.MiddleName) ? "" : " " + h.MiddleName) + h.LastName + (string.IsNullOrEmpty(h.MiddleLastName) ? "" : " " + h.MiddleLastName);
                            string movto = _unitOfWork.MovementType.GetList().Where(m => m.Id.Equals(policy.Policy.IdMovementType)).FirstOrDefault().Alias;
                            string insurance = _unitOfWork.Insurance.GetById(policy.Policy.IdInsurance).Description;
                            string insuranceLine = _unitOfWork.InsuranceLine.GetById(policy.Policy.IdInsuranceLine).Description;
                            string insuranceSubline = _unitOfWork.InsuranceSubline.GetById(policy.Policy.IdInsuranceSubline).Description;
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
                        if (policy.PolicyFees != null && policy.PolicyFees.Count > 0 && policy.Policy.IdPaymentMethod != "4")
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
                            if (policy.Policy.IdPaymentMethod == "4")
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
                        if (policy.Policy.IdPaymentMethod == "2" && policy.Policy.StartDate.HasValue)
                        {
                            //Primero se debe eliminar las existentes
                            _unitOfWork.PolicyFeeFinancial.DeleteFeeByPolicy(idPolicy);
                            DateTime start = new DateTime(policy.Policy.StartDate.Value.Year, policy.Policy.StartDate.Value.Month, policy.Policy.Payday);
                            for (int i = 1; i <= policy.Policy.FeeNumbers; i++)
                            {
                                PolicyFeeFinancial policyFeeFinancial = new PolicyFeeFinancial
                                {
                                    Number = i,
                                    IdPolicy = idPolicy,
                                    Date = policy.Policy.StartDate.Value.AddMonths(i - 1),
                                    Value = policy.Policy.FeeValue,
                                    DateInsurance = start.AddMonths(i - 1),
                                    DatePayment = start.AddMonths(i - 1)
                                };
                                _unitOfWork.PolicyFeeFinancial.Insert(policyFeeFinancial);
                            }
                        }
                        //Cuota incial
                        if (policy.Policy.TotalInitialFee > 0)
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
                        if (policy.Policy.IsOrder)
                        {
                            //Creamos una gestión con la modificación realizada
                            Customer h = _unitOfWork.Customer.GetById(policy.Policy.IdPolicyHolder);
                            string policyHolder = h.FirstName + (string.IsNullOrEmpty(h.MiddleName) ? "" : " " + h.MiddleName) + h.LastName + (string.IsNullOrEmpty(h.MiddleLastName) ? "" : " " + h.MiddleLastName);
                            string movto = _unitOfWork.MovementType.GetList().Where(m => m.Id.Equals(policy.Policy.IdMovementType)).FirstOrDefault().Alias;
                            string insurance = _unitOfWork.Insurance.GetById(policy.Policy.IdInsurance).Description;
                            string insuranceLine = _unitOfWork.InsuranceLine.GetById(policy.Policy.IdInsuranceLine).Description;
                            string insuranceSubline = _unitOfWork.InsuranceSubline.GetById(policy.Policy.IdInsuranceSubline).Description;
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
                            Customer h = _unitOfWork.Customer.GetById(policy.Policy.IdPolicyHolder);
                            string policyHolder = h.FirstName + (string.IsNullOrEmpty(h.MiddleName) ? "" : " " + h.MiddleName) + h.LastName + (string.IsNullOrEmpty(h.MiddleLastName) ? "" : " " + h.MiddleLastName);
                            string movto = _unitOfWork.MovementType.GetList().Where(m => m.Id.Equals(policy.Policy.IdMovementType)).FirstOrDefault().Alias;
                            string insurance = _unitOfWork.Insurance.GetById(policy.Policy.IdInsurance).Description;
                            string insuranceLine = _unitOfWork.InsuranceLine.GetById(policy.Policy.IdInsuranceLine).Description;
                            string insuranceSubline = _unitOfWork.InsuranceSubline.GetById(policy.Policy.IdInsuranceSubline).Description;
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
        [Route("GetPolicyReportProduction")]
        public IActionResult GetPolicyReportProduction([FromBody] GetPolicyReportProduction request)
        {
            try
            {
                var result = _unitOfWork.Policy.PolicyReportProduction(request.IdUser, request.StartDate, request.EndDate);
                return Ok(result);
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
