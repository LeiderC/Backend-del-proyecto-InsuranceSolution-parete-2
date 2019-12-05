using System;
using System.Collections.Generic;
using System.Linq;
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
                List<PolicyList> lst = _unitOfWork.Policy.PolicyPagedListSearchTerms(request.Identification, request.Name, request.Number, request.IdCustomer, request.Page, request.Rows).ToList();
                return Ok(_unitOfWork.Policy.PolicyPagedListSearchTerms(request.Identification, request.Name, request.Number, request.IdCustomer, request.Page, request.Rows));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        [HttpGet]
        [Route("GetPaginatedPolicy/{page:int}/{rows:int}")]
        public IActionResult GetPaginatedPolicy(int page, int rows)
        {
            try
            {
                return Ok(_unitOfWork.Policy.PolicyPagedList(page, rows));
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
                    idPolicy = _unitOfWork.Policy.Insert(policy.Policy);
                    //Productos propios
                    if (policy.PolicyProducts.Count > 0)
                    {
                        foreach (var item in policy.PolicyProducts)
                        {
                            PolicyProduct product = item as PolicyProduct;
                            product.IdPolicy = idPolicy;
                            _unitOfWork.PolicyProduct.Insert(product);
                        }
                    }
                    //Datos de vehículo
                    if (policy.Vehicle != null)
                    {
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
                        }
                        else
                            _unitOfWork.Vehicle.Insert(policy.Vehicle);
                    }
                    //Asegurados
                    if (policy.PolicyInsured != null && policy.PolicyInsured.Count > 0)
                    {
                        foreach (var item in policy.PolicyInsured)
                        {
                            PolicyInsured insured = new PolicyInsured
                            {
                                IdInsured = item.Id,
                                IdPolicy = idPolicy
                            };
                            _unitOfWork.PolicyInsured.Insert(insured);
                        }
                    }
                    //Beneficiarios
                    if (policy.PolicyBeneficiaries != null && policy.PolicyBeneficiaries.Count > 0)
                    {
                        foreach (var item in policy.PolicyBeneficiaries)
                        {
                            //Debemos crear priemero el beneficiario si no existe
                            int idBeneficiary = 0;
                            Beneficiary ben = _unitOfWork.Beneficiary.BeneficiaryByIdentification(item.IdentificationNumber);
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
        public IActionResult Put([FromBody]Policy policy)
        {
            try
            {
                if (ModelState.IsValid && _unitOfWork.Policy.Update(policy))
            {
                return Ok(new { Message = "El politica se ha actualizado" });
            }
            else
                return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
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
