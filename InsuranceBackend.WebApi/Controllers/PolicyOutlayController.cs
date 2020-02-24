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
    [Route("api/policyOutlay")]
    [Authorize]
    public class PolicyOutlayController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public PolicyOutlayController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetById(int id)
        {
            try
            {
                return Ok(_unitOfWork.PolicyOutlay.GetById(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]PolicyOutlay PolicyOutlay)
        {
            int idPolicyOutlay = 0;
            if (!ModelState.IsValid)
                return BadRequest();
            using (var transaction = new TransactionScope())
            {
                try
                {
                    string idUser = User.Claims.Where(c => c.Type.Equals(ClaimTypes.PrimarySid)).FirstOrDefault().Value;
                    PolicyOutlay.IdUser = int.Parse(idUser);
                    PolicyOutlay.CreationDate = DateTime.Now;
                    idPolicyOutlay = _unitOfWork.PolicyOutlay.Insert(PolicyOutlay);

                    // Debemos volver a crear las cuotas actualizando el día del pago
                    IEnumerable<PolicyFeeList> policyFees = _unitOfWork.PolicyFee.PolicyFeeListByPolicy(PolicyOutlay.IdPolicy, false);
                    // Eliminamos la cuotas existentes
                    _unitOfWork.PolicyFee.DeleteFeeByPolicy(PolicyOutlay.IdPolicy);
                    foreach (PolicyFeeList item in policyFees)
                    {
                        int day = PolicyOutlay.PayDay;
                        if (day.Equals(30) && item.DatePayment.Value.Month.Equals(2))
                        {
                            // Si el mes es febrero se debe tomar el último día del mes
                            day = DateTime.DaysInMonth(item.DatePayment.Value.Year, item.DatePayment.Value.Month);
                        }

                        DateTime datePayment = new DateTime(item.DatePayment.Value.Year, item.DatePayment.Value.Month, day);
                        item.DatePayment = datePayment;

                        // Insertamos las cuotas recalculadas
                        PolicyFee policyFee = new PolicyFee
                        {
                            Number = item.Number,
                            IdPolicy = item.IdPolicy,
                            Date = item.Date,
                            Value = item.Value,
                            DateInsurance = item.DateInsurance,
                            DatePayment = item.DatePayment
                        };
                        _unitOfWork.PolicyFee.Insert(policyFee);
                    }
                    transaction.Complete();
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    return StatusCode(500, "Internal server error: " + ex.Message);
                }
            }
            return Ok(idPolicyOutlay);
        }


        [HttpPut]
        public IActionResult Put([FromBody]PolicyOutlay PolicyOutlay)
        {
            try
            {
                if (ModelState.IsValid && _unitOfWork.PolicyOutlay.Update(PolicyOutlay))
                {
                    return Ok(new { Message = "Desembolso actualizado" });
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
                var PolicyOutlay = _unitOfWork.PolicyOutlay.GetById(id);
                if (PolicyOutlay == null)
                    return NotFound();
                if (_unitOfWork.PolicyOutlay.Delete(PolicyOutlay))
                    return Ok(new { Message = "Desembolso eliminado" });
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