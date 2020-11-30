using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Transactions;
using InsuranceBackend.Models;
using InsuranceBackend.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceBackend.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/policyPendingRegistration")]
    [Authorize]
    public class PolicyPendingRegistrationController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public PolicyPendingRegistrationController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(_unitOfWork.PolicyPendingRegistration.GetList());
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] PolicyPendingRegistrationSave policyPendingRegistrationSave)
        {
            int id = 0;
            if (!ModelState.IsValid)
                return BadRequest();
            using (var transaction = new TransactionScope())
            {
                try
                {
                    if (!policyPendingRegistrationSave.PolicyPendingRegistration.CreationDate.HasValue)
                        policyPendingRegistrationSave.PolicyPendingRegistration.CreationDate = DateTime.Now;
                    string idUser = User.Claims.Where(c => c.Type.Equals(ClaimTypes.PrimarySid)).FirstOrDefault().Value;
                    policyPendingRegistrationSave.PolicyPendingRegistration.IdUser = int.Parse(idUser);
                    id = _unitOfWork.PolicyPendingRegistration.Insert(policyPendingRegistrationSave.PolicyPendingRegistration);
                    PolicyList policy = _unitOfWork.Policy.PolicyListById(policyPendingRegistrationSave.PolicyPendingRegistration.IdPolicy);
                    //Debemos guardar el documento digital y asociarlo al recaudo
                    DigitalizedFile digitalizedFile = policyPendingRegistrationSave.DigitalizedFile;
                    if (digitalizedFile != null)
                    {
                        digitalizedFile.IdPolicyOrder = policy.IdPolicyOrder;
                        digitalizedFile.Date = DateTime.Now;
                        digitalizedFile.Description = "MATRICULA VEHÍCULO";
                        _unitOfWork.DigitalizedFile.Insert(digitalizedFile);
                    }

                    transaction.Complete();
                }
                catch (System.Data.SqlClient.SqlException sqlex)
                {
                    transaction.Dispose();
                    if (sqlex.Number.Equals(2601))
                    {
                        return StatusCode(500, "No se puede ingresar el registro de matricula mas de una vez");
                    }
                    return StatusCode(500, "Internal server error: " + sqlex.Message);
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    return StatusCode(500, "Internal server error: " + ex.Message);
                }
            }
            return Ok(id);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var domain = _unitOfWork.PolicyPendingRegistration.GetById(id);
                if (domain == null)
                    return NotFound();
                if (_unitOfWork.PolicyPendingRegistration.Delete(domain))
                    return Ok(new { Message = "La matricula se ha eliminado" });
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
