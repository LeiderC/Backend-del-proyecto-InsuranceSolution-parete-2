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
                return Ok(_unitOfWork.Policy.GetById(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("GetPolicyBySearchTerms")]
        public IActionResult GetCustomerByIdentification([FromBody]GetPaginatedPolicySearchTerm request)
        {
            try
            {
                int idUser = 0;
                if(request.FindByUserPolicyOrder)
                    idUser = int.Parse(User.Claims.Where(c => c.Type.Equals(ClaimTypes.PrimarySid)).FirstOrDefault().Value);
                return Ok(_unitOfWork.Policy.PolicyPagedListSearchTerms(request.Identification, request.Name, request.Number, request.IdCustomer, idUser, request.IsOrder, request.Page, request.Rows));
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
        public IActionResult Post([FromBody]PolicySave policy)
        {
            int idPolicy = 0;
            if (!ModelState.IsValid)
                return BadRequest();
            using (var transaction = new TransactionScope())
            {
                try
                {
                    //Datos de vehículo
                    if (policy.Vehicle != null)
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
                    if(policy.PolicyFees!=null && policy.PolicyFees.Count>0)
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
                            IsExtra = false,
                        };
                        int idManagement = _unitOfWork.Management.Insert(management);
                        //Creamos la tarea para sistematizar
                        Settings s = _unitOfWork.Settings.GetList().FirstOrDefault();
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
                            DelegatedUser = s.TechnicalUserId,
                            StartDate = DateTime.Now,
                            State = "P",
                            Subject = subjectTask,
                            ManagementPartner = "O",
                            IsExtra = true,
                        };
                        int idTask = _unitOfWork.Management.Insert(task);
                        _unitOfWork.ManagementExtra.Insert(new ManagementExtra { IdManagement = idManagement, IdManagementExtra = idTask });
                    }
                    else
                    {
                        if (policy.PolicyOrderId > 0)
                        {
                            //Primero deshabilitamos la que existe
                            if (_unitOfWork.PolicyOrderDetail.UpdateState(policy.PolicyOrderId, "I"))
                                _unitOfWork.PolicyOrderDetail.Insert(new PolicyOrderDetail { IdPolicyOrder = policy.PolicyOrderId, IdPolicy = idPolicy, CreationDate = DateTime.Now, State = "A" });
                            //Debemos dar la tarea por terminada y crear una gestión indicando la sistematización que se hizo
                            //tarea
                            Management task = _unitOfWork.Management.ManagementByPolicyOrder(policy.PolicyOrderId, "T");
                            task.State = "R";
                            _unitOfWork.Management.Update(task);
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
        public IActionResult Put([FromBody]PolicySave policy)
        {
            int idPolicy = 0;
            if (!ModelState.IsValid)
                return BadRequest();
            using (var transaction = new TransactionScope())
            {
                try
                {
                    //Datos de vehículo
                    if (policy.Vehicle != null)
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
                        if (policy.PolicyFees != null && policy.PolicyFees.Count > 0)
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
    }
}
